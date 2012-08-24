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

%%

<YYINITIAL> {NEW_LINE_PAIR} { currTokenType = makeToken (LexTokenType.NEW_LINE); yybegin(YYINITIAL); return currTokenType; }
<YYINITIAL> {NEW_LINE_CHAR} { currTokenType = makeToken (LexTokenType.NEW_LINE); yybegin(YYINITIAL); return currTokenType; }

<YYINITIAL> {DELIMITED_COMMENT}  { currTokenType = makeToken (LexTokenType.C_STYLE_COMMENT); return currTokenType; }
<YYINITIAL> {SINGLE_LINE_COMMENT}  { currTokenType = makeToken (LexTokenType.END_OF_LINE_COMMENT); yybegin(YYINITIAL); return currTokenType; }
<YYINITIAL> {UNFINISHED_DELIMITED_COMMENT} { currTokenType = makeToken (LexTokenType.C_STYLE_COMMENT); return currTokenType; }

<YYINITIAL> {WHITE_SPACE}  { currTokenType = makeToken(LexTokenType.WHITE_SPACE); return currTokenType; }

<YYINITIAL> "using" { return currTokenType = makeToken(LexTokenType.USING_KEYWORD); }
<YYINITIAL> "init" { return currTokenType = makeToken(LexTokenType.INIT_KEYWORD); }
<YYINITIAL> "eofval" { return currTokenType = makeToken(LexTokenType.EOFVAL_KEYWORD); }
<YYINITIAL> "type" { return currTokenType = makeToken(LexTokenType.TYPE_KEYWORD); }
<YYINITIAL> "virtual" { return currTokenType = makeToken(LexTokenType.VIRTUAL_KEYWORD); }
<YYINITIAL> "function" { return currTokenType = makeToken(LexTokenType.FUNCTION_KEYWORD); }
<YYINITIAL> "implements" { return currTokenType = makeToken(LexTokenType.IMPLEMENTS_KEYWORD); }
<YYINITIAL> "public" { return currTokenType = makeToken(LexTokenType.PUBLIC_KEYWORD); }
<YYINITIAL> "class" { return currTokenType = makeToken(LexTokenType.CLASS_KEYWORD); }
<YYINITIAL> "namespace" { return currTokenType = makeToken(LexTokenType.NAMESPACE_KEYWORD); }
<YYINITIAL> "include" { return currTokenType = makeToken(LexTokenType.INCLUDE_KEYWORD); }

<YYINITIAL> "{" { currTokenType = makeToken (LexTokenType.LBRACE); return currTokenType; }
<YYINITIAL> "}" { currTokenType = makeToken (LexTokenType.RBRACE); return currTokenType; }
<YYINITIAL> "[" { currTokenType = makeToken (LexTokenType.LBRACKET); return currTokenType; }
<YYINITIAL> "]" { currTokenType = makeToken (LexTokenType.RBRACKET); return currTokenType; }
<YYINITIAL> "(" { currTokenType = makeToken (LexTokenType.LPARENTH); return currTokenType; }
<YYINITIAL> ")" { currTokenType = makeToken (LexTokenType.RPARENTH); return currTokenType; }
<YYINITIAL> "." { currTokenType = makeToken (LexTokenType.DOT); return currTokenType; }
<YYINITIAL> "," { currTokenType = makeToken (LexTokenType.COMMA); return currTokenType; }
<YYINITIAL> ":" { currTokenType = makeToken (LexTokenType.COLON); return currTokenType; }
<YYINITIAL> ";" { currTokenType = makeToken (LexTokenType.SEMICOLON); return currTokenType; }

<YYINITIAL> "+" { currTokenType = makeToken (LexTokenType.PLUS); return currTokenType; }
<YYINITIAL> "-" { currTokenType = makeToken (LexTokenType.MINUS); return currTokenType; }
<YYINITIAL> "*" { currTokenType = makeToken (LexTokenType.ASTERISK); return currTokenType; }
<YYINITIAL> "/" { currTokenType = makeToken (LexTokenType.DIV); return currTokenType; }
<YYINITIAL> "%" { currTokenType = makeToken (LexTokenType.PERC); return currTokenType; }
<YYINITIAL> "&" { currTokenType = makeToken (LexTokenType.AND); return currTokenType; }
<YYINITIAL> "|" { currTokenType = makeToken (LexTokenType.OR); return currTokenType; }
<YYINITIAL> "^" { currTokenType = makeToken (LexTokenType.XOR); return currTokenType; }
<YYINITIAL> "!" { currTokenType = makeToken (LexTokenType.EXCL); return currTokenType; }
<YYINITIAL> "~" { currTokenType = makeToken (LexTokenType.TILDE); return currTokenType; }
<YYINITIAL> "@" { currTokenType = makeToken (LexTokenType.AT); return currTokenType; }
<YYINITIAL> "#" { currTokenType = makeToken (LexTokenType.SHARP); return currTokenType; }
<YYINITIAL> "`" { currTokenType = makeToken (LexTokenType.BACK_QUOTE); return currTokenType; }
<YYINITIAL> "$" { currTokenType = makeToken (LexTokenType.DOLLAR); return currTokenType; }

<YYINITIAL> "=" { currTokenType = makeToken (LexTokenType.EQ); return currTokenType; }
<YYINITIAL> ">" { currTokenType = makeToken (LexTokenType.GT); return currTokenType; }
<YYINITIAL> "<" { currTokenType = makeToken (LexTokenType.LT); return currTokenType; }
<YYINITIAL> "?" { currTokenType = makeToken (LexTokenType.QUEST); return currTokenType; }
<YYINITIAL> "++" { currTokenType = makeToken (LexTokenType.PLUSPLUS); return currTokenType; }
<YYINITIAL> "--" { currTokenType = makeToken (LexTokenType.MINUSMINUS); return currTokenType; }
<YYINITIAL> "&&" { currTokenType = makeToken (LexTokenType.ANDAND); return currTokenType; }
<YYINITIAL> "||" { currTokenType = makeToken (LexTokenType.OROR); return currTokenType; }
<YYINITIAL> "<<" { currTokenType = makeToken (LexTokenType.LTLT); return currTokenType; }

<YYINITIAL> "??" { currTokenType = makeToken(LexTokenType.DOUBLE_QUEST); return currTokenType; }
<YYINITIAL> "::" { currTokenType = makeToken(LexTokenType.DOUBLE_COLON); return currTokenType; }

<YYINITIAL> "==" { currTokenType = makeToken (LexTokenType.EQEQ); return currTokenType; }
<YYINITIAL> "!=" { currTokenType = makeToken (LexTokenType.NE); return currTokenType; }
<YYINITIAL> "<=" { currTokenType = makeToken (LexTokenType.LE); return currTokenType; }
<YYINITIAL> ">=" { currTokenType = makeToken (LexTokenType.GE); return currTokenType; }
<YYINITIAL> "+=" { currTokenType = makeToken (LexTokenType.PLUSEQ); return currTokenType; }
<YYINITIAL> "-=" { currTokenType = makeToken (LexTokenType.MINUSEQ); return currTokenType; }
<YYINITIAL> "*=" { currTokenType = makeToken (LexTokenType.ASTERISKEQ); return currTokenType; }
<YYINITIAL> "/=" { currTokenType = makeToken (LexTokenType.DIVEQ); return currTokenType; }
<YYINITIAL> "%=" { currTokenType = makeToken (LexTokenType.PERCEQ); return currTokenType; }
<YYINITIAL> "&=" { currTokenType = makeToken (LexTokenType.ANDEQ); return currTokenType; }

<YYINITIAL> "|=" { currTokenType = makeToken (LexTokenType.OREQ); return currTokenType; }
<YYINITIAL> "^=" { currTokenType = makeToken (LexTokenType.XOREQ); return currTokenType; }
<YYINITIAL> "<<=" { currTokenType = makeToken (LexTokenType.LTLTEQ); return currTokenType; }
<YYINITIAL> "->" { currTokenType = makeToken (LexTokenType.ARROW); return currTokenType; }

<YYINITIAL> "=>" { currTokenType = makeToken (LexTokenType.LAMBDA_ARROW); return currTokenType; }


<YYINITIAL> {INTEGER_LITERAL}  { currTokenType = makeToken (LexTokenType.INTEGER_LITERAL); return currTokenType; }

<YYINITIAL> {CHARACTER_LITERAL}  { currTokenType = makeToken (LexTokenType.CHARACTER_LITERAL); return currTokenType; }
<YYINITIAL> {UNFINISHED_CHARACTER_LITERAL} { currTokenType = makeToken (LexTokenType.CHARACTER_LITERAL); return currTokenType; }
<YYINITIAL> {EXCEEDING_CHARACTER_LITERAL} { currTokenType = makeToken (LexTokenType.CHARACTER_LITERAL); return currTokenType; }

<YYINITIAL> {STRING_LITERAL}  { currTokenType = makeToken (LexTokenType.STRING_LITERAL); return currTokenType; }
<YYINITIAL> {DOUBLE_QUOTE} { currTokenType = makeToken (LexTokenType.DOUBLE_QUOTE); return currTokenType; }
<YYINITIAL> {ERROR_STRING_LITERAL}  { currTokenType = makeToken (LexTokenType.STRING_LITERAL); return currTokenType; }
<YYINITIAL> {BACK_SLASH}  { currTokenType = makeToken (LexTokenType.SIMPLE_BACK_SLASH); return currTokenType; }
<YYINITIAL> {BACK_SLASH_CHAR} { currTokenType = makeToken (LexTokenType.BACK_SLASH); return currTokenType; }
<YYINITIAL> {QUOTE} { currTokenType = makeToken (LexTokenType.QUOTE); return currTokenType; }
<YYINITIAL> {IDENTIFIER}  { currTokenType = makeToken (((TokenNodeType) keywords[yytext()]) ?? LexTokenType.IDENTIFIER); return currTokenType; }
