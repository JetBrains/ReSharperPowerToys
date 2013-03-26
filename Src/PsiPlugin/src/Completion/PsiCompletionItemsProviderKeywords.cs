using System;
using System.Linq;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Completion.LookupItems;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Completion
{
  [Language(typeof (PsiLanguage))]
  public class PsiCompletionItemsProviderKeywords : ItemsProviderOfSpecificContext<PsiCodeCompletionContext>
  {
    protected override void AddItemsGroups(PsiCodeCompletionContext context, GroupedItemsCollector collector, IntellisenseManager intellisenseManager)
    {
      collector.AddRanges(EvaluateRanges(context));
      collector.AddFilter(new KeywordsBetterFilter());
    }

    protected override bool IsAvailable(PsiCodeCompletionContext context)
    {
      CodeCompletionType type = context.BasicContext.CodeCompletionType;
      return type == CodeCompletionType.AutomaticCompletion || type == CodeCompletionType.BasicCompletion;
    }

    private static TextLookupItemBase CreateKeyworkLookupItem(string x)
    {
      return new PsiKeywordLookupItem(x, GetSuffix());
    }

    private static string GetSuffix()
    {
      return " ";
    }

    protected override bool AddLookupItems(PsiCodeCompletionContext context, GroupedItemsCollector collector)
    {
      var psiFile = context.BasicContext.File as IPsiFile;
      if (psiFile == null)
      {
        return false;
      }
      foreach (TextLookupItemBase textLookupItem in KeywordCompletionUtil.GetAplicableKeywords(psiFile, context.BasicContext.SelectedTreeRange).Select(CreateKeyworkLookupItem))
      {
        textLookupItem.InitializeRanges(context.Ranges, context.BasicContext);
        collector.AddAtDefaultPlace(textLookupItem);
      }
      return true;
    }

    private TextLookupRanges EvaluateRanges(ISpecificCodeCompletionContext context)
    {
      var file = context.BasicContext.File as IPsiFile;

      DocumentRange selectionRange = context.BasicContext.SelectedRange;

      if (file != null)
      {
        var token = file.FindNodeAt(selectionRange) as ITokenNode;

        if (token != null)
        {
          DocumentRange tokenRange = token.GetNavigationRange();

          var insertRange = new TextRange(tokenRange.TextRange.StartOffset, selectionRange.TextRange.EndOffset);
          var replaceRange = new TextRange(tokenRange.TextRange.StartOffset, Math.Max(tokenRange.TextRange.EndOffset, selectionRange.TextRange.EndOffset));

          return new TextLookupRanges(insertRange, false, replaceRange);
        }
      }
      return new TextLookupRanges(TextRange.InvalidRange, false, TextRange.InvalidRange);
    }
  }
}
