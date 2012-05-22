using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public static class PsiFormatterHelper
  {
    public static IWhitespaceNode CreateSpace(string spaceText)
    {
      return new Whitespace(spaceText);
    }

    public static void ReplaceSpaces(ITreeNode leftNode, ITreeNode rightNode, IEnumerable<string> wsTexts)
    {
      if (wsTexts == null)
      {
        return;
      }
      FormatterImplHelper.ReplaceSpaces(leftNode, rightNode, wsTexts.CreateWhitespaces());
    }

    [NotNull]
    private static IWhitespaceNode[] CreateWhitespaces([NotNull] this IEnumerable<string> wsTexts)
    {
      if (wsTexts == null)
      {
        throw new ArgumentNullException("wsTexts");
      }

      return wsTexts.Where(text => !text.IsEmpty()).Select(text =>
      {
        if (text.IsNewLine())
        {
          return CreateNewLine("\r\n");
        }
        // consistency check (remove in release?)
        if (!PsiLexer.IsWhitespace(text))
        {
          throw new ApplicationException("Inconsistent space structure");
        }
        return CreateSpace(text);
      }).ToArray();
    }

    public static IWhitespaceNode CreateNewLine(string text)
    {
      return new NewLine(text);
    }

    private static IEnumerable<IWhitespaceNode> GetLineFeedsTo(this ITreeNode fromNode, ITreeNode toNode)
    {
      return fromNode.GetWhitespacesTo(toNode).Where(wsNode => (wsNode.GetTokenType() == PsiTokenType.NEW_LINE) && (wsNode is IWhitespaceNode)).Cast<IWhitespaceNode>();
    }

    public static void MakeIndent(this ITreeNode indentNode, string indent)
    {
      var lastSpace = indentNode.PrevSibling as IWhitespaceNode;
      if (lastSpace != null && lastSpace.GetTokenType() != PsiTokenType.NEW_LINE)
      {
        ITreeNode firstSpace = lastSpace.LeftWhitespaces().TakeWhile(ws => ws != PsiTokenType.NEW_LINE).LastOrDefault() ?? lastSpace;
        while (firstSpace.GetTokenType() != PsiTokenType.NEW_LINE)
        {
          firstSpace = firstSpace.GetNextToken();
        }
        firstSpace = firstSpace.GetNextToken();
        if (firstSpace != lastSpace || lastSpace.GetText() != indent)
        {
          if (indent.IsEmpty())
          {
            LowLevelModificationUtil.DeleteChildRange(firstSpace, lastSpace);
          }
          else
          {
            LowLevelModificationUtil.ReplaceChildRange(firstSpace, lastSpace, CreateSpace(indent));
          }
        }
      }
      else if (!indent.IsEmpty())
      {
        LowLevelModificationUtil.AddChildBefore(indentNode, CreateSpace(indent));
      }
    }

    public static bool HasLineFeedsTo(this ITreeNode fromNode, ITreeNode toNode)
    {
      return fromNode.GetLineFeedsTo(toNode).Any();
    }

    public static string GetSampleText(this TokenNodeType type)
    {
      string text = type.TokenRepresentation;
      Assertion.Assert(text != null, "No sample for token of type " + type);
      return text;
    }
  }
}
