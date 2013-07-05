using JetBrains.DocumentManagers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;

namespace JetBrains.ReSharper.PowerToys.LiveTemplatesMacro
{
  [MacroImplementation(Definition = typeof(CurrentFilePathMacro))]
  public class CurrentFilePathMacroImpl : SimpleMacroImplementation
  {
    public override HotspotItems GetLookupItems(IHotspotContext context)
    {
      var solution = context.SessionContext.Solution;
      var currentDocument = context.ExpressionRange.Document;

      IProjectFile projectItem = solution.GetComponent<DocumentManager>().GetProjectFile(currentDocument);
      var path = projectItem.Location.FullPath;

      return MacroUtil.SimpleEvaluateResult(path);
    }
  }
}