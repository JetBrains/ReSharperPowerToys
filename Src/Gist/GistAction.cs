using System.Collections.Generic;
using System.Drawing;
using System.Net;
using JetBrains.ActionManagement;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.Communication;
using JetBrains.Application.DataContext;
using JetBrains.DocumentManagers;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.PowerToys.Gist.GitHub;
using JetBrains.ReSharper.Psi;
using JetBrains.TextControl;
using JetBrains.UI;
using JetBrains.UI.PopupWindowManager;
using JetBrains.UI.RichText;
using JetBrains.UI.Tooltips;
using JetBrains.Util;
using JetBrains.Util.Special;
using System.Linq;
using RestSharp;

namespace JetBrains.ReSharper.PowerToys.Gist
{
  [ActionHandler("PowerToys.Gist")]
  public class GistAction : IActionHandler
  {
    public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      return
        context.CheckAllNotNull(ProjectModel.DataContext.DataConstants.SOLUTION, ProjectModel.DataContext.DataConstants.PROJECT_MODEL_ELEMENTS) ||
        context.CheckAllNotNull(ProjectModel.DataContext.DataConstants.SOLUTION, IDE.DataConstants.DOCUMENT_SELECTION);
    }

    public void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      var shell = Shell.Instance;
      var solution = context.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      Assertion.AssertNotNull(solution, "solution == null");

      IDictionary<string, string> publishData = null;
      // Publish selected text
      var documentSelection = context.GetData(IDE.DataConstants.DOCUMENT_SELECTION);
      if (documentSelection != null)
      {
        var filename = documentSelection.Document.GetPsiSourceFile(solution).IfNotNull(_ => _.Name);
        var text = documentSelection.Document.GetText(documentSelection.TextRange);
        publishData = new Dictionary<string, string> { { filename, text } };
      }

      // Publish selected files
      var projectModelElements = context.GetData(ProjectModel.DataContext.DataConstants.PROJECT_MODEL_ELEMENTS);
      if ((publishData == null) && (projectModelElements != null))
      {
        var documentManager = DocumentManager.GetInstance(solution);
        publishData = projectModelElements
          .OfType<IProjectFile>()
          .Concat(projectModelElements.OfType<IProjectFolder>().SelectMany(_ => _.GetAllProjectFiles()))
          .Distinct()
          .ToDictionary(_ => _.Name, _ => documentManager.GetOrCreateDocument(_).GetText());
      }

      if (publishData == null) return;

      var url = Publish(shell.GetComponent<WebProxySettingsReader>().GetProxySettings(context), publishData);
      
      if (!string.IsNullOrEmpty(url))
      {
        shell.GetComponent<Clipboard>().SetDataObject(url);
        ShowTooltip(context, shell, new RichText("Url ").Append(new RichText(url, TextStyle.FromForeColor(Color.Blue))).Append(" copied to clipboard", TextStyle.Default));
      }
      else
      {
        ShowTooltip(context, shell, "Error posting Gist");
      }
    }

    private static void ShowTooltip(IDataContext context, Shell shell, RichText tooltip)
    {
      var tooltipManager = shell.GetComponent<ITooltipManager>();
      tooltipManager.Show(tooltip,
        lifetime =>
        {
          var windowContextSource = context.GetData(UI.DataConstants.PopupWindowContextSource);

          if (windowContextSource != null)
          {
            var windowContext = windowContextSource.Create(lifetime);
            var ctxTextControl = windowContext as TextControlPopupWindowContext;
            return ctxTextControl == null ? windowContext :
              ctxTextControl.OverrideLayouter(lifetime, lifetimeLayouter => new DockingLayouter(lifetimeLayouter, new TextControlAnchoringRect(lifetimeLayouter, ctxTextControl.TextControl, ctxTextControl.TextControl. Caret.Offset(), shell.Locks), Anchoring2D.AnchorTopOrBottom));
          }

          return shell.GetComponent<MainWindowPopupWindowContext>().Create(lifetime);
        });
    }

    [CanBeNull]
    private string Publish(IWebProxy proxy, IDictionary<string, string> content)
    {
      var client = new Client { Proxy = proxy };
      var response = client.Execute<GitHub.Gist>(new RestRequest("/gists", Method.POST) {  RequestFormat = DataFormat.Json }
        .AddBody(new GitHub.Gist { IsPublic = true, Files = content.ToDictionary(_ => _.Key, _ => new GistFile { Content = _.Value }) }));
      return response.Data != null ? response.Data.HtmlUrl : null;
    }
  }
}
