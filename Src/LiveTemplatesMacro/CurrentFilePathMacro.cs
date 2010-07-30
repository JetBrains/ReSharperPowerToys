using System.Collections.Generic;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using JetBrains.ReSharper.Feature.Services.Lookup;

namespace JetBrains.ReSharper.PowerToys.LiveTemplatesMacro
{
  [Macro("AddLiveTemplatesMacro.CurrentFilePath", // macro name should be unique among all other macros, it's recommended to prefix it with your plugin name to achieve that
    ShortDescription = "Current file path", // description of the macro to be shown in the list of macros
    LongDescription="Evaluates current file path" // long description of the macro to be shown in the area below the list
    )]
  public class CurrentFilePathMacro : IMacro
  {
    #region Implementation

    private static string Evaluate(IHotspotContext context)
    {
      IDocument currentDocument = context.SessionContext.TextControl.Document;
      IProjectFile projectItem = DocumentManager.GetInstance(context.SessionContext.Solution).GetProjectFile(currentDocument);
      return projectItem.Location.FullPath;
    }

    #endregion

    #region IMacro Members

    public string EvaluateQuickResult(IHotspotContext context, IList<string> arguments)
    {
      return Evaluate(context);
    }

    public HotspotItems GetLookupItems(IHotspotContext context, IList<string> arguments)
    {
      return new HotspotItems(new TextLookupItem(Evaluate(context)));
    }

    public string GetPlaceholder()
    {
      return "a";
    }

    public bool HandleExpansion(IHotspotContext context, IList<string> arguments)
    {
      return false;
    }

    public ParameterInfo[] Parameters
    {
      get
      {
        // our macro is parameterless
        return new ParameterInfo[0];
      }
    }

    #endregion
  }
}