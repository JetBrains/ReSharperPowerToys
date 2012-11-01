using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing;

namespace JetBrains.ReSharper.PsiPlugin.Lexer.Psi
{
  public class FilteringPsiLexer : FilteringLexer, ILexer<int>
  {
    public FilteringPsiLexer(CachingLexer lexer)
      : base(lexer)
    {
    }

    public CachingLexer Lexer
    {
      get { return myLexer as CachingLexer; }
    }

    protected override bool Skip(TokenNodeType tokenType)
    {
      return ((tokenType == PsiTokenType.NEW_LINE) || (tokenType == PsiTokenType.WHITE_SPACE) || (tokenType == PsiTokenType.END_OF_LINE_COMMENT) || (tokenType == PsiTokenType.C_STYLE_COMMENT));
    }

    public void Advance(int deltha)
    {
      (myLexer as CachingLexer).Advance(deltha);
    }

    #region Implementation of ILexer<int>

    public int CurrentPosition
    {
      get { return (myLexer as CachingLexer).CurrentPosition; }
      set { (myLexer as CachingLexer).CurrentPosition = value; }
    }

    #endregion
  }
}
