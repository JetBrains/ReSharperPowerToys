using JetBrains.ReSharper.Psi.Parsing;

namespace JetBrains.ReSharper.Psi.Jam.Parsing
{
  public class FilteringJamLexer : FilteringLexer
  {
    public FilteringJamLexer(ILexer lexer) : base(lexer) {}

    protected override bool Skip(TokenNodeType tokenType)
    {
      return tokenType.IsWhitespace || tokenType.IsComment;
    }
  }
}