using JetBrains.ReSharper.Feature.Services.SelectEmbracingConstruct;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.SelectEmbracingConstruct
{
  internal class PsiSelection : SelectedRangeBase<PsiFile>
  {
    private TreeOffset myStartOffset;
    public PsiSelection(PsiFile fileNode, TreeOffset startOffset)
      : base(fileNode, fileNode.GetDocumentRange(new TreeTextRange(startOffset)))
    {
      myStartOffset = startOffset;
    }

    public PsiSelection(PsiFile fileNode, TreeOffset startStartOffset, TreeOffset endOffset)
      : base(fileNode, fileNode.GetDocumentRange(new TreeTextRange(startStartOffset,endOffset)))
    {
      myStartOffset = startStartOffset;
    }

    #region Overrides of SelectedRangeBase<PsiFile>

    public override ISelectedRange Parent
    {
      get { 
        var token = FileNode.FindTokenAt(myStartOffset);
        ITreeNode prevToken = token.GetPreviousToken();
        if(prevToken == null)
        {
          return null;
        }
        while(prevToken.PrevSibling != null)
        {
          prevToken = prevToken.PrevSibling;
        }
        var parent = token.FindCommonParent(prevToken);
        return new PsiSelection(FileNode, parent.GetTreeStartOffset(), parent.GetTreeEndOffset());
      }
    }

    #endregion
  }
}