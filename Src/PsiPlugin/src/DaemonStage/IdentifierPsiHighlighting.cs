using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Daemon;

namespace JetBrains.ReSharper.PsiPlugin.DaemonStage
{
  [DaemonTooltipProvider(typeof(PsiIdentifierTooltipProvider))]
  [StaticSeverityHighlighting(Severity.INFO, HighlightingGroupIds.IdentifierHighlightingsGroup, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = false)]
  public sealed class IdentifierPsiHighlighting : ICustomAttributeIdHighlighting
  {
    private readonly string myAtributeId;

    public string ToolTip
    {
      get { return String.Empty; }
    }

    public string ErrorStripeToolTip
    {
      get { return ToolTip; }
    }

    public string AttributeId
    {
      get { return myAtributeId; }
    }

    public int NavigationOffsetPatch
    {
      get { return 0; }
    }

    public IdentifierPsiHighlighting(string attributeId)
    {
      myAtributeId = attributeId;
    }

    public bool IsValid()
    {
      return true;
    }
  }
}
