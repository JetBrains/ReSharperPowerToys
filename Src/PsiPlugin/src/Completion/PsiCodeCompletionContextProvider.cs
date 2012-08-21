using JetBrains.ReSharper.Feature.Services.CodeCompletion.Impl;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Completion
{
  [IntellisensePart]
  internal class PsiCodeCompletionContextProvider : CodeCompletionContextProviderBase
  {
    public override bool IsApplicable(CodeCompletionContext context)
    {
      var psiFile = context.File as PsiFile;
      return psiFile != null;
    }

    public override ISpecificCodeCompletionContext GetCompletionContext(CodeCompletionContext context)
    {
      var unterminatedContext = new PsiReparsedCompletionContext(context.File, context.SelectedTreeRange, "aa");
      unterminatedContext.Init();
      IReference referenceToComplete = unterminatedContext.Reference;
      ITreeNode elementToComplete = unterminatedContext.TreeNode;
      if (elementToComplete == null)
      {
        return null;
      }
      TreeTextRange referenceRange = referenceToComplete != null ? referenceToComplete.GetTreeTextRange() : GetElementRange(elementToComplete);
      TextRange referenceDocumentRange = unterminatedContext.ToDocumentRange(referenceRange);
      if (!referenceDocumentRange.IsValid)
      {
        return null;
      }

      if (!referenceDocumentRange.Contains(context.CaretDocumentRange.TextRange))
      {
        return null;
      }
      TextLookupRanges ranges = GetTextLookupRanges(context, referenceDocumentRange);
      return new PsiCodeCompletionContext(context, ranges, unterminatedContext);
    }

    private static TreeTextRange GetElementRange(ITreeNode element)
    {
      var tokenNode = element as ITokenNode;

      if (tokenNode != null)
      {
        if (tokenNode.GetTokenType().IsIdentifier || tokenNode.GetTokenType().IsKeyword)
        {
          return tokenNode.GetTreeTextRange();
        }
      }

      return new TreeTextRange(element.GetTreeTextRange().EndOffset);
    }
  }
}
