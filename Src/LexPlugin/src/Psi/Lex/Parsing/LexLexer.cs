using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.LexPlugin.Psi.Lex.Parsing
{
  public class LexLexer : LexLexerGenerated
  {
    private static readonly TokenNodeType[] OurHash = new TokenNodeType[65536];

    private static readonly Dictionary<TokenNodeType, string> OurTokenTextMap = new Dictionary<TokenNodeType, string>();
    private static readonly Dictionary<TokenNodeType, string> OurKeywordTextMap = new Dictionary<TokenNodeType, string>();

    static LexLexer()
    {
      OurKeywordTextMap[LexTokenType.USING_KEYWORD] = "using";
      OurKeywordTextMap[LexTokenType.INIT_KEYWORD] = "init";
      OurKeywordTextMap[LexTokenType.INCLUDE_KEYWORD] = "include";
      OurKeywordTextMap[LexTokenType.EOFVAL_KEYWORD] = "eofval";
      OurKeywordTextMap[LexTokenType.TYPE_KEYWORD] = "type";
      OurKeywordTextMap[LexTokenType.VIRTUAL_KEYWORD] = "virtual";
      OurKeywordTextMap[LexTokenType.FUNCTION_KEYWORD] = "function";
      OurKeywordTextMap[LexTokenType.IMPLEMENTS_KEYWORD] = "implements";
      OurKeywordTextMap[LexTokenType.PUBLIC_KEYWORD] = "public";
      OurKeywordTextMap[LexTokenType.CLASS_KEYWORD] = "class";
      OurKeywordTextMap[LexTokenType.NAMESPACE_KEYWORD] = "namespace";
      OurKeywordTextMap[LexTokenType.RETURN_KEYWORD] = "return";
      OurKeywordTextMap[LexTokenType.NULL_KEYWORD] = "null";
      OurKeywordTextMap[LexTokenType.STATE_KEYWORD] = "state";

      OurTokenTextMap[LexTokenType.LPARENTH] = "(";
      OurTokenTextMap[LexTokenType.RPARENTH] = ")";
      OurTokenTextMap[LexTokenType.LBRACE] = "{";
      OurTokenTextMap[LexTokenType.RBRACE] = "}";
      OurTokenTextMap[LexTokenType.LBRACKET] = "[";
      OurTokenTextMap[LexTokenType.RBRACKET] = "]";
      OurTokenTextMap[LexTokenType.SEMICOLON] = ";";
      OurTokenTextMap[LexTokenType.COMMA] = ",";
      OurTokenTextMap[LexTokenType.DOT] = ".";
      OurTokenTextMap[LexTokenType.EQ] = "=";
      OurTokenTextMap[LexTokenType.GT] = ">";
      OurTokenTextMap[LexTokenType.LT] = "<";
      OurTokenTextMap[LexTokenType.EXCL] = "!";
      OurTokenTextMap[LexTokenType.TILDE] = "~";
      OurTokenTextMap[LexTokenType.AT] = "@";
      OurTokenTextMap[LexTokenType.SHARP] = "#";
      OurTokenTextMap[LexTokenType.BACK_QUOTE] = "`";
      OurTokenTextMap[LexTokenType.QUEST] = "?";
      OurTokenTextMap[LexTokenType.COLON] = ":";
      OurTokenTextMap[LexTokenType.PLUS] = "+";
      OurTokenTextMap[LexTokenType.MINUS] = "-";
      OurTokenTextMap[LexTokenType.ASTERISK] = "*";
      OurTokenTextMap[LexTokenType.DIV] = "/";
      OurTokenTextMap[LexTokenType.AND] = "&";
      OurTokenTextMap[LexTokenType.OR] = "|";
      OurTokenTextMap[LexTokenType.XOR] = "^";
      OurTokenTextMap[LexTokenType.PERC] = "%";
      OurTokenTextMap[LexTokenType.EQEQ] = "==";
      OurTokenTextMap[LexTokenType.LE] = "<=";
      OurTokenTextMap[LexTokenType.GE] = ">=";
      OurTokenTextMap[LexTokenType.NE] = "!=";
      OurTokenTextMap[LexTokenType.ANDAND] = "&&";
      OurTokenTextMap[LexTokenType.OROR] = "||";
      OurTokenTextMap[LexTokenType.PLUSPLUS] = "++";
      OurTokenTextMap[LexTokenType.MINUSMINUS] = "--";
      OurTokenTextMap[LexTokenType.LTLT] = "<<";
      OurTokenTextMap[LexTokenType.GTGT] = ">>";
      OurTokenTextMap[LexTokenType.PLUSEQ] = "+=";
      OurTokenTextMap[LexTokenType.MINUSEQ] = "-=";
      OurTokenTextMap[LexTokenType.ASTERISKEQ] = "*=";
      OurTokenTextMap[LexTokenType.DIVEQ] = "/=";
      OurTokenTextMap[LexTokenType.ANDEQ] = "&=";
      OurTokenTextMap[LexTokenType.OREQ] = "|=";
      OurTokenTextMap[LexTokenType.XOREQ] = "^=";
      OurTokenTextMap[LexTokenType.PERCEQ] = "%=";
      OurTokenTextMap[LexTokenType.LTLTEQ] = "<<=";
      OurTokenTextMap[LexTokenType.GTGTEQ] = ">>=";
      OurTokenTextMap[LexTokenType.DOUBLE_COLON] = "::";
      OurTokenTextMap[LexTokenType.DOUBLE_QUEST] = "??";
      OurTokenTextMap[LexTokenType.ARROW] = "->";
      OurTokenTextMap[LexTokenType.LAMBDA_ARROW] = "=>";

      for (int i = 0; i < OurHash.Length; i++)
      {
        OurHash[i] = LexTokenType.IDENTIFIER;
      }
      for (IDictionaryEnumerator ide = keywords.GetEnumerator(); ide.MoveNext();)
      {
        ushort hash = CalcHash((string) ide.Entry.Key);
        Assertion.Assert(OurHash[hash] == LexTokenType.IDENTIFIER,
          "The condition (ourHash[hash] == PsiTokenType.IDENTIFIER) is false.");
        OurHash[hash] = (TokenNodeType) ide.Entry.Value;
      }
    }

    public LexLexer(IBuffer buffer)
      : base(buffer)
    {
    }

    private static ushort CalcHash(string str)
    {
      return str.Aggregate<char, ushort>(0, IncHash);
    }

    private static ushort IncHash(ushort hash, char c)
    {
      return (ushort)(((hash << 1)) + Convert.ToUInt16(c));
    }

    public static string GetTokenText(TokenNodeType token)
    {
      if (OurTokenTextMap.ContainsKey(token))
      {
        return OurTokenTextMap.GetValue(token);
      }
      if (OurKeywordTextMap.ContainsKey(token))
      {
        return OurKeywordTextMap.GetValue(token);
      }
      return token.ToString();
    }

    public static bool IsKeyword(String s)
    {
      return OurKeywordTextMap.ContainsValue(s);
    }

    public static bool IsWhitespace(string s)
    {
      var lexer = new LexLexer(new StringBuffer(s));
      lexer.Start();
      return lexer.TokenType != null && lexer.TokenType.IsWhitespace && lexer.TokenEnd == s.Length;
    }
  }
}
