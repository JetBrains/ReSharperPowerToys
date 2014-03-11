/*
 * Copyright 2007-2014 JetBrains
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.PowerToys.MakeMethodGeneric.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Refactorings;
using JetBrains.ReSharper.Refactorings.Common;
using JetBrains.ReSharper.Refactorings.Conflicts;
using JetBrains.ReSharper.Refactorings.OverridesSupport;
using JetBrains.ReSharper.Refactorings.Rename;
using JetBrains.ReSharper.Refactorings.Workflow;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric
{
  /// <summary>
  /// Contains code that execute PSI transaction. 
  /// </summary>
  public class MakeMethodGenericRefactoring : DrivenRefactoring<MakeMethodGenericWorkflow, MakeMethodGenericBase>
  {
    private readonly SearchDomainFactory mySearchDomainFactory;

    public MakeMethodGenericRefactoring(MakeMethodGenericWorkflow workFlow, ISolution solution,
                                        IRefactoringDriver driver,
                                        SearchDomainFactory searchDomainFactory) : base(workFlow, solution, driver)
    {
      mySearchDomainFactory = searchDomainFactory;
    }

    public IMethod Method { get; private set; }
    public IParameter Parameter { get; private set; }

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

      IPsiServices services = Parameter.GetPsiServices();

      IReference[] referencesToParameter;
      IReference[] referencesToRootOverrides;

      // search for method overrides (OverridesFinder is util class that 
      // allows to find all overrides and problems with quasi implementations)
      OverridesFinder overridesFinder = OverridesFinder.CreateInstance(Method);
      using (var subPi = new SubProgressIndicator(pi, 1))
        overridesFinder.Find(subPi);

      JetHashSet<HierarchyMember> hierarchyMembers = overridesFinder.Overrides;

      List<IMethod> methods = ScanHierarchyConflicts(hierarchyMembers).ToList();
      List<IParameter> parameters = GetAllParameters(methods).ToList();

      // find parameters and methods usages...
      using (var subPi = new SubProgressIndicator(pi, 1))
      {
        subPi.TaskName = "Searching parameter usages:";
        IEnumerable<IPsiSourceFile> projectFiles = from param in parameters
                                                   let projectFilesOfOneParameter = param.GetSourceFiles()
                                                   from projectFile in projectFilesOfOneParameter
                                                   select projectFile;
        ISearchDomain searchDomain = mySearchDomainFactory.CreateSearchDomain(projectFiles.ToList());
        referencesToParameter = services.Finder.FindReferences(parameters, searchDomain, subPi);
      }

      using (var subPi = new SubProgressIndicator(pi, 1))
      {
        subPi.TaskName = "Searching method usages:";
        referencesToRootOverrides = services.Finder.FindReferences(methods,
                                                                   mySearchDomainFactory.CreateSearchDomain(Solution,
                                                                                                          false), subPi);
      }

      // this step processes method usages, removes argument and stores reference specific data to the 'MethodInvocation'.
      List<MethodInvocation> usages;
      using (var subPi = new SubProgressIndicator(pi, 1))
        usages = PreProcessMethodUsages(referencesToRootOverrides, subPi).ToList();

      // replace usages of parameters with typeof(TNewTypeParameter) expression. 
      using (var subPi = new SubProgressIndicator(pi, 1))
        ProcessParameterUsages(referencesToParameter, subPi);

      // Remove parameters from method declarations and insert new type parmeter. Map contains method -> new type parameter relation. 
      Dictionary<IMethod, ITypeParameter> map = UpdateDeclarations(methods);

      // We have changed declarations. cashes should be updated)
      Solution.GetPsiServices().Caches.Update();

      // Process method usages one more time to insert correct type arguments to the call.
      using (var subPi = new SubProgressIndicator(pi, 1))
        BindUsages(usages, map, subPi);

      return true;
    }

    private IEnumerable<IParameter> GetAllParameters(IEnumerable<IMethod> overrides)
    {
      int index = Method.Parameters.IndexOf(Parameter);
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
      foreach (HierarchyMember hierarchyMember in hierarchyMembers)
      {
        // paranoiac check 
        var member = hierarchyMember.Member as IMethod;
        if (member != null)
        {
          members.Add(member);
          conflictConsumer.AddConflictsForHierarhyMember(hierarchyMember);
        }
      }
      foreach (HierarchyConflict hierarchyConflict in conflictConsumer.MyHierarchyConflicts)
      {
        IConflict conflict = hierarchyConflict.CreateConflict(provider);
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
      foreach (IReference reference in references)
      {
        // reference can be invalid if previous changes affected it's element. It is unlikely to be occured...
        if (reference.IsValid())
          // process reference with language specific implementation...
          Exec[reference.GetTreeNode().Language].ProcessParameterReference(reference);
        pi.Advance(1);
      }
    }

    private void BindUsages(ICollection<MethodInvocation> usages, IDictionary<IMethod, ITypeParameter> map,
                            IProgressIndicator pi)
    {
      if (usages.Count == 0)
        return;

      pi.Start(usages.Count);
      foreach (MethodInvocation usage in usages)
      {
        if (usage.IsValid())
        {
          ITypeParameter typeParameter;
          if (map.TryGetValue(usage.Method, out typeParameter))
          {
            // one more call to language specific code...
            Exec[usage.Reference.GetTreeNode().Language].BindUsage(usage, typeParameter);
          }
          else
          {
            Exec[usage.Reference.GetTreeNode().Language].BindUsage(usage, null);
          }
        }
        pi.Advance(1);
      }
    }

    private Dictionary<IMethod, ITypeParameter> UpdateDeclarations(IEnumerable<IMethod> methods)
    {
      int index = Method.Parameters.IndexOf(Parameter);
      var map = new Dictionary<IMethod, ITypeParameter>();
      foreach (IMethod method in methods)
      {
        ITypeParameter parameter = null;
        foreach (IDeclaration declaration in method.GetDeclarations())
        {
          // methods can have multiple declarations (partial methods). 
          Exec[declaration.Language].RemoveParameter(declaration, index);
          parameter = Exec[declaration.Language].AddTypeParameter(declaration);
        }
        if (parameter != null)
        {
          map.Add(method, parameter);
        }
      }
      return map;
    }

    private IEnumerable<MethodInvocation> PreProcessMethodUsages(ICollection<IReference> references,
                                                                 IProgressIndicator pi)
    {
      if (references.Count == 0)
        yield break;

      pi.Start(references.Count);
      foreach (IReference reference in references)
      {
        MethodInvocation usage = Exec[reference.GetTreeNode().Language].ProcessUsage(reference);
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