using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Completion.LookupItems;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Completion
{
  [Language(typeof(PsiLanguage))]
  public class PsiCompletionItemsProviderKeywords : ItemsProviderOfSpecificContext<PsiCodeCompletionContext>
  {
    protected override void AddItemsGroups(PsiCodeCompletionContext context, GroupedItemsCollector collector, IntellisenseManager intellisenseManager)
    {
      collector.AddRanges(EvaluateRanges(context));
      collector.AddFilter(new KeywordsBetterFilter());
    }

    protected override bool IsAvailable(PsiCodeCompletionContext context)
    {
      var type = context.BasicContext.CodeCompletionType;
      return type == CodeCompletionType.AutomaticCompletion || type == CodeCompletionType.BasicCompletion;
    }

    private static TextLookupItemBase CreateKeyworkLookupItem(string x, PsiCodeCompletionContext context)
    {
      return new PsiKeywordLookupItem(x, GetSuffix(x));
    }

    private static string GetSuffix(string keyword)
    {
      /*switch (keyword)
      {
        case "break":
        case "continue":
          return ";";
        case "this":
        case "null":
        case "true":
        case "false":
          return "";
        case "while":
          return "";
      }*/
      return " ";
    }

    protected override bool AddLookupItems(PsiCodeCompletionContext context, GroupedItemsCollector collector)
    {
      var psiFile = context.BasicContext.File as IPsiFile;
      if (psiFile == null)
        return false;
      foreach (var textLookupItem in KeywordCompletionUtil.GetAplicableKeywords(psiFile, context.BasicContext.SelectedTreeRange).Select(name => CreateKeyworkLookupItem(name, context)))
      {
        textLookupItem.InitializeRanges(context.Ranges, context.BasicContext);
        collector.AddAtDefaultPlace(textLookupItem);
      }
      return true;
    }

    protected TextLookupRanges EvaluateRanges(ISpecificCodeCompletionContext context)
    {
      IPsiFile file = context.BasicContext.File as IPsiFile;
      /*var shortNameRange =
        context.BasicContext.CodeCompletionType != CodeCompletionType.AutomaticCompletion &&
        context.BasicContext.CodeCompletionType != CodeCompletionType.ImportCompletion;
      var selectionRange = context.BasicContext.SelectedRange.TextRange;*/

      var selectionRange = context.BasicContext.SelectedRange;

      var token = file.FindNodeAt(selectionRange) as ITokenNode;

      var tokenRange = token.GetNavigationRange();

      //if (textChanged == ReferenceTextChangeType.Changed)
        //return new TextLookupRanges(selectionRange, shortNameRange, selectionRange);

      var insertRange = new TextRange(tokenRange.TextRange.StartOffset, selectionRange.TextRange.EndOffset);
      var replaceRange = new TextRange(tokenRange.TextRange.StartOffset, Math.Max(tokenRange.TextRange.EndOffset, selectionRange.TextRange.EndOffset));

      return new TextLookupRanges(insertRange, false, replaceRange);
    }
  }
}
