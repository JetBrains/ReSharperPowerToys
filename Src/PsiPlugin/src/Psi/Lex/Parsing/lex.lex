using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.PsiPlugin;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.Text;
using JetBrains.Util;


%%

%unicode

%init{
   currTokenType = null;
%init}

%{

%}

%namespace JetBrains.ReSharper.PsiPlugin.Psi.Lex.Parsing
%class LexLexerGenerated
%public
%implements IIncrementalLexer
%function _locateToken
%virtual
%type TokenNodeType
%eofval{
  currTokenType = null; return currTokenType;
%eofval}

NULL_CHAR=\u0000
CARRIAGE_RETURN_CHAR=\u000D
LINE_FEED_CHAR=\u000A
NEW_LINE_PAIR={CARRIAGE_RETURN_CHAR}{LINE_FEED_CHAR}
NEW_LINE_CHAR=({CARRIAGE_RETURN_CHAR}|{LINE_FEED_CHAR}|(\u0085)|(\u2028)|(\u2029))
NOT_NEW_LINE=([^\u0085\u2028\u2029\u000D\u000A])
NOT_NEW_LINE_NUMBER_WS=([^\#\u0085)\u2028\u2029\n\r\ \t\f\u0009\u000B\u000C])


INPUT_CHARACTER={NOT_NEW_LINE}
ASTERISKS="*"+

%include ../../../../obj/Unicode.lex

WHITE_SPACE_CHAR=({UNICODE_ZS}|(\u0009)|(\u000B)|(\u000C)|(\u200B)|(\uFEFF)|{NULL_CHAR})

BACK_SLASH = \\
BACK_SLASH_CHAR = ({BACK_SLASH}\\)

QUOTE = ({BACK_SLASH}\')
DOUBLE_QUOTE = ({BACK_SLASH}\")

DELIMITED_COMMENT_SECTION=([^\*]|({ASTERISKS}[^\*\/]))

UNFINISHED_DELIMITED_COMMENT="/*"{DELIMITED_COMMENT_SECTION}*

DELIMITED_COMMENT={UNFINISHED_DELIMITED_COMMENT}{ASTERISKS}"/"
SINGLE_LINE_COMMENT=("//"{INPUT_CHARACTER}*)


DECIMAL_DIGIT=[0-9]
HEX_DIGIT=({DECIMAL_DIGIT}|[A-Fa-f])
INTEGER_TYPE_SUFFIX=([UuLl]|UL|Ul|uL|ul|LU|lU|Lu|lu)


DECIMAL_INTEGER_LITERAL=({DECIMAL_DIGIT}+{INTEGER_TYPE_SUFFIX}?)
HEXADECIMAL_INTEGER_LITERAL=(0[Xx]({HEX_DIGIT})*{INTEGER_TYPE_SUFFIX}?)
INTEGER_LITERAL=({DECIMAL_INTEGER_LITERAL}|{HEXADECIMAL_INTEGER_LITERAL})

EXPONENT_PART=([eE](([+-])?({DECIMAL_DIGIT})*))
REAL_TYPE_SUFFIX=[FfDdMm]
REAL_LITERAL=({DECIMAL_DIGIT}*"."{DECIMAL_DIGIT}+({EXPONENT_PART})?{REAL_TYPE_SUFFIX}?)|({DECIMAL_DIGIT}+({EXPONENT_PART}|({EXPONENT_PART}?{REAL_TYPE_SUFFIX})))

SINGLE_CHARACTER=[^\'\\\u0085\u2028\u2029\u000D\u000A]
SIMPLE_ESCAPE_SEQUENCE=(\\[\'\"\\0abfnrtv])
HEXADECIMAL_ESCAPE_SEQUENCE=(\\x{HEX_DIGIT}({HEX_DIGIT}|{HEX_DIGIT}{HEX_DIGIT}|{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT})?)
UNICODE_ESCAPE_SEQUENCE=((\\u{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT})|(\\U{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}))
CHARACTER=({SINGLE_CHARACTER}|{SIMPLE_ESCAPE_SEQUENCE}|{HEXADECIMAL_ESCAPE_SEQUENCE}|{UNICODE_ESCAPE_SEQUENCE})

BAD_ESCAPE_SEQUENCE=((\\u)|(\\[^\'\"\\0abfnrtv]))
CHARACTER_LITERAL=\'({CHARACTER})\'
UNFINISHED_CHARACTER_LITERAL=\'(({CHARACTER})|({BAD_ESCAPE_SEQUENCE}(\'?))|\')
EXCEEDING_CHARACTER_LITERAL=\'{CHARACTER}({CHARACTER}|{BAD_ESCAPE_SEQUENCE})+\'

DECIMAL_DIGIT_CHARACTER={UNICODE_ND}
CONNECTING_CHARACTER={UNICODE_PC}
COMBINING_CHARACTER=({UNICODE_MC}|{UNICODE_MN})
FORMATTING_CHARACTER={UNICODE_CF}
LETTER_CHARACTER=({UNICODE_LL}|{UNICODE_LM}|{UNICODE_LO}|{UNICODE_LT}|{UNICODE_LU}|{UNICODE_NL}|{UNICODE_ESCAPE_SEQUENCE})

LOWER_CASE_LETTER_CHARACTER = ("a"|"b"|"c"|"d"|"e"|"f"|"g"|"h"|"i"|"j"|"k"|"l"|"m"|"n"|"o"|"p"|"q"|"r"|"s"|"t"|"u"|"v"|"w"|"x"|"y"|"z")
UPPER_CASE_LETTER_CHARACTER = ("A"|"B"|"C"|"D"|"E"|"F"|"G"|"H"|"I"|"J"|"K"|"L"|"M"|"N"|"O"|"P"|"Q"|"R"|"S"|"T"|"U"|"V"|"W"|"X"|"Y"|"Z")


IDENTIFIER_START_CHARACTER=({LETTER_CHARACTER}|(\u005F))
IDENTIFIER_PART_CHARACTER=({LETTER_CHARACTER}|{DECIMAL_DIGIT_CHARACTER}|{CONNECTING_CHARACTER}|{COMBINING_CHARACTER}|{FORMATTING_CHARACTER})
IDENTIFIER=("@"?{IDENTIFIER_START_CHARACTER}{IDENTIFIER_PART_CHARACTER}*)


REGULAR_STRING_LITERAL_CHARACTER=({SINGLE_REGULAR_STRING_LITERAL_CHARACTER}|{SIMPLE_ESCAPE_SEQUENCE}|{HEXADECIMAL_ESCAPE_SEQUENCE}|{UNICODE_ESCAPE_SEQUENCE})
SINGLE_REGULAR_STRING_LITERAL_CHARACTER=[^\"\\\u0085\u2028\u2029\u000D\u000A]
REGULAR_STRING_LITERAL=(\"{REGULAR_STRING_LITERAL_CHARACTER}*\")

VERBATIM_STRING_LITERAL=(\@\"{VERBATIM_STRING_LITERAL_CHARACTER}*\")
VERBATIM_STRING_LITERAL_CHARACTER=({SINGLE_VERBATIM_STRING_LITERAL_CHARACTER}|{QUOTE_ESCAPE_SEQUENCE})
SINGLE_VERBATIM_STRING_LITERAL_CHARACTER=[^\"]
QUOTE_ESCAPE_SEQUENCE=(\"\")

STRING_LITERAL=({REGULAR_STRING_LITERAL}|{VERBATIM_STRING_LITERAL})
UNFINISHED_REGULAR_STRING_LITERAL=(\"{REGULAR_STRING_LITERAL_CHARACTER}*)
UNFINISHED_VERBATIM_STRING_LITERAL=(@\"{VERBATIM_STRING_LITERAL_CHARACTER}*)
ERROR_REGULAR_STRING_LITERAL=(\"{REGULAR_STRING_LITERAL_CHARACTER}*{BAD_ESCAPE_SEQUENCE}({BAD_ESCAPE_SEQUENCE}|{REGULAR_STRING_LITERAL_CHARACTER})*\"?)
ERROR_STRING_LITERAL=({UNFINISHED_REGULAR_STRING_LITERAL}|{UNFINISHED_VERBATIM_STRING_LITERAL}|{ERROR_REGULAR_STRING_LITERAL})

NOT_NUMBER_SIGN=[^#]

WHITE_SPACE=({WHITE_SPACE_CHAR}+)
END_LINE={NOT_NEW_LINE}*(({PP_NEW_LINE_PAIR})|({PP_NEW_LINE_CHAR}))


%state YY_IN_BRACE_BLOCK
%state YY_IN_PAREN_EXPRESSION

%%

<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {NEW_LINE_PAIR} { currTokenType = makeToken (LexTokenType.NEW_LINE); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {NEW_LINE_CHAR} { currTokenType = makeToken (LexTokenType.NEW_LINE); return currTokenType; }

<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {DELIMITED_COMMENT}  { currTokenType = makeToken (LexTokenType.C_STYLE_COMMENT); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {SINGLE_LINE_COMMENT}  { currTokenType = makeToken (LexTokenType.END_OF_LINE_COMMENT); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {UNFINISHED_DELIMITED_COMMENT} { currTokenType = makeToken (LexTokenType.C_STYLE_COMMENT); return currTokenType; }

<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {WHITE_SPACE}  { currTokenType = makeToken(LexTokenType.WHITE_SPACE); return currTokenType; }

<YYINITIAL,YY_IN_BRACE_BLOCK> "using" { return currTokenType = makeToken(LexTokenType.USING_KEYWORD); }
<YYINITIAL,YY_IN_BRACE_BLOCK> "init" { return currTokenType = makeToken(LexTokenType.INIT_KEYWORD); }
<YYINITIAL,YY_IN_BRACE_BLOCK> "eofval" { return currTokenType = makeToken(LexTokenType.EOFVAL_KEYWORD); }

<YYINITIAL> "type" { return currTokenType = makeToken(LexTokenType.TYPE_KEYWORD); }
<YYINITIAL> "state" { return currTokenType = makeToken(LexTokenType.STATE_KEYWORD); }
<YY_IN_BRACE_BLOCK> "type" {   currTokenType = makeToken (LexTokenType.IDENTIFIER); return currTokenType; }
<YY_IN_BRACE_BLOCK> "state" {   currTokenType = makeToken (LexTokenType.IDENTIFIER); return currTokenType; }
<YYINITIAL,YY_IN_PAREN_EXPRESSION> "{" { yybegin(YY_IN_BRACE_BLOCK);  currTokenType = makeToken (LexTokenType.LBRACE); return currTokenType; }
<YY_IN_BRACE_BLOCK> "{" { myBraces++; currTokenType = makeToken (LexTokenType.LBRACE); return currTokenType; }
<YYINITIAL,YY_IN_PAREN_EXPRESSION> "}" { currTokenType = makeToken (LexTokenType.RBRACE); return currTokenType; }
<YY_IN_BRACE_BLOCK> "}" { myBraces--; if(myBraces < 0){yybegin(YYINITIAL); myBraces = 0;} currTokenType = makeToken (LexTokenType.RBRACE); return currTokenType; }


<YYINITIAL,YY_IN_BRACE_BLOCK> "virtual" { return currTokenType = makeToken(LexTokenType.VIRTUAL_KEYWORD); }
<YYINITIAL,YY_IN_BRACE_BLOCK> "function" { return currTokenType = makeToken(LexTokenType.FUNCTION_KEYWORD); }
<YYINITIAL,YY_IN_BRACE_BLOCK> "implements" { return currTokenType = makeToken(LexTokenType.IMPLEMENTS_KEYWORD); }
<YYINITIAL,YY_IN_BRACE_BLOCK> "public" { return currTokenType = makeToken(LexTokenType.PUBLIC_KEYWORD); }
<YYINITIAL,YY_IN_BRACE_BLOCK> "class" { return currTokenType = makeToken(LexTokenType.CLASS_KEYWORD); }
<YYINITIAL,YY_IN_BRACE_BLOCK> "namespace" { return currTokenType = makeToken(LexTokenType.NAMESPACE_KEYWORD); }
<YYINITIAL,YY_IN_BRACE_BLOCK> "include" { return currTokenType = makeToken(LexTokenType.INCLUDE_KEYWORD); }
<YYINITIAL,YY_IN_BRACE_BLOCK> "return" { return currTokenType = makeToken(LexTokenType.RETURN_KEYWORD); }
<YYINITIAL,YY_IN_BRACE_BLOCK> "null" { return currTokenType = makeToken(LexTokenType.NULL_KEYWORD); }


<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "[" { currTokenType = makeToken (LexTokenType.LBRACKET); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "]" { currTokenType = makeToken (LexTokenType.RBRACKET); return currTokenType; }
<YYINITIAL> "(" { yybegin(YY_IN_PAREN_EXPRESSION); currTokenType = makeToken (LexTokenType.LPARENTH); return currTokenType; }
<YY_IN_BRACE_BLOCK> "(" { currTokenType = makeToken (LexTokenType.LPARENTH); return currTokenType; }
<YY_IN_PAREN_EXPRESSION> "(" { myParen++; currTokenType = makeToken (LexTokenType.LPARENTH); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK> ")" { currTokenType = makeToken (LexTokenType.RPARENTH); return currTokenType; }
<YY_IN_PAREN_EXPRESSION> ")" { myParen--; if(myParen < 0){yybegin(YYINITIAL); myParen = 0;} currTokenType = makeToken (LexTokenType.RPARENTH); return currTokenType; }

<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "." { currTokenType = makeToken (LexTokenType.DOT); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "," { currTokenType = makeToken (LexTokenType.COMMA); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> ":" { currTokenType = makeToken (LexTokenType.COLON); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> ";" { currTokenType = makeToken (LexTokenType.SEMICOLON); return currTokenType; }

<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "+" { currTokenType = makeToken (LexTokenType.PLUS); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "-" { currTokenType = makeToken (LexTokenType.MINUS); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "*" { currTokenType = makeToken (LexTokenType.ASTERISK); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "/" { currTokenType = makeToken (LexTokenType.DIV); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "%%" { currTokenType = makeToken (LexTokenType.PERCPERC); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "%" { currTokenType = makeToken (LexTokenType.PERC); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "&" { currTokenType = makeToken (LexTokenType.AND); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "|" { currTokenType = makeToken (LexTokenType.OR); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "^" { currTokenType = makeToken (LexTokenType.XOR); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "!" { currTokenType = makeToken (LexTokenType.EXCL); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "~" { currTokenType = makeToken (LexTokenType.TILDE); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "@" { currTokenType = makeToken (LexTokenType.AT); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "#" { currTokenType = makeToken (LexTokenType.SHARP); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "`" { currTokenType = makeToken (LexTokenType.BACK_QUOTE); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "$" { currTokenType = makeToken (LexTokenType.DOLLAR); return currTokenType; }

<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "=" { currTokenType = makeToken (LexTokenType.EQ); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> ">" { currTokenType = makeToken (LexTokenType.GT); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "<" { currTokenType = makeToken (LexTokenType.LT); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "?" { currTokenType = makeToken (LexTokenType.QUEST); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "++" { currTokenType = makeToken (LexTokenType.PLUSPLUS); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "--" { currTokenType = makeToken (LexTokenType.MINUSMINUS); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "&&" { currTokenType = makeToken (LexTokenType.ANDAND); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "||" { currTokenType = makeToken (LexTokenType.OROR); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "<<" { currTokenType = makeToken (LexTokenType.LTLT); return currTokenType; }

<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "??" { currTokenType = makeToken(LexTokenType.DOUBLE_QUEST); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "::" { currTokenType = makeToken(LexTokenType.DOUBLE_COLON); return currTokenType; }

<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "==" { currTokenType = makeToken (LexTokenType.EQEQ); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "!=" { currTokenType = makeToken (LexTokenType.NE); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "<=" { currTokenType = makeToken (LexTokenType.LE); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> ">=" { currTokenType = makeToken (LexTokenType.GE); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "+=" { currTokenType = makeToken (LexTokenType.PLUSEQ); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "-=" { currTokenType = makeToken (LexTokenType.MINUSEQ); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "*=" { currTokenType = makeToken (LexTokenType.ASTERISKEQ); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "/=" { currTokenType = makeToken (LexTokenType.DIVEQ); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "%=" { currTokenType = makeToken (LexTokenType.PERCEQ); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "&=" { currTokenType = makeToken (LexTokenType.ANDEQ); return currTokenType; }

<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "|=" { currTokenType = makeToken (LexTokenType.OREQ); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "^=" { currTokenType = makeToken (LexTokenType.XOREQ); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "<<=" { currTokenType = makeToken (LexTokenType.LTLTEQ); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "->" { currTokenType = makeToken (LexTokenType.ARROW); return currTokenType; }

<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> "=>" { currTokenType = makeToken (LexTokenType.LAMBDA_ARROW); return currTokenType; }


<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {INTEGER_LITERAL}  { currTokenType = makeToken (LexTokenType.INTEGER_LITERAL); return currTokenType; }

<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {CHARACTER_LITERAL}  { currTokenType = makeToken (LexTokenType.CHARACTER_LITERAL); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {UNFINISHED_CHARACTER_LITERAL} { currTokenType = makeToken (LexTokenType.CHARACTER_LITERAL); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {EXCEEDING_CHARACTER_LITERAL} { currTokenType = makeToken (LexTokenType.CHARACTER_LITERAL); return currTokenType; }

<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {STRING_LITERAL}  { currTokenType = makeToken (LexTokenType.STRING_LITERAL); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {DOUBLE_QUOTE} { currTokenType = makeToken (LexTokenType.DOUBLE_QUOTE); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {ERROR_STRING_LITERAL}  { currTokenType = makeToken (LexTokenType.STRING_LITERAL); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {BACK_SLASH}  { currTokenType = makeToken (LexTokenType.SIMPLE_BACK_SLASH); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {BACK_SLASH_CHAR} { currTokenType = makeToken (LexTokenType.BACK_SLASH); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {QUOTE} { currTokenType = makeToken (LexTokenType.QUOTE); return currTokenType; }
<YYINITIAL,YY_IN_BRACE_BLOCK,YY_IN_PAREN_EXPRESSION> {IDENTIFIER}  { currTokenType = makeToken (((TokenNodeType) keywords[yytext()]) ?? LexTokenType.IDENTIFIER); return currTokenType; }
