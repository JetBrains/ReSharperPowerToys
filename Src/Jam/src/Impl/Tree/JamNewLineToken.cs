using JetBrains.ReSharper.Psi.Jam.Parsing;

namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  internal class JamNewLineToken : JamWhitespaceTokenBase
  {
    public JamNewLineToken(string text) : base(text, JamTokenType.NEW_LINE) {}

    public override bool IsNewLine
    {
      get { return true; }
    }
  }
}