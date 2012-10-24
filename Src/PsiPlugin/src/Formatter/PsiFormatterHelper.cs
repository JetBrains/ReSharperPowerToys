using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
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
        ITreeNode firstSpace = lastSpace.LeftWhitespaces().TakeWhile(ws => ws.GetTokenType() != PsiTokenType.NEW_LINE).LastOrDefault() ?? lastSpace;
        Debug.Assert(firstSpace != null, "firstSpace != null");
        if (firstSpace != lastSpace)
        {
          while ((firstSpace != null) && (firstSpace.GetTokenType() != PsiTokenType.NEW_LINE) && (firstSpace.GetNextToken() != lastSpace))
          {
            firstSpace = firstSpace.GetNextToken();
          }
          firstSpace = firstSpace.GetNextToken();
        }
        if (firstSpace != null)
        {
          /*if ((firstSpace != lastSpace || lastSpace.GetText() != indent) && (firstSpace.Parent == lastSpace.Parent))
          {*/
            if (indent.IsEmpty())
            {
              LowLevelModificationUtil.DeleteChildRange(firstSpace, lastSpace);
            }
            else
            {
              LowLevelModificationUtil.ReplaceChildRange(firstSpace, lastSpace, CreateSpace(indent));
            }
          //}
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

    public static IEnumerable<FormattingRange> GetNodePairs(this CodeFormattingContext context)
    {
      ITreeNode firstNode = context.FirstNode;
      ITreeNode lastNode = context.LastNode;
      IList<FormattingRange> list = new List<FormattingRange>();
      GetNodePairs(firstNode, lastNode, list);
      return list;
    }

    private static void GetNodePairs(ITreeNode firstNode, ITreeNode lastNode, IList<FormattingRange> list)
    {
      var firstChild = firstNode;
      var lastChild = lastNode;
      var commonParent = firstNode.FindCommonParent(lastNode);
      while (firstChild != null && firstChild.Parent != commonParent)
      {
        firstChild = firstChild.Parent;
      }
      while (lastChild != null && lastChild.Parent != commonParent)
      {
        lastChild = lastChild.Parent;
      }
      Assertion.Assert(firstChild != null, "firstChild != null");
      Assertion.Assert(lastChild != null, "lastChild != null");
      var node = firstChild;
      while (node != null && node != lastChild.NextSibling)
      {
        if (!node.IsWhitespaceToken())
        {
          GetNodePairs(node, list, commonParent);
        }
        node = node.NextSibling;
      }
    }

    private static void GetNodePairs(ITreeNode treeNode, IList<FormattingRange> list, ITreeNode parent)
    {
      ITreeNode node = treeNode;
      if (node.FirstChild == null)
      {
        if (!node.IsWhitespaceToken())
        {
          ITreeNode nextNode = null;
          while ((node != null) && (nextNode == null))
          {
            var sibling = node.NextSibling;
            while (sibling != null && sibling.IsWhitespaceToken())
            {
              sibling = sibling.NextSibling;
            }
            if (sibling == null)
            {
              node = node.Parent;
              if (node == parent)
              {
                break;
              }
            }
            else
            {
              nextNode = sibling;
            }
          }
          if (nextNode != null)
          {
            list.Add(new FormattingRange(node, nextNode));
          }
        }
      }
      else
      {
        ITreeNode child = node.FirstChild;
        {
          while (child != null)
          {
            if (!child.IsWhitespaceToken())
            {
              GetNodePairs(child, list, parent);
            }
            child = child.NextSibling;
          }
        }
      }
    }
  }
}
