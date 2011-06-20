using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.Application.Configuration;
using JetBrains.Application.DataContext;
using JetBrains.DocumentManagers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Features.Common.FindResultsBrowser;
using JetBrains.ReSharper.Psi;
using JetBrains.UI.Application;

namespace JetBrains.ReSharper.PowerToys.FindText
{
  /// <summary>
  /// Handles FindText action, see Actions.xml
  /// </summary>
  [ActionHandler("PowerToys.FindText")]
  public class PowerToys_FindTextAction : IActionHandler
  {
    public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      // Check that we have a solution
      return context.CheckAllNotNull(ProjectModel.DataContext.DataConstants.SOLUTION);
    }

    public void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      // Get solution from context in which action is executed
      ISolution solution = context.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      if (solution == null)
        return;

      var documentManager = solution.GetComponent<DocumentManager>();
      var globalSettingsTable = Shell.Instance.GetComponent<GlobalSettingsTable>();
      var psiServices = solution.GetPsiServices();
      var mainWindow = solution.GetComponent<IMainWindow>();
      
      // Ask user about search string
      using (var dialog = new EnterSearchStringDialog(globalSettingsTable))
      {
        if (dialog.ShowDialog(mainWindow) == DialogResult.OK)
        {
          // Create request, descriptor, perform search and show results 
          var searchRequest = new FindTextSearchRequest(solution, dialog.SearchString, dialog.CaseSensitive, dialog.SearchFlags, documentManager, psiServices);
          var descriptor = new FindTextDescriptor(searchRequest);
          descriptor.Search();
          FindResultsBrowser.ShowResults(descriptor);
        }
      }
    }
  }
}