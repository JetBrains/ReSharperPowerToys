using System.Text;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiIndentCache : CustomizableIndentCacheBase
  {
    public PsiIndentCache()
      : base(null)
    {
    }

    protected override string CalcLineIndent(ITreeNode node)
    {
      string customIndent = GetCustomIndent(node, CustomIndentType.RelativeLineCalculation);
      if (customIndent != null)
      {
        return customIndent;
      }

      var indent = new StringBuilder();
      foreach (ITokenNode token in node.GetFirstTokenIn().PrevTokens())
      {
        TokenNodeType tokenType = token.GetTokenType();
        if (tokenType == PsiTokenType.WHITE_SPACE)
        {
          indent.Prepend(token.GetText());
        }
        else if (tokenType == PsiTokenType.NEW_LINE)
        {
          break;
        }
      }

      return indent.ToString();
    }

    protected override string CalcNodeIndent(ITreeNode node)
    {
      var indent = new StringBuilder();
      foreach (ITokenNode token in node.GetFirstTokenIn().PrevTokens())
      {
        TokenNodeType tokenType = token.GetTokenType();
        if (tokenType == PsiTokenType.WHITE_SPACE)
        {
          indent.Prepend(token.GetText());
        }
        else if (tokenType != PsiTokenType.NEW_LINE)
        {
          indent.Prepend(new string(' ', token.GetTextLength()));
        }
        else
        {
          break;
        }
      }

      return indent.ToString();
    }
  }
}
