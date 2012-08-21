using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
using JetBrains.ReSharper.PsiPlugin.PsiGrammar;
using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Completion
{
  [Language(typeof (PsiLanguage))]
  internal class PsiCodeCompletionItemsProvider : ItemsProviderWithReference<PsiCodeCompletionContext, PsiReferenceBase, PsiFile>
  {
    protected override bool IsAvailable(PsiCodeCompletionContext context)
    {
      if (!((context.BasicContext.CodeCompletionType == CodeCompletionType.BasicCompletion) || (context.BasicContext.CodeCompletionType == CodeCompletionType.AutomaticCompletion)))
      {
        return false;
      }

      IReference reference = context.ReparsedContext.Reference;
      if (reference == null)
      {
        return false;
      }

      return true;
    }
  }
}
