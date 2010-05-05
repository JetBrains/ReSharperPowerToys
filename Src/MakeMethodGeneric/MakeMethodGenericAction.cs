using JetBrains.ActionManagement;
using JetBrains.ReSharper.Refactorings.Workflow;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric
{
  /// <summary>
  /// Refactoring action uses refactoring workflow to implement <see cref="IActionHandler"/> interface
  /// </summary>
  [ActionHandler]
  internal class MakeMethodGenericAction : RefactoringAction
  {
    public MakeMethodGenericAction() : base(solution => new MakeMethodGenericWorkflow(solution))
    {
    }
  }
}