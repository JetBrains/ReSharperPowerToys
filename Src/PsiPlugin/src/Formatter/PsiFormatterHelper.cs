using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public static class PsiFormatterHelper
  {
    public static IWhitespaceNode CreateSpace(string spaceText)
    {
      return (IWhitespaceNode)TreeElementFactory.CreateLeafElement(PsiTokenType.WHITE_SPACE, FormatterImplHelper.GetPooledWhitespace(spaceText), 0, 1);
    }

    public static void ReplaceSpaces(ITreeNode leftNode, ITreeNode rightNode, IEnumerable<string> wsTexts)
    {
      if (wsTexts == null)
        return;
      FormatterImplHelper.ReplaceSpaces(leftNode, rightNode, wsTexts.CreateWhitespaces());
    }

    [NotNull]
    private static IWhitespaceNode[] CreateWhitespaces([NotNull] this IEnumerable<string> wsTexts)
    {
      if (wsTexts == null)
        throw new ArgumentNullException("wsTexts");

      return wsTexts.Where(text => !text.IsEmpty()).Select(text =>
      {
        if (text.IsNewLine())
          return CreateNewLine();
        // consistency check (remove in release?)
        if (!PsiLexer.IsWhitespace(text))
          throw new ApplicationException("Inconsistent space structure");
        return CreateSpace(text);
      }).ToArray();
    }

    public static void Format([NotNull] this ICodeFormatter formatter, ITreeNode root, IContextBoundSettingsStore overrideSettingsStore = null)
    {
      formatter.Format(root, default(IProgressIndicator), overrideSettingsStore);
    }

    public static IWhitespaceNode CreateNewLine()
    {
      var buf = FormatterImplHelper.NewLineBuffer;
      return (IWhitespaceNode)TreeElementFactory.CreateLeafElement(PsiTokenType.NEW_LINE, buf, 0, buf.Length);
    }

    public static int GetLineFeedsCount(this FormattingStageContext context)
    {
      return context.LeftChild.GetLineFeedsCountTo(context.RightChild);
    }

    private static IEnumerable<IWhitespaceNode> GetLineFeedsTo(this ITreeNode fromNode, ITreeNode toNode)
    {
      return  (IEnumerable<IWhitespaceNode>) fromNode.GetWhitespacesTo(toNode).Where(wsNode => (wsNode == PsiTokenType.NEW_LINE) && (wsNode is IWhitespaceNode));
    }

    private static int GetLineFeedsCountTo(this ITreeNode fromNode, ITreeNode toNode)
    {
      return fromNode.GetLineFeedsTo(toNode).Count();
    }

    public static void MakeIndent(this ITreeNode indentNode, string indent)
    {
      var lastSpace = indentNode.PrevSibling as IWhitespaceNode;
      if (lastSpace != null && !(lastSpace == PsiTokenType.NEW_LINE))
      {
        var firstSpace = lastSpace.LeftWhitespaces().TakeWhile(ws => !(ws == PsiTokenType.NEW_LINE)).LastOrDefault() ?? lastSpace;

        if (firstSpace != lastSpace || lastSpace.GetText() != indent)
          if (indent.IsEmpty())
            LowLevelModificationUtil.DeleteChildRange(firstSpace, lastSpace);
          else
            LowLevelModificationUtil.ReplaceChildRange(firstSpace, lastSpace, CreateSpace(indent));
      }
      else if (!indent.IsEmpty())
        LowLevelModificationUtil.AddChildBefore(indentNode, CreateSpace(indent));
    }

    public static bool HasLineFeedsTo(this ITreeNode fromNode, ITreeNode toNode)
    {
      return fromNode.GetLineFeedsTo(toNode).Any();
    }
  }
}
