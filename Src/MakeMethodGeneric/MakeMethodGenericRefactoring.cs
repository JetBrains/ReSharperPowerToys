using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.PowerToys.MakeMethodGeneric.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Refactorings;
using JetBrains.ReSharper.Refactorings.Common;
using JetBrains.ReSharper.Refactorings.Conflicts;
using JetBrains.ReSharper.Refactorings.OverridesSupport;
using JetBrains.ReSharper.Refactorings.Workflow;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric
{
  /// <summary>
  /// Contains code that execute PSI transaction. 
  /// </summary>
  public class MakeMethodGenericRefactoring : DrivenRefactoring<MakeMethodGenericWorkflow, MakeMethodGenericBase>
  {
    public IMethod Method { get; private set; }
    public IParameter Parameter { get; private set; }

    public MakeMethodGenericRefactoring(MakeMethodGenericWorkflow workFlow, ISolution solution, IRefactoringDriver driver) : base(workFlow, solution, driver)
    {
    }

    /// <summary>
    /// This code changes PSI documents. It is executed usder PSI transaction, Command cookies, Reentrancy guard ets. 
    /// All documents are committed (PSI is valid). 
    /// </summary>
    public override bool Execute(IProgressIndicator pi)
    {
      pi.Start(6);

      //check if data stored in workflow is valid...
      Method = Workflow.MethodPointer.FindDeclaredElement();
      Parameter = Workflow.ParameterPointer.FindDeclaredElement();

      if (Method == null || Parameter == null)
        return false;

      var manager = Parameter.GetManager();

      IReference[] referencesToParameter;
      IReference[] referencesToRootOverrides;

      // search for method overrides (OverridesFinder is util class that 
      // allows to find all overrides and problems with quasi implementations)
      var overridesFinder = OverridesFinder.CreateInstance(Method);
      using (var subPi = new SubProgressIndicator(pi, 1))
        overridesFinder.Find(subPi);

      var hierarchyMembers = overridesFinder.Overrides;

      var methods = ScanHierarchyConflicts(hierarchyMembers).ToList();
      var parameters = GetAllParameters(methods).ToList();

      // find parameters and methods usages...
      using (var subPi = new SubProgressIndicator(pi, 1))
      {
        subPi.TaskName = "Searching parameter usages:";
        var projectFiles = from param in parameters 
                           let projectFilesOfOneParameter = param.GetProjectFiles() 
                           from projectFile in projectFilesOfOneParameter 
                           select projectFile;
        var searchDomain = SearchDomainFactory.Instance.CreateSearchDomain(projectFiles.ToList());
        referencesToParameter = manager.Finder.FindReferences(parameters, searchDomain, subPi);
      }

      using (var subPi = new SubProgressIndicator(pi, 1))
      {
        subPi.TaskName = "Searching method usages:";
        referencesToRootOverrides = manager.Finder.FindReferences(methods, SearchDomainFactory.Instance.CreateSearchDomain(Solution, false), subPi);
      }

      // this step processes method usages, removes argument and stores reference specific data to the 'MethodInvocation'.
      List<MethodInvocation> usages;
      using (var subPi = new SubProgressIndicator(pi, 1))
        usages = PreProcessMethodUsages(referencesToRootOverrides, subPi).ToList();

      // replace usages of parameters with typeof(TNewTypeParameter) expression. 
      using (var subPi = new SubProgressIndicator(pi, 1))
        ProcessParameterUsages(referencesToParameter, subPi);

      // Remove parameters from method declarations and insert new type parmeter. Map contains method -> new type parameter relation. 
      var map = UpdateDeclarations(methods);

      // We have changed declarations. cashes should be updated)
      PsiManager.GetInstance(Solution).UpdateCaches();

      // Process method usages one more time to insert correct type arguments to the call.
      using (var subPi = new SubProgressIndicator(pi, 1))
        BindUsages(usages, map, subPi);

      return true;
    }

    private IEnumerable<IParameter> GetAllParameters(IEnumerable<IMethod> overrides)
    {
      var index = Method.Parameters.IndexOf(Parameter);
      return from method in overrides
             let parameters = method.Parameters
             where index < parameters.Count
             select parameters[index];
    }

    private List<IMethod> ScanHierarchyConflicts(IEnumerable<HierarchyMember> hierarchyMembers)
    {
      var members = new List<IMethod>();
      var provider = new MakeGenericHierarchyConflictTextProvider();
      var conflictConsumer = new HierarchyConflictConsumer();
      foreach (var hierarchyMember in hierarchyMembers)
      {
        // paranoiac check 
        var member = hierarchyMember.Member as IMethod;
        if (member != null)
        {
          members.Add(member);
          conflictConsumer.AddConflictsForHierarhyMember(hierarchyMember);
        }
      }
      foreach (var hierarchyConflict in conflictConsumer.MyHierarchyConflicts)
      {
        var conflict = hierarchyConflict.CreateConflict(provider);
        if (conflict != null)
          Driver.AddConflict(conflict);
      }
      return members;
    }

    private void ProcessParameterUsages(ICollection<IReference> references, IProgressIndicator pi)
    {
      // can not start progress indicator with zero count.
      if (references.Count == 0)
        return;

      pi.Start(references.Count);
      foreach (var reference in references)
      {
        // reference can be invalid if previous changes affected it's element. It is unlikely to be occured...
        if (reference.IsValid())
          // process reference with language specific implementation...
          myExec[reference.GetElement().Language].ProcessParameterReference(reference);
        pi.Advance(1);
      }
    }

    private void BindUsages(ICollection<MethodInvocation> usages, IDictionary<IMethod, ITypeParameter> map, IProgressIndicator pi)
    {
      if (usages.Count == 0)
        return;

      pi.Start(usages.Count);
      foreach (var usage in usages)
      {
        if (usage.IsValid())
        {
          ITypeParameter typeParameter;
          if (map.TryGetValue(usage.Method, out typeParameter))
          {
            // one more call to language specific code...
            myExec[usage.Reference.GetElement().Language].BindUsage(usage, typeParameter);
          }
          else
          {
            myExec[usage.Reference.GetElement().Language].BindUsage(usage, null);
          }
        }
        pi.Advance(1);
      }
    }

    private Dictionary<IMethod, ITypeParameter> UpdateDeclarations(IEnumerable<IMethod> methods)
    {
      var index = Method.Parameters.IndexOf(Parameter);
      var map = new Dictionary<IMethod, ITypeParameter>();
      foreach (var method in methods)
      {
        ITypeParameter parameter = null;
        foreach (var declaration in method.GetDeclarations())
        {
          // methods can have multiple declarations (partial methods). 
          myExec[declaration.Language].RemoveParameter(declaration, index);
          parameter = myExec[declaration.Language].AddTypeParameter(declaration);
        }
        if (parameter != null)
        {
          map.Add(method, parameter);
        }        
      }
      return map;
    }

    private IEnumerable<MethodInvocation> PreProcessMethodUsages(ICollection<IReference> references, IProgressIndicator pi)
    {
      if (references.Count == 0)
        yield break;

      pi.Start(references.Count);
      foreach (var reference in references)
      {
        var usage = myExec[reference.GetElement().Language].ProcessUsage(reference);
        if (usage != null)
          yield return usage;
        pi.Advance(1);
      }
    }

    /// <summary>
    /// Create language specific part of refactoring.
    /// Filter your languare specific service and instantiate part...
    /// </summary>
    protected override MakeMethodGenericBase CreateRefactoring(IRefactoringLanguageService service)
    {
      var refactoringsLanguageService = service as PowerToyRefactoringsLanguageService;
      if (refactoringsLanguageService != null)
      {
        return refactoringsLanguageService.CreateMakeMethodGeneric(Workflow, Solution, Driver);
      }
      return null;
    }

    protected override MakeMethodGenericBase CreateUnsupportedRefactoring()
    {
      return new MakeMethodGenericUnsupported(Workflow, Solution, Driver);
    }
  }
}