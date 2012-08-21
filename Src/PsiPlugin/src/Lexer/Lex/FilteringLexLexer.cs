using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Psi.Lex.Parsing;

namespace JetBrains.ReSharper.PsiPlugin.Lexer.Lex
{
  public class FilteringLexLexer : FilteringLexer
  {
    public FilteringLexLexer(ILexer lexer)
      : base(lexer)
    {
    }

    protected override bool Skip(TokenNodeType tokenType)
    {
      return ((tokenType == LexTokenType.NEW_LINE) || (tokenType == LexTokenType.WHITE_SPACE) || (tokenType == LexTokenType.END_OF_LINE_COMMENT) || (tokenType == LexTokenType.C_STYLE_COMMENT));
    }
  }
}
