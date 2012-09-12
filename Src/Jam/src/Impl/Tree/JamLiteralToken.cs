using JetBrains.ReSharper.Psi.Jam.Parsing;

namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  public class JamLiteralToken : JamToken
  {
    public JamLiteralToken(JamTokenType tokenType, string text) : base(tokenType, text) {}
  }
}