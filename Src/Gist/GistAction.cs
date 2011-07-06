using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;

namespace JetBrains.ReSharper.PowerToys.Gist
{
  [ActionHandler("PowerToys.Gist")]
  public class GistAction : IActionHandler
  {
    public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      return context.CheckAnyNotNull(ProjectModel.DataContext.DataConstants.PROJECT_MODEL_ELEMENTS, IDE.DataConstants.DOCUMENT_SELECTION);
    }

    public void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      var documentSelection = context.GetData(IDE.DataConstants.DOCUMENT_SELECTION);
      if (documentSelection != null)
      {
        // Publish selected text
        return;
      }

      var projectModelElements = context.GetData(ProjectModel.DataContext.DataConstants.PROJECT_MODEL_ELEMENTS);
      if (projectModelElements != null)
      {
        // Publish selected files
        return;
      }
    }
  }
}
