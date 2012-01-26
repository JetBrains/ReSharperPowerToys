using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Impl;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.Completion
{
  [Language(typeof(PsiLanguage))]
  internal class PsiCodeCompletionItemsProvider : ItemsProviderWithReference<PsiCodeCompletionContext,PsiReferenceBase,PsiFile>
  {
    protected override bool IsAvailable(PsiCodeCompletionContext context)
    {
      if (context.BasicContext.CodeCompletionType != CodeCompletionType.BasicCompletion)
        return false;

      var reference = context.ReparsedContext.Reference;
      if (reference == null)
        return false;

      return true;
    }

    private ISymbolTable GetSymbolTableOfReference(IReference reference)
    {
      return reference.GetReferenceSymbolTable(false);
    }
  }
}
