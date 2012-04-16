using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("Rule", null, HighlightingGroupIds.LanguageUsage, "Rule", @"
          Rule", Severity.INFO, false, Internal = false)]

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Highlightings
{
  [ConfigurableSeverityHighlighting("Rule", "PSI", OverlapResolve = OverlapResolveKind.NONE, ToolTipFormatString = "Rule")]
  //[StaticSeverityHighlighting(Severity.INFO, HighlightingGroupIds.CodeInfo,
    //OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = false)]

  internal class PsiRuleHighlighting : ICustomAttributeIdHighlighting, IHighlightingWithRange
  {
    private readonly ITreeNode myElement;
    private const string Attribute = HighlightingAttributeIds.METHOD_IDENTIFIER_ATTRIBUTE;

    public PsiRuleHighlighting(ITreeNode element)
    {
      myElement = element;
    }

    public bool IsValid()
    {
      return true;
    }

    public string ToolTip
    {
      get { return null; }
    }

    public string ErrorStripeToolTip
    {
      get { return null; }
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
      get { return Attribute; }
    }
  }
}
