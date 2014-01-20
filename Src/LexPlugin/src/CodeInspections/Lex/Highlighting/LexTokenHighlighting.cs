using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("Token", null, HighlightingGroupIds.LanguageUsage, "Token", @"
          Token", Severity.INFO, false, Internal = false)]

namespace JetBrains.ReSharper.LexPlugin.CodeInspections.Lex.Highlighting
{
  [ConfigurableSeverityHighlighting("Token", "Lex", OverlapResolve = OverlapResolveKind.NONE, ToolTipFormatString = "Token")]
  internal class LexTokenHighlighting : ICustomAttributeIdHighlighting
  {
    private const string Attribute = HighlightingAttributeIds.METHOD_IDENTIFIER_ATTRIBUTE;
    private readonly ITreeNode myElement;

    public LexTokenHighlighting(ITreeNode element)
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
