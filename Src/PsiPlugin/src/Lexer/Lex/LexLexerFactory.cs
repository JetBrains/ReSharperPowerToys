using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Psi.Lex.Parsing;
using JetBrains.Text;

namespace JetBrains.ReSharper.PsiPlugin.Lexer.Lex
{
  public class LexLexerFactory : ILexerFactory
  {
    public static readonly LexLexerFactory Instance = new LexLexerFactory();

    #region ILexerFactory Members

    public ILexer CreateLexer(IBuffer buffer)
    {
      return new LexLexer(buffer);
    }

    #endregion
  }
}
