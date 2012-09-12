using JetBrains.ReSharper.Psi.Jam.Parsing;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  public class JamKeyword : JamToken, IIdentifier
  {
    public JamKeyword(JamTokenType tokenType, string text) : base(tokenType, text) {}

    #region IIdentifier Members

    public string Name
    {
      get { return JamLexer.UnescapeName(GetText()); }
    }

    #endregion
  }
}