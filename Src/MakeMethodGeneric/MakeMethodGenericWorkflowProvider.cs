using JetBrains.ProjectModel;
using JetBrains.ReSharper.Refactorings.Workflow;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric
{
	[RefactoringWorkflowProviderAttribute(typeof(MakeMethodGenericWorkflowProvider))]
	public class MakeMethodGenericWorkflowProvider : IRefactoringWorkflowProvider
	{
		#region IRefactoringWorkflowProvider Members

		public IRefactoringWorkflow CreateWorkflow(ISolution solution)
		{
			return new MakeMethodGenericWorkflow(solution);
		}

		public RefactoringActionGroup ActionGroup
		{
			get
			{
				return RefactoringActionGroup.Unsorded;
			}
		}

		#endregion
	}
}