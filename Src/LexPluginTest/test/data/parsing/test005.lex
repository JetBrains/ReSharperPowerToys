{caret}
%state YY_IN_BRACE_BLOCK



<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {NEW_LINE_PAIR} { currTokenType = makeToken (LexTokenType.NEW_LINE); return currTokenType; }