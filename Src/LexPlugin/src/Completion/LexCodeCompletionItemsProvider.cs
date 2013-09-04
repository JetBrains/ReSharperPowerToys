using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.LexPlugin.Grammar;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;

namespace JetBrains.ReSharper.LexPlugin.Completion
{
  [Language(typeof(LexLanguage))]
  internal class LexCodeCompletionItemsProvider : ItemsProviderOfSpecificContext<LexCodeCompletionContext>
  {
    protected override bool IsAvailable(LexCodeCompletionContext context)
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
