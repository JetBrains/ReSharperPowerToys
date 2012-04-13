using System;
using System.Text;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiIndentCache : CustomizableIndentCacheBase
  {
    public PsiIndentCache() : base(null)
    {}

    protected override string CalcLineIndent(ITreeNode node)
    {
      var customIndent = GetCustomIndent(node, CustomIndentType.RelativeLineCalculation);
      if (customIndent != null)
        return customIndent;

      var indent = new StringBuilder();
      foreach (var token in node.GetFirstTokenIn().PrevTokens())
      {
        var tokenType = token.GetTokenType();
        if (tokenType == PsiTokenType.WHITE_SPACE)
          indent.Prepend(token.GetText());
        else if (tokenType == PsiTokenType.NEW_LINE)
          break;
        //else
          //indent.Remove(0, indent.Length);
      }

      return indent.ToString();
    }

    protected override string CalcNodeIndent(ITreeNode node)
    {
      var indent = new StringBuilder();
      foreach (var token in node.GetFirstTokenIn().PrevTokens())
      {
        var tokenType = token.GetTokenType();
        if (tokenType == PsiTokenType.WHITE_SPACE)
          indent.Prepend(token.GetText());
        else if (tokenType != PsiTokenType.NEW_LINE)
          indent.Prepend(new string(' ', token.GetTextLength()));
        else
          break;
      }

      return indent.ToString();
    }
  }
}