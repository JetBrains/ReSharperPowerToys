using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Parsing
{
  public class PsiLexer : PsiLexerGenerated
  {
    private static readonly TokenNodeType[] ourHash = new TokenNodeType[65536];

    private static readonly Dictionary<TokenNodeType, string> ourTokenTextMap = new Dictionary<TokenNodeType, string>();
    private static readonly Dictionary<TokenNodeType, string> ourKeywordTextMap = new Dictionary<TokenNodeType, string>();



    static PsiLexer()
    {

      ourKeywordTextMap[PsiTokenType.ABSTRACT] = "abstract";
      ourKeywordTextMap[PsiTokenType.ERRORHANDLING] = "errorhandling";
      ourKeywordTextMap[PsiTokenType.EXTRAS] = "extras";
      ourKeywordTextMap[PsiTokenType.GET] = "get";
      ourKeywordTextMap[PsiTokenType.GETTER] = "getter";
      ourKeywordTextMap[PsiTokenType.OPTIONS] = "options";
      ourKeywordTextMap[PsiTokenType.INTERFACE] = "interface";
      ourKeywordTextMap[PsiTokenType.INTERFACES] = "interfaces";
      ourKeywordTextMap[PsiTokenType.PRIVATE] = "private";
      ourKeywordTextMap[PsiTokenType.PATHS] = "paths";
      ourKeywordTextMap[PsiTokenType.RETURN_TYPE] = "returnType";
      ourKeywordTextMap[PsiTokenType.ROLE_KEYWORD] = "ROLE";
      ourKeywordTextMap[PsiTokenType.NULL_KEYWORD] = "null";
      ourKeywordTextMap[PsiTokenType.LIST_KEYWORD] = "LIST";
      ourKeywordTextMap[PsiTokenType.SEP_KEYWORD] = "SEP";

      ourTokenTextMap[PsiTokenType.LPARENTH] = "(";
      ourTokenTextMap[PsiTokenType.RPARENTH] = ")";
      ourTokenTextMap[PsiTokenType.LBRACE] = "{";
      ourTokenTextMap[PsiTokenType.RBRACE] = "}";
      ourTokenTextMap[PsiTokenType.LBRACKET] = "[";
      ourTokenTextMap[PsiTokenType.RBRACKET] = "]";
      ourTokenTextMap[PsiTokenType.SEMICOLON] = ";";
      ourTokenTextMap[PsiTokenType.COMMA] = ",";
      ourTokenTextMap[PsiTokenType.DOT] = ".";
      ourTokenTextMap[PsiTokenType.EQ] = "=";
      ourTokenTextMap[PsiTokenType.GT] = ">";
      ourTokenTextMap[PsiTokenType.LT] = "<";
      ourTokenTextMap[PsiTokenType.EXCL] = "!";
      ourTokenTextMap[PsiTokenType.TILDE] = "~";
      ourTokenTextMap[PsiTokenType.AT] = "@";
      ourTokenTextMap[PsiTokenType.SHARP] = "#";
      ourTokenTextMap[PsiTokenType.BACK_QUOTE] = "`";
      ourTokenTextMap[PsiTokenType.QUEST] = "?";
      ourTokenTextMap[PsiTokenType.COLON] = ":";
      ourTokenTextMap[PsiTokenType.PLUS] = "+";
      ourTokenTextMap[PsiTokenType.MINUS] = "-";
      ourTokenTextMap[PsiTokenType.ASTERISK] = "*";
      ourTokenTextMap[PsiTokenType.DIV] = "/";
      ourTokenTextMap[PsiTokenType.AND] = "&";
      ourTokenTextMap[PsiTokenType.OR] = "|";
      ourTokenTextMap[PsiTokenType.XOR] = "^";
      ourTokenTextMap[PsiTokenType.PERC] = "%";
      ourTokenTextMap[PsiTokenType.EQEQ] = "==";
      ourTokenTextMap[PsiTokenType.LE] = "<=";
      ourTokenTextMap[PsiTokenType.GE] = ">=";
      ourTokenTextMap[PsiTokenType.NE] = "!=";
      ourTokenTextMap[PsiTokenType.ANDAND] = "&&";
      ourTokenTextMap[PsiTokenType.OROR] = "||";
      ourTokenTextMap[PsiTokenType.PLUSPLUS] = "++";
      ourTokenTextMap[PsiTokenType.MINUSMINUS] = "--";
      ourTokenTextMap[PsiTokenType.LTLT] = "<<";
      ourTokenTextMap[PsiTokenType.GTGT] = ">>";
      ourTokenTextMap[PsiTokenType.PLUSEQ] = "+=";
      ourTokenTextMap[PsiTokenType.MINUSEQ] = "-=";
      ourTokenTextMap[PsiTokenType.ASTERISKEQ] = "*=";
      ourTokenTextMap[PsiTokenType.DIVEQ] = "/=";
      ourTokenTextMap[PsiTokenType.ANDEQ] = "&=";
      ourTokenTextMap[PsiTokenType.OREQ] = "|=";
      ourTokenTextMap[PsiTokenType.XOREQ] = "^=";
      ourTokenTextMap[PsiTokenType.PERCEQ] = "%=";
      ourTokenTextMap[PsiTokenType.LTLTEQ] = "<<=";
      ourTokenTextMap[PsiTokenType.GTGTEQ] = ">>=";
      ourTokenTextMap[PsiTokenType.DOUBLE_COLON] = "::";
      ourTokenTextMap[PsiTokenType.DOUBLE_QUEST] = "??";
      ourTokenTextMap[PsiTokenType.ARROW] = "->";
      ourTokenTextMap[PsiTokenType.LAMBDA_ARROW] = "=>";

      for (int i = 0; i < ourHash.Length; i++)
        ourHash[i] = PsiTokenType.IDENTIFIER;
      for (IDictionaryEnumerator ide = keywords.GetEnumerator(); ide.MoveNext();)
      {
        ushort hash = CalcHash((string) ide.Entry.Key);
        Assertion.Assert(ourHash[hash] == PsiTokenType.IDENTIFIER,
                         "The condition (ourHash[hash] == PsiTokenType.IDENTIFIER) is false.");
        ourHash[hash] = (TokenNodeType) ide.Entry.Value;
      }
    }

    public PsiLexer(IBuffer buffer)
      : base(buffer)
    {
    }

    private static ushort CalcHash(string str)
    {
      ushort hash = 0;
      for (int i = 0; i < str.Length; i++)
        hash = IncHash(hash, str[i]);
      return hash;
    }

    private static ushort IncHash(ushort hash, char c)
    {
      return (ushort) (((hash << 1)) + Convert.ToUInt16(c));
    }

    public static string GetTokenText(TokenNodeType token)
    {
      if (ourTokenTextMap.ContainsKey(token))
      {
        return ourTokenTextMap.GetValue(token);
      } 
      if(ourKeywordTextMap.ContainsKey(token))
      {
        return ourKeywordTextMap.GetValue(token);
      }
      return token.ToString();
    }

    public static bool isKeyword(String s)
    {
      return ourKeywordTextMap.ContainsValue(s);
    }
  }
}