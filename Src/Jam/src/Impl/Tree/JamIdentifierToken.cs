using JetBrains.ReSharper.Psi.Jam.Parsing;
using JetBrains.ReSharper.Psi.Jam.Tree;

namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  public class JamIdentifierToken : JamToken, IJamIdentifier
  {
    public JamIdentifierToken(string text) : base(JamTokenType.IDENTIFIER, text) {}

    #region IJamIdentifier Members

    public string Name
    {
      get { return JamLexer.UnescapeName(GetText()); }
    }

    #endregion
  }
}