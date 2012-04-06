using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Psi.CodeStyle;
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
  }
}
