using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("Rule", null, HighlightingGroupIds.LanguageUsage, "Rule", @"
          Rule", Severity.INFO, false, Internal = false)]

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi.Highlightings
{
  [ConfigurableSeverityHighlighting("Rule", "PSI", OverlapResolve = OverlapResolveKind.NONE, ToolTipFormatString = "Rule")]
  internal class PsiRuleHighlighting : ICustomAttributeIdHighlighting
  {
    private const string Attribute = HighlightingAttributeIds.METHOD_IDENTIFIER_ATTRIBUTE;
    private readonly ITreeNode myElement;

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

    public string AttributeId
    {
      get { return Attribute; }
    }

    public DocumentRange CalculateRange()
    {
      return myElement.GetNavigationRange();
    }
  }
}
