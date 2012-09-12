using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;

namespace JetBrains.ReSharper.Psi.Jam.Parsing
{
  public class JamLexerFactory : ILexerFactory
  {
    public static readonly ILexerFactory Instance = new JamLexerFactory();

    #region ILexerFactory Members

    public ILexer CreateLexer(IBuffer buffer)
    {
      return new JamLexer(buffer);
    }

    #endregion
  }
}