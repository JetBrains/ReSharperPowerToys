using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.Jam;
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

%namespace JetBrains.ReSharper.Psi.Jam.Parsing
%class JamLexerGenerated
%public
%implements IIncrementalLexer
%function _locateToken
%virtual
%type TokenNodeType
%eofval{
  currTokenType = null; return currTokenType;
%eofval}

%include ./../../obj/Unicode.lex

NULL_CHAR=\u0000

CARRIAGE_RETURN_CHAR=\u000D
LINE_FEED_CHAR=\u000A

NEW_LINE_PAIR={CARRIAGE_RETURN_CHAR}{LINE_FEED_CHAR}
NEW_LINE_CHAR=({CARRIAGE_RETURN_CHAR}|{LINE_FEED_CHAR}|(\u0085)|(\u2028)|(\u2029))
NOT_NEW_LINE=([^\u0085\u2028\u2029\u000D\u000A])

INPUT_CHARACTER={NOT_NEW_LINE}

WHITE_SPACE_CHAR=({UNICODE_ZS}|(\u0009)|(\u000B)|(\u000C)|(\u200B)|(\uFEFF)|{NULL_CHAR})
WHITE_SPACE=({WHITE_SPACE_CHAR}+)

LETTER_CHARACTER=({UNICODE_LL}|{UNICODE_LM}|{UNICODE_LO}|{UNICODE_LT}|{UNICODE_LU}|{UNICODE_NL})
DECIMAL_DIGIT_CHARACTER={UNICODE_ND}
CONNECTING_CHARACTER={UNICODE_PC}
COMBINING_CHARACTER=({UNICODE_MC}|{UNICODE_MN})

NAME_START_CHARACTER=({LETTER_CHARACTER}|(_))
NAME_PART_CHARACTER=({LETTER_CHARACTER}|{DECIMAL_DIGIT_CHARACTER}|{CONNECTING_CHARACTER}|{COMBINING_CHARACTER}|([_]))
IDENTIFIER=({NAME_START_CHARACTER}{NAME_PART_CHARACTER}*)

DECIMAL_DIGIT=[0-9]
INTEGER_LITERAL=({DECIMAL_DIGIT}+)
REAL_LITERAL=(({DECIMAL_DIGIT})*"."({DECIMAL_DIGIT})+)

ESCAPE_SEQUENCE=(\\)
ESCAPE_STRING_LITERAL_CHARACTER_INNER=(({ESCAPE_SEQUENCE}\")|\"\")

STRING_LITERAL_CHARACTER_INNER=[^\"\\\u000D\u000A\u000C]
STRING_LITERAL_CHARACTER=({STRING_LITERAL_CHARACTER_INNER}|{ESCAPE_STRING_LITERAL_CHARACTER_INNER})
STRING_LITERAL=(\"{STRING_LITERAL_CHARACTER}*\")
STRING_LITERAL_ERROR=(\"{STRING_LITERAL_CHARACTER}*|{ESCAPE_SEQUENCE})

ASTERISKS=(\*+)
DELIMITED_COMMENT_SECTION=([^\*]|({ASTERISKS}[^\*\/]))
UNFINISHED_DELIMITED_COMMENT="/*"{DELIMITED_COMMENT_SECTION}*
DELIMITED_COMMENT={UNFINISHED_DELIMITED_COMMENT}{ASTERISKS}"/"

%%

<YYINITIAL> {NEW_LINE_PAIR} { currTokenType = makeToken (JamTokenType.NEW_LINE); yybegin(YYINITIAL); return currTokenType; }
<YYINITIAL> {NEW_LINE_CHAR} { currTokenType = makeToken (JamTokenType.NEW_LINE); yybegin(YYINITIAL); return currTokenType; }

<YYINITIAL> {WHITE_SPACE}  { currTokenType = makeToken(JamTokenType.WHITE_SPACE); return currTokenType; }

<YYINITIAL> {DELIMITED_COMMENT}  { currTokenType = makeToken(JamTokenType.COMMENT); return currTokenType; }
<YYINITIAL> {UNFINISHED_DELIMITED_COMMENT} { currTokenType = makeToken(JamTokenType.COMMENT); return currTokenType; }

<YYINITIAL> {INTEGER_LITERAL}  { currTokenType = makeToken(JamTokenType.INTEGER_LITERAL); return currTokenType; }
<YYINITIAL> {REAL_LITERAL}  { currTokenType = makeToken(JamTokenType.REAL_LITERAL); return currTokenType; }

<YYINITIAL> {STRING_LITERAL}  { currTokenType = makeToken(JamTokenType.STRING_LITERAL); return currTokenType; }
<YYINITIAL> {STRING_LITERAL_ERROR}  { currTokenType = makeToken(JamTokenType.STRING_LITERAL); return currTokenType; }

<YYINITIAL> "if"  { currTokenType = makeToken(JamTokenType.IF_KEYWORD); return currTokenType; }
<YYINITIAL> "else"  { currTokenType = makeToken(JamTokenType.ELSE_KEYWORD); return currTokenType; }
<YYINITIAL> "var"  { currTokenType = makeToken(JamTokenType.VAR_KEYWORD); return currTokenType; }
<YYINITIAL> "sub"  { currTokenType = makeToken(JamTokenType.SUB_KEYWORD); return currTokenType; }
<YYINITIAL> "return"  { currTokenType = makeToken(JamTokenType.RETURN_KEYWORD); return currTokenType; }

<YYINITIAL> "=="  { currTokenType = makeToken(JamTokenType.EQEQ); return currTokenType; }
<YYINITIAL> "!="  { currTokenType = makeToken(JamTokenType.NEQ); return currTokenType; }
<YYINITIAL> "="  { currTokenType = makeToken(JamTokenType.EQUALS); return currTokenType; }
<YYINITIAL> ";"  { currTokenType = makeToken(JamTokenType.SEMICOLON); return currTokenType; }
<YYINITIAL> ","  { currTokenType = makeToken(JamTokenType.COMMA); return currTokenType; }
<YYINITIAL> "+"  { currTokenType = makeToken(JamTokenType.PLUS); return currTokenType; }
<YYINITIAL> "-"  { currTokenType = makeToken(JamTokenType.MINUS); return currTokenType; }
<YYINITIAL> "/"  { currTokenType = makeToken(JamTokenType.DIVIDE); return currTokenType; }
<YYINITIAL> "*"  { currTokenType = makeToken(JamTokenType.MULTIPLY); return currTokenType; }
<YYINITIAL> ">="  { currTokenType = makeToken(JamTokenType.GE); return currTokenType; }
<YYINITIAL> "<="  { currTokenType = makeToken(JamTokenType.LE); return currTokenType; }
<YYINITIAL> ">"  { currTokenType = makeToken(JamTokenType.GT); return currTokenType; }
<YYINITIAL> "<"  { currTokenType = makeToken(JamTokenType.LT); return currTokenType; }
<YYINITIAL> "("  { currTokenType = makeToken(JamTokenType.LPAREN); return currTokenType; }
<YYINITIAL> ")"  { currTokenType = makeToken(JamTokenType.RPAREN); return currTokenType; }
<YYINITIAL> "{"  { currTokenType = makeToken(JamTokenType.LBRACE); return currTokenType; }
<YYINITIAL> "}"  { currTokenType = makeToken(JamTokenType.RBRACE); return currTokenType; }

<YYINITIAL> {IDENTIFIER}  { currTokenType = makeToken(JamTokenType.IDENTIFIER); return currTokenType; }

<YYINITIAL> . { currTokenType = makeToken (JamTokenType.BAD_CHARACTER); return currTokenType; }