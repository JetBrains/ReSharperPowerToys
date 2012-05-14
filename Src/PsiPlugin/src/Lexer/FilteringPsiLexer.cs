using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Parsing;

namespace JetBrains.ReSharper.PsiPlugin.Lexer
{
  public class FilteringPsiLexer : FilteringLexer
  {
    public FilteringPsiLexer(ILexer lexer)
      : base(lexer)
    {
    }

    protected override bool Skip(TokenNodeType tokenType)
    {
      return ((tokenType == PsiTokenType.NEW_LINE) || (tokenType == PsiTokenType.WHITE_SPACE) || (tokenType == PsiTokenType.END_OF_LINE_COMMENT) || (tokenType == PsiTokenType.C_STYLE_COMMENT));
    }
  }
}
