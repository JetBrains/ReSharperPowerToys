using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Impl;
using JetBrains.ReSharper.LexPlugin.Completion;
using JetBrains.ReSharper.LexPlugin.Grammar;
using JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree.Impl;
using JetBrains.ReSharper.LexPlugin.Resolve;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;

namespace JetBrains.ReSharper.LexPlugin.src.Completion
{
  [Language(typeof(LexLanguage))]
  internal class LexCodeCompletionItemsProvider : ItemsProviderWithReference<LexCodeCompletionContext, LexReferenceBase, LexFile>
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
