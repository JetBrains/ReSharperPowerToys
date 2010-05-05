using JetBrains.ActionManagement;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Naming;
using JetBrains.ReSharper.Psi.Naming.Extentions;
using JetBrains.ReSharper.Psi.Naming.Settings;
using JetBrains.ReSharper.Psi.Services;
using JetBrains.ReSharper.Refactorings.Workflow;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric
{
  /// <summary>
  /// </summary>
  public class MakeMethodGenericWorkflow : DrivenRefactoringWorkflow
  {
    [NotNull]
    public string TypeParameterName { get; set; }
    [NotNull]
    public IDeclaredElementPointer<IMethod> MethodPointer { get; private set; }
    [NotNull]
    public IDeclaredElementPointer<IParameter> ParameterPointer { get; private set; }

    public MakeMethodGenericWorkflow(ISolution solution) : base(solution)
    {
    }

    /// <summary>
    /// Execution of refatoring starts here. Data from context is initialized. 
    /// </summary>
    public override bool Initialize(IDataContext context)
    {
      IParameter parameter;
      IMethod method;
      if (!IsAvailableInternal(context, out parameter, out method))
        return false;

      // Never reference DeclaredElements, References, Types, etc. from workflow. Use pointers!
      // Code can be edited during execution end elements (types, references) can be invalidated. 
      MethodPointer = method.CreateElementPointer();
      ParameterPointer = parameter.CreateElementPointer();

      // use also:
      // PsiManager.GetInstance(Solution).CreateReferencePointer(...);
      // PsiManager.GetInstance(Solution).CreateTreeElementPointer(...);

      // following code produces name for type parameter using parameter name...
      var namingManager = method.GetManager().Naming;
      var suggestionOptions = new SuggestionOptions() {DefaultName = "T"};
      TypeParameterName = namingManager.Suggestion.GetDerivedName(parameter, NamedElementKinds.TypeParameters, ScopeKind.Common, method.Language, suggestionOptions); 
      return true;
    }

    /// <summary>
    /// This method is used by refactoring action handler. Should be as quick as possible. 
    /// </summary>
    public override bool IsAvailable(IDataContext context)
    {
      IParameter parameter;
      IMethod method;
      if (!IsAvailableInternal(context, out parameter, out method))
        return false;

      return true;
    }

    private bool IsAvailableInternal(IDataContext context, out IParameter systemTypeParameter, out IMethod method)
    {
      systemTypeParameter = null;

      method = context.GetData(DataConstants.DECLARED_ELEMENT) as IMethod;
      if (method == null)
        return false;

      if (method is ICompiledElement)
        return false;

      var declarations = method.GetDeclarations();
      if (declarations.Count == 0)
        return false; 

      var parameters = method.Parameters;
      if (parameters.Count == 0)
        return false;

      var module = method.Module;
      if (module == null)
        return false; 
     
      var systemType = TypeFactory.CreateTypeByCLRName("System.Type", module);

      foreach (var parameter in parameters)
        if (parameter.Type.Equals(systemType))
          systemTypeParameter = parameter;

      if (systemTypeParameter == null)
        return false;

      return true;
    }

    /// <summary>
    /// Last step of refactoring. This code is executed when all changes are made and PSI transaction is committed.
    /// Usal actions here are: project model changes (e.g. file rename), textual changes in documents. 
    /// </summary>
    public override void PostExecute(IProgressIndicator progressIndicator)
    {
    }

    public override string HelpKeyword
    {
      get
      {
        //
        return null;
      }
    }

    /// <summary>
    /// UI page to be shown first. return 'null' to start refactoring immediately. 
    /// </summary>
    public override IRefactoringPage FirstPendingRefactoringPage
    {
      get
      {
        return new MakeMethodGenericPage(this);
      }
    }

    /// <summary>
    /// This property determines whether to show 'enable undo...' checkbox in wizard form.
    /// </summary>
    public override bool MightModifyManyDocuments
    {
      get
      {
        return true;
      }
    }

    public override string Title
    {
      get
      {
        return "Make Method Generic";
      }
    }

    /// <summary>
    /// Returns refactoring class that executed PSI transaction.
    /// </summary>
    public override IRefactoringExecuter CreateRefactoring(IRefactoringDriver driver)
    {
      return new MakeMethodGenericRefactoring(this, Solution, driver);
    }

    public bool IsValid()
    {
      return MethodPointer.FindDeclaredElement() != null && 
             ParameterPointer.FindDeclaredElement() != null;
    }
  }
}