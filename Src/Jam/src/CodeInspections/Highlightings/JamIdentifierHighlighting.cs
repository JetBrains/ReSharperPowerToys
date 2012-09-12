using System;
using JetBrains.ReSharper.Daemon;

namespace JetBrains.ReSharper.Psi.Jam.CodeInspections.Highlightings
{
  [DaemonTooltipProvider(typeof(JamIdentifierTooltipProvider))]
  [StaticSeverityHighlighting(Severity.INFO, HighlightingGroupIds.IdentifierHighlightingsGroup, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = false)]
  public sealed class JamIdentifierHighlighting : ICustomAttributeIdHighlighting
  {
    private readonly string myAtributeId;

    public JamIdentifierHighlighting(string attributeId)
    {
      myAtributeId = attributeId;
    }

    public string AttributeId
    {
      get { return myAtributeId; }
    }

    public string ErrorStripeToolTip
    {
      get { return ToolTip; }
    }

    public string ToolTip
    {
      get { return String.Empty; }
    }

    public int NavigationOffsetPatch
    {
      get { return 0; }
    }

    public bool IsValid()
    {
      return true;
    }
  }
}