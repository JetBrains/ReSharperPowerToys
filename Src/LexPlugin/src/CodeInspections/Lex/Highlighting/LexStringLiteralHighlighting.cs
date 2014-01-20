using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("String", null, HighlightingGroupIds.LanguageUsage, "String", @"
          String", Severity.INFO, false, Internal = false)]
namespace JetBrains.ReSharper.LexPlugin.CodeInspections.Lex.Highlighting
{
  [ConfigurableSeverityHighlighting("String", "Lex", OverlapResolve = OverlapResolveKind.NONE, ToolTipFormatString = "String")]
  internal class LexStringLiteralHighlighting: ICustomAttributeIdHighlighting
  {
    private const string AtributeId = HighlightingAttributeIds.TYPE_INTERFACE_ATTRIBUTE;
    private readonly ITreeNode myElement;

    public LexStringLiteralHighlighting(ITreeNode element)
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
      get { return AtributeId; }
    }

    public DocumentRange CalculateRange()
    {
      return myElement.GetDocumentRange();
    }
  }
}
