using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.MenuItem
{
  [ActionHandler("AddMenuItem.ShowCurrentSolution")]
  internal class ShowCurrentSolutionAction : IActionHandler
  {
    #region IActionHandler Members

    public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      // fetch active solution from context
      ISolution solution = context.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);

      // enable this action if there is an active solution, disable otherwise
      return solution != null;
    }

    public void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      // Fetch active solution from context.

      // It should be not null because it is checked in "Update". 
      // "Execute" is guaranteed to not be invoked if "Update" returns false.
      ISolution solution = context.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      if (solution == null)
        return;

      string message = string.Format("Currently active solution is {0}", solution.Name);
      MessageBox.ShowInfo(message, "AddMenuItem Sample Plugin");
    }

    #endregion
  }
}