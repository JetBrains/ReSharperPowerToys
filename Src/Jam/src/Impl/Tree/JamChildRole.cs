namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  // ReSharper disable InconsistentNaming
  public enum JamChildRole : short
  {
    // general common roles
    LAST = -1,
    NONE = 0,

    JAM_LITERAL,
    JAM_VAR,
    JAM_IDENTIFIER,
    JAM_EXPRESSION,
    JAM_SEMICOLON,
    JAM_STATEMENT,
    JAM_DECLARATION,
    JAM_LVALUE,
    JAM_OPERATOR,
    JAM_RVALUE,
    JAM_ARGUMENT,
    JAM_COMMA,
    JAM_LPAREN,
    JAM_RPAREN,
    JAM_ARGUMENT_LIST,
    JAM_PARAMETER,
    JAM_SUB,
    JAM_PARAMETER_LIST,
    JAM_LBRACE,
    JAM_RBRACE,
    JAM_RETURN
  }
}