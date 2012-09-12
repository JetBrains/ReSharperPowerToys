using JetBrains.ReSharper.Psi.Jam.Parsing;

namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  internal class JamWhitespaceToken : JamWhitespaceTokenBase
  {
    public JamWhitespaceToken(string text) : base(text, JamTokenType.WHITE_SPACE) {}

    public override bool IsNewLine
    {
      get { return false; }
    }
  }
}