using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Highlightings
{

  [StaticSeverityHighlighting(Severity.INFO, HighlightingGroupIds.CodeInfo, 
    OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = false)]
  internal class PsiCommentHighlighting : ICustomAttributeIdHighlighting, IHighlightingWithRange
  {
    private readonly ITreeNode myNode;

    public PsiCommentHighlighting(ITreeNode node)
    {
      myNode = node;
    }

    private const string AtributeId = HighlightingAttributeIds.JAVA_SCRIPT_XML_DOC_TAG;

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
      return myNode.GetDocumentRange();
    }

    public string AttributeId
    {
      get { return AtributeId; }
    }
  }
}
