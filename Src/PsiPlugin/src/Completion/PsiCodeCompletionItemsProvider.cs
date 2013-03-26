using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Impl;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Completion
{
  [Language(typeof (PsiLanguage))]
  internal class PsiCodeCompletionItemsProvider : ItemsProviderWithSymbolTable<PsiCodeCompletionContext, PsiReferenceBase, PsiFile>
  {
    protected override bool IsAvailable(PsiCodeCompletionContext context)
    {
      var codeCompletionType = context.BasicContext.CodeCompletionType;
      return (codeCompletionType == CodeCompletionType.BasicCompletion) ||
             (codeCompletionType == CodeCompletionType.AutomaticCompletion);
    }

    protected override TextLookupRanges EvaluateRanges(PsiCodeCompletionContext context)
    {
      return context.Ranges;
    }

    protected override PsiReferenceBase GetReference(PsiCodeCompletionContext context)
    {
      return context.ReparsedContext.Reference as PsiReferenceBase;
    }

    protected override ISymbolTable GetCompletionSymbolTable(PsiReferenceBase reference, PsiCodeCompletionContext context)
    {
      return reference.GetCompletionSymbolTable();
    }
  }
}
