using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("Identifier", null, HighlightingGroupIds.CodeSmell, "Identifier",
    @"Identifier", Severity.INFO, false, Internal = false)]

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services
{
  //[StaticSeverityHighlighting(Severity.INFO, HighlightingGroupIds.IdentifierHighlightingsGroup, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = false)]
     [ConfigurableSeverityHighlighting("Identifier", "PSI", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = "Identifier")]
  internal class PsiIdentifierHighlighting : ICustomAttributeIdHighlighting, IHighlightingWithRange
  {
    private ITreeNode myElement;
    private readonly string myAtributeId = HighlightingAttributeIds.METHOD_IDENTIFIER_ATTRIBUTE;

    public PsiIdentifierHighlighting(ITreeNode element)
    {
      myElement = element;
    }

    public bool IsValid()
    {
      return true;
    }

    public string ToolTip
    {
      get { return "Identifier"; }
    }

    public string ErrorStripeToolTip
    {
      get { return "Identifier"; }
    }

    public int NavigationOffsetPatch
    {
      get { return 0; }
    }

    public DocumentRange CalculateRange()
    {
      return myElement.GetNavigationRange();
    }

    public string AttributeId
    {
      get { return myAtributeId; }
    }
  }
}
