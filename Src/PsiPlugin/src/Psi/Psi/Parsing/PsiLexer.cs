using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing
{
  public class PsiLexer : PsiLexerGenerated
  {
    private static readonly TokenNodeType[] OurHash = new TokenNodeType[65536];

    private static readonly Dictionary<TokenNodeType, string> OurTokenTextMap = new Dictionary<TokenNodeType, string>();
    private static readonly Dictionary<TokenNodeType, string> OurKeywordTextMap = new Dictionary<TokenNodeType, string>();


    static PsiLexer()
    {
      OurKeywordTextMap[PsiTokenType.ABSTRACT] = "abstract";
      OurKeywordTextMap[PsiTokenType.ERRORHANDLING] = "errorhandling";
      OurKeywordTextMap[PsiTokenType.EXTRAS] = "extras";
      OurKeywordTextMap[PsiTokenType.GET] = "get";
      OurKeywordTextMap[PsiTokenType.GETTER] = "getter";
      OurKeywordTextMap[PsiTokenType.OPTIONS] = "options";
      OurKeywordTextMap[PsiTokenType.INTERFACE] = "interface";
      OurKeywordTextMap[PsiTokenType.INTERFACES] = "interfaces";
      OurKeywordTextMap[PsiTokenType.PRIVATE] = "private";
      OurKeywordTextMap[PsiTokenType.PATHS] = "paths";
      OurKeywordTextMap[PsiTokenType.RETURN_TYPE] = "returnType";
      OurKeywordTextMap[PsiTokenType.ROLE_KEYWORD] = "ROLE";
      OurKeywordTextMap[PsiTokenType.NULL_KEYWORD] = "null";
      OurKeywordTextMap[PsiTokenType.LIST_KEYWORD] = "LIST";
      OurKeywordTextMap[PsiTokenType.SEP_KEYWORD] = "SEP";

      OurTokenTextMap[PsiTokenType.LPARENTH] = "(";
      OurTokenTextMap[PsiTokenType.RPARENTH] = ")";
      OurTokenTextMap[PsiTokenType.LBRACE] = "{";
      OurTokenTextMap[PsiTokenType.RBRACE] = "}";
      OurTokenTextMap[PsiTokenType.LBRACKET] = "[";
      OurTokenTextMap[PsiTokenType.RBRACKET] = "]";
      OurTokenTextMap[PsiTokenType.SEMICOLON] = ";";
      OurTokenTextMap[PsiTokenType.COMMA] = ",";
      OurTokenTextMap[PsiTokenType.DOT] = ".";
      OurTokenTextMap[PsiTokenType.EQ] = "=";
      OurTokenTextMap[PsiTokenType.GT] = ">";
      OurTokenTextMap[PsiTokenType.LT] = "<";
      OurTokenTextMap[PsiTokenType.EXCL] = "!";
      OurTokenTextMap[PsiTokenType.TILDE] = "~";
      OurTokenTextMap[PsiTokenType.AT] = "@";
      OurTokenTextMap[PsiTokenType.SHARP] = "#";
      OurTokenTextMap[PsiTokenType.BACK_QUOTE] = "`";
      OurTokenTextMap[PsiTokenType.QUEST] = "?";
      OurTokenTextMap[PsiTokenType.COLON] = ":";
      OurTokenTextMap[PsiTokenType.PLUS] = "+";
      OurTokenTextMap[PsiTokenType.MINUS] = "-";
      OurTokenTextMap[PsiTokenType.ASTERISK] = "*";
      OurTokenTextMap[PsiTokenType.DIV] = "/";
      OurTokenTextMap[PsiTokenType.AND] = "&";
      OurTokenTextMap[PsiTokenType.OR] = "|";
      OurTokenTextMap[PsiTokenType.XOR] = "^";
      OurTokenTextMap[PsiTokenType.PERC] = "%";
      OurTokenTextMap[PsiTokenType.EQEQ] = "==";
      OurTokenTextMap[PsiTokenType.LE] = "<=";
      OurTokenTextMap[PsiTokenType.GE] = ">=";
      OurTokenTextMap[PsiTokenType.NE] = "!=";
      OurTokenTextMap[PsiTokenType.ANDAND] = "&&";
      OurTokenTextMap[PsiTokenType.OROR] = "||";
      OurTokenTextMap[PsiTokenType.PLUSPLUS] = "++";
      OurTokenTextMap[PsiTokenType.MINUSMINUS] = "--";
      OurTokenTextMap[PsiTokenType.LTLT] = "<<";
      OurTokenTextMap[PsiTokenType.GTGT] = ">>";
      OurTokenTextMap[PsiTokenType.PLUSEQ] = "+=";
      OurTokenTextMap[PsiTokenType.MINUSEQ] = "-=";
      OurTokenTextMap[PsiTokenType.ASTERISKEQ] = "*=";
      OurTokenTextMap[PsiTokenType.DIVEQ] = "/=";
      OurTokenTextMap[PsiTokenType.ANDEQ] = "&=";
      OurTokenTextMap[PsiTokenType.OREQ] = "|=";
      OurTokenTextMap[PsiTokenType.XOREQ] = "^=";
      OurTokenTextMap[PsiTokenType.PERCEQ] = "%=";
      OurTokenTextMap[PsiTokenType.LTLTEQ] = "<<=";
      OurTokenTextMap[PsiTokenType.GTGTEQ] = ">>=";
      OurTokenTextMap[PsiTokenType.DOUBLE_COLON] = "::";
      OurTokenTextMap[PsiTokenType.DOUBLE_QUEST] = "??";
      OurTokenTextMap[PsiTokenType.ARROW] = "->";
      OurTokenTextMap[PsiTokenType.LAMBDA_ARROW] = "=>";

      for (int i = 0 ; i < OurHash.Length ; i++)
      {
        OurHash[i] = PsiTokenType.IDENTIFIER;
      }
      for (IDictionaryEnumerator ide = keywords.GetEnumerator() ; ide.MoveNext() ;)
      {
        ushort hash = CalcHash((string)ide.Entry.Key);
        Assertion.Assert(OurHash[hash] == PsiTokenType.IDENTIFIER,
          "The condition (ourHash[hash] == PsiTokenType.IDENTIFIER) is false.");
        OurHash[hash] = (TokenNodeType)ide.Entry.Value;
      }
    }

    public PsiLexer(IBuffer buffer)
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
      var lexer = new PsiLexer(new StringBuffer(s));
      lexer.Start();
      return lexer.TokenType != null && lexer.TokenType.IsWhitespace && lexer.TokenEnd == s.Length;
    }
  }
}
