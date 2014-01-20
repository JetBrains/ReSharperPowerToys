using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("Comment", null, HighlightingGroupIds.LanguageUsage, "Comment", @"
          Comment", Severity.INFO, false, Internal = false)]
namespace JetBrains.ReSharper.LexPlugin.CodeInspections.Lex.Highlighting
{
  [ConfigurableSeverityHighlighting("Comment", "Lex", OverlapResolve = OverlapResolveKind.NONE, ToolTipFormatString = "Comment")]
  internal class LexCommentHighlighting : ICustomAttributeIdHighlighting
  {
    private const string OurAttributeId = HighlightingAttributeIds.JAVA_SCRIPT_XML_DOC_TAG;
    private readonly ITreeNode myNode;

    public LexCommentHighlighting(ITreeNode node)
    {
      myNode = node;
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
      get { return OurAttributeId; }
    }

    public DocumentRange CalculateRange()
    {
      return myNode.GetDocumentRange();
    }
  }
}
