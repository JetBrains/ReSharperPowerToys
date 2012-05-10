using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.Text;

namespace JetBrains.ReSharper.PsiPlugin.Lexer
{
  internal class PsiLexerFactory : ILexerFactory
  {
    public static readonly PsiLexerFactory Instance = new PsiLexerFactory();

    #region ILexerFactory Members

    public ILexer CreateLexer(IBuffer buffer)
    {
      return new PsiLexer(buffer);
    }

    #endregion
  }
}
