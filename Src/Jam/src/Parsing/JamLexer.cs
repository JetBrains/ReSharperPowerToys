using JetBrains.Text;

namespace JetBrains.ReSharper.Psi.Jam.Parsing
{
  public class JamLexer : JamLexerGenerated
  {
    public JamLexer(IBuffer buffer) : base(buffer) {}
    public JamLexer(IBuffer buffer, int startOffset, int endOffset) : base(buffer, startOffset, endOffset) {}

    public static string UnescapeName(string text)
    {
      // TODO : impl
      return text;
    }
  }
}