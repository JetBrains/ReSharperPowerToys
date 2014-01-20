using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("State", null, HighlightingGroupIds.LanguageUsage, "State", @"
          State", Severity.INFO, false, Internal = false)]

namespace JetBrains.ReSharper.LexPlugin.CodeInspections.Lex.Highlighting
{
  [ConfigurableSeverityHighlighting("State", "Lex", OverlapResolve = OverlapResolveKind.NONE, ToolTipFormatString = "State")]
  public class LexStateHighlighting : ICustomAttributeIdHighlighting
  {
    private const string Attribute = HighlightingAttributeIds.FIELD_IDENTIFIER_ATTRIBUTE;
    private readonly ITreeNode myElement;

    public LexStateHighlighting(ITreeNode element)
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
