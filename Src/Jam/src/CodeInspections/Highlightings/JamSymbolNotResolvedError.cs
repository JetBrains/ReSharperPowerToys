using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Resolve;

[assembly: RegisterConfigurableSeverity("JamSymbolNotResolvedError", null, HighlightingGroupIds.CodeSmell, "Unknown symbol", @"Cannot resolve symbol with specified name.", Severity.ERROR, false, Internal = false)]

namespace JetBrains.ReSharper.Psi.Jam.CodeInspections.Highlightings
{
  [ConfigurableSeverityHighlighting("JamSymbolNotResolvedError", JamLanguage.Name, AttributeId = HighlightingAttributeIds.UNRESOLVED_ERROR_ATTRIBUTE, OverlapResolve = OverlapResolveKind.UNRESOLVED_ERROR, ToolTipFormatString = "Cannot resolve symbol '{0}'")]
  public class JamSymbolNotResolvedError : JamHighlightingBase, IHighlightingWithRange
  {
    private readonly string myMessage;
    private readonly IReference myReference;

    public JamSymbolNotResolvedError(IReference reference)
    {
      myReference = reference;
      myMessage = string.Format("Cannot resolve symbol '{0}'", Reference.GetName());
    }

    public IReference Reference
    {
      get { return myReference; }
    }

    public string ToolTip
    {
      get { return myMessage; }
    }

    public string ErrorStripeToolTip
    {
      get { return ToolTip; }
    }

    public int NavigationOffsetPatch
    {
      get { return 0; }
    }

    public override bool IsValid()
    {
      return Reference != null && Reference.IsValid();
    }

    DocumentRange IHighlightingWithRange.CalculateRange()
    {
      return Reference.GetDocumentRange();
    }
  }
}