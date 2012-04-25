using JetBrains.ReSharper.Feature.Services.TypingAssist;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.TypingAssist
{
  public class PsiBracketMatcher : BracketMatcher
  {
    public PsiBracketMatcher()
      : base(
        new[]
        {
          new Pair<TokenNodeType, TokenNodeType>(PsiTokenType.LBRACE, PsiTokenType.RBRACE),
          new Pair<TokenNodeType, TokenNodeType>(PsiTokenType.LBRACKET, PsiTokenType.RBRACKET),
          new Pair<TokenNodeType, TokenNodeType>(PsiTokenType.LPARENTH, PsiTokenType.RPARENTH),
        })
    {
    }
  }
}
