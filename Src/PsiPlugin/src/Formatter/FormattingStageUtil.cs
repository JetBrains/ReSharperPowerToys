using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public static class FormattingStageUtil
  {
    private static readonly string[] ourNoSpace = EmptyArray<string>.Instance;
    private static readonly string[] ourSingleSpace = new[] { " " }; 

    [Pure]
    public static IEnumerable<string> GetNodesSpace(
      int minLineFeeds,
      int maxLineFeeds,
      int preferedLineFeeds,
      bool makeSpace,
      PsiFmtStageContext context,
      FormattingStageData data)
    {
      // calculates number of line feeds
      var currentLineFeeds = context.GetLineFeedsCount();

      if (maxLineFeeds == 0)
        return makeSpace ? ourSingleSpace : ourNoSpace;

      // check if we can use spaces)
      if (currentLineFeeds == 0 && minLineFeeds == 0 && data.Profile.Profile != CodeFormatProfile.GENERATOR)
        return makeSpace ? ourSingleSpace : ourNoSpace;

      if (data.Profile.Profile == CodeFormatProfile.GENERATOR)
      {
        if (preferedLineFeeds == 0)
          return makeSpace ? ourSingleSpace : ourNoSpace;
        return Enumerable.Range(0, preferedLineFeeds).Select(i => Environment.NewLine);
      }

      // feet )
      if (currentLineFeeds >= minLineFeeds && currentLineFeeds <= maxLineFeeds || data.Profile.Profile == CodeFormatProfile.INDENT)
        return GetCurrentWhitespaces(context);

      if (currentLineFeeds < minLineFeeds)
      {
        // we've decided to add some new feeds. May be bad idea for html...
        if (IsSingleLine(context, data))
          return GetCurrentWhitespaces(context);
        return Enumerable.Range(0, minLineFeeds).Select(i => Environment.NewLine);
      }

      var toRemove = currentLineFeeds - maxLineFeeds;

      return context.LeftChild.GetWhitespacesTo(context.RightChild).SkipWhile(
        node =>
          {
            if (toRemove == 0)
              return false;
            if (node.GetTokenType() == PsiTokenType.NEW_LINE)
              toRemove--;
            return true;
          }).
        Select(ws => ws.GetText());
    }

    private static IEnumerable<string> GetCurrentWhitespaces(PsiFmtStageContext context)
    {
      return context.LeftChild.
        GetWhitespacesTo(context.RightChild).
        Where(ws => (ws == PsiTokenType.NEW_LINE) || ws.NextSibling == context.RightChild).
        Select(ws => ws.GetText());
    }

    private static bool IsSingleLine(PsiFmtStageContext context, FormattingStageData data)
    {
      var leftChild = context.LeftChild;
      var rightChild = context.RightChild;
      if(leftChild is IModifier)
      {
        return true;
      }
      if (leftChild.GetTokenType() == PsiTokenType.ABSTRACT || leftChild.GetTokenType() == PsiTokenType.ERRORHANDLING || leftChild.GetTokenType() == PsiTokenType.PRIVATE)
      {
        return true;
      }
      if(leftChild.GetTokenType() == PsiTokenType.EXTRAS || leftChild.GetTokenType() == PsiTokenType.OPTIONS)
      {
        return true;
      }
      /*foreach (var extension in data.Extensions)
      {
        var formatSingleLine = extension.FormatSingleLine(context.LeftChild);
        if (formatSingleLine.HasValue)
          return formatSingleLine.Value;
      }*/
      return false;
    }
  }
}