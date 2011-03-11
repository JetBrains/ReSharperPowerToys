using JetBrains.ActionManagement;
using JetBrains.ReSharper.Refactorings.Workflow;
using JetBrains.UI.RichText;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric
{
  /// <summary>
  /// Refactoring action uses refactoring workflow to implement <see cref="IActionHandler"/> interface
  /// </summary>
  [ActionHandler]
  internal class MakeMethodGenericAction : ExtensibleRefactoringAction<MakeMethodGenericWorkflowProvider>
  {
    protected override RichText Caption
    {
      get { return new RichText("Make Method Generic"); }
    }
  }
}