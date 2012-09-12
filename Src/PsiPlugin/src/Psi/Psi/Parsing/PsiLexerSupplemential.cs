using System.Collections;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing
{
  public partial class PsiLexerGenerated 
  {
    protected static readonly Hashtable keywords = new Hashtable();
    protected static readonly Hashtable ourTokenTypeToText = new Hashtable();

    private static readonly NodeTypeSet MAKE_DIVIDE = new NodeTypeSet
      (
      PsiTokenType.IDENTIFIER,
      PsiTokenType.RBRACKET,
      PsiTokenType.RPARENTH,
      PsiTokenType.RBRACE,
      PsiTokenType.MINUS,
      PsiTokenType.STRING_LITERAL
      );

    private TokenNodeType currTokenType;

    protected bool myMakeDivide;

    #region IIncrementalLexer Members

    public int LexemIndent
    {
      get { return 7; }
    }

    public uint LexerStateEx
    {
      get { return (uint)yy_lexical_state; }
    }

    public void Start(int startOffset, int endOffset, uint state)
    {
      yy_buffer_index = startOffset;
      yy_buffer_start = startOffset;
      yy_buffer_end = startOffset;
      yy_eof_pos = endOffset;
      yy_lexical_state = (int)state;
      currTokenType = null;
    }

    public int EOFPos
    {
      get { return yy_eof_pos; }
    }

    #endregion

    #region ILexer Members

    public void Start()
    {
      Start(0, yy_buffer.Length, YYINITIAL);
    }

    public void Advance()
    {
      locateToken();
      currTokenType = null;
    }

    public object CurrentPosition
    {
      get
      {
        TokenPosition tokenPosition;
        tokenPosition.CurrTokenType = currTokenType;
        tokenPosition.YyBufferIndex = yy_buffer_index;
        tokenPosition.YyBufferStart = yy_buffer_start;
        tokenPosition.YyBufferEnd = yy_buffer_end;
        tokenPosition.YyLexicalState = yy_lexical_state;
        tokenPosition.MakeDivide = myMakeDivide;
        return tokenPosition;
      }
      set
      {
        var tokenPosition = (TokenPosition)value;
        currTokenType = tokenPosition.CurrTokenType;
        yy_buffer_index = tokenPosition.YyBufferIndex;
        yy_buffer_start = tokenPosition.YyBufferStart;
        yy_buffer_end = tokenPosition.YyBufferEnd;
        yy_lexical_state = tokenPosition.YyLexicalState;
        myMakeDivide = tokenPosition.MakeDivide;
      }
    }

    public TokenNodeType TokenType
    {
      get
      {
        locateToken();
        return currTokenType;
      }
    }

    public int TokenStart
    {
      get
      {
        locateToken();
        return yy_buffer_start;
      }
    }

    public int TokenEnd
    {
      get
      {
        locateToken();
        return yy_buffer_end;
      }
    }

    public IBuffer Buffer
    {
      get { return yy_buffer; }
    }

    #endregion

    private TokenNodeType makeToken(TokenNodeType type)
    {
      return currTokenType = type;
    }

    protected static string GetKeywordTextByTokenType(NodeType tokenType)
    {
      return (string)ourTokenTypeToText[tokenType];
    }

    private void locateToken()
    {
      if (currTokenType == null)
      {
        currTokenType = _locateToken();
      }
      if (currTokenType != null && !currTokenType.IsWhitespace)
      {
        myMakeDivide = MAKE_DIVIDE[currTokenType];
      }
    }

    #region Nested type: TokenPosition

    private struct TokenPosition
    {
      public TokenNodeType CurrTokenType;
      public bool MakeDivide;
      public int YyBufferEnd;
      public int YyBufferIndex;
      public int YyBufferStart;
      public int YyLexicalState;
    }

    #endregion
  }
}
