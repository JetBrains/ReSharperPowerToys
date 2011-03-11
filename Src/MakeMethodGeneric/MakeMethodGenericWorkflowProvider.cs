using System.Collections.Generic;
using JetBrains.ActionManagement;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Refactorings.Workflow;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric
{
  [RefactoringWorkflowProviderAttribute]
  public class MakeMethodGenericWorkflowProvider : IRefactoringWorkflowProvider
  {
    public RefactoringActionGroup ActionGroup
    {
      get { return RefactoringActionGroup.Unsorded; }
    }

    #region IRefactoringWorkflowProvider Members

    public IEnumerable<IRefactoringWorkflow> CreateWorkflow(IDataContext dataContext)
    {
      return null;
    }

    #endregion

    public IRefactoringWorkflow CreateWorkflow(ISolution solution, string actionId, SearchDomainFactory factory)
    {
      return new MakeMethodGenericWorkflow(solution, actionId, factory);
    }
  }
}