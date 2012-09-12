%%
NEW_{caret}LINE_PAIR={CARRIAGE_RETURN_CHAR}{LINE_FEED_CHAR}
%%

<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {NEW_LINE_PAIR} { currTokenType = makeToken (LexTokenType.NEW_LINE); return currTokenType; }
