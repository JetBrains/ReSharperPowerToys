using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.MenuItem
{
  /// <summary>
  /// The action ID is deduced from the class name (but for the “Action” or “ActionHandler” suffix), dots are replaced with underscores.
  /// </summary>
  [ActionHandler]
  internal class AddMenuItem_ShowCurrentSolutionAction : IActionHandler
  {
    #region IActionHandler Members

    public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      // fetch active solution from context
      ISolution solution = context.GetData(JetBrains.ProjectModel.DataContext.DataConstants.SOLUTION);

      // enable this action if there is an active solution, disable otherwise
      return solution != null;
    }

    public void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      // Fetch active solution from context.
      // It should be not null because it is checked in "Update". 
      // "Execute" is guaranteed to not be invoked if "Update" returns false.
      ISolution solution = context.GetData(JetBrains.ProjectModel.DataContext.DataConstants.SOLUTION);

      string message = string.Format("Currently active solution is {0}", solution.Name);
      MessageBox.ShowInfo(message, "AddMenuItem Sample Plugin");
    }

    #endregion
  }
}