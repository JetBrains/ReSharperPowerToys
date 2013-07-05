using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Services;

namespace JetBrains.ReSharper.PowerToys.LiveTemplatesMacro
{
  [MacroImplementation(Definition = typeof(MethodResultTypeMacro))]
  public class MethodResultTypeMacroImpl : SimpleMacroImplementation
  {
    public override HotspotItems GetLookupItems(IHotspotContext context)
    {
      var document = context.ExpressionRange.Document;
      if (document == null)
        return null;

      var method = TextControlToPsi.GetContainingTypeOrTypeMember(context.SessionContext.Solution, document, context.ExpressionRange.StartOffsetRange().TextRange.StartOffset) as IMethod;
      if (method != null)
      {
        var lookupItems = new List<ILookupItem>();
        var methodReturnTypeName = method.ReturnType.GetPresentableName(method.PresentationLanguage);
        var item = new TextLookupItem(methodReturnTypeName);
        lookupItems.Add(item);
        var hotSpotItems = new HotspotItems(lookupItems);
        return hotSpotItems;
      }

      return null;
    }
  }
}