using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.Configuration;
using JetBrains.DocumentManagers;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Features.Common.FindResultsBrowser;
using JetBrains.ReSharper.Psi;
using JetBrains.UI.Application;

namespace JetBrains.ReSharper.PowerToys.FindText
{
  /// <summary>
  /// Handles FindText action, see Actions.xml
  /// </summary>
  [ActionHandler]
  public class PowerToys_FindTextAction : IActionHandler
  {
    private GlobalSettingsTable globalSettingsTable;
    private readonly IMainWindow mainWindow;
    private readonly DocumentManager documentManager;
    private readonly IPsiServices psiServices;

    public PowerToys_FindTextAction(IMainWindow mainWindow, DocumentManager documentManager, IPsiServices psiServices)
    {
      this.mainWindow = mainWindow;
      this.documentManager = documentManager;
      this.psiServices = psiServices;
    }

    public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      // Check that we have a solution
      return context.CheckAllNotNull(DataConstants.SOLUTION);
    }

    public void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      // Get solution from context in which action is executed
      ISolution solution = context.GetData(DataConstants.SOLUTION);
      if (solution == null)
        return;

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