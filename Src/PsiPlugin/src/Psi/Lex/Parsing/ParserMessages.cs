using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Lex.Parsing
{
  public static class ParserMessages
  {
    public static string IDS_CHAR_ID = "charId";
    public static string IDS_SPECIAL_CHAR = "special char";
    public static string IDS_INCLUDE_STATEMENT = "include statement";
    public static string IDS_TOKEN_DECLARATION = "token declaration";
    public static string IDS_FOLDER = "folder";
    public static string IDS_LEXING_EXPRESSION = "lexing expression";
    public static string IDS_QUALIFIER = "qualifier";
    public static string IDS_SIMPLE_EXPRESSION = "simple expression";
    private const string IDS_EXPECTED_SYMBOL = "{0} expected";
    private const string IDS_EXPECTED_TWO_SYMBOLS = "{0} or {1} expected";
    private const string IDS_UNEXPECTED_TOKEN = "Unexpected token";

    public static string GetString(string id)
    {
      return id;
    }

    public static string GetUnexpectedTokenMessage()
    {
      return IDS_UNEXPECTED_TOKEN;
    }

    public static string GetExpectedMessage(string expectedSymbol)
    {
      return String.Format(GetString(IDS_EXPECTED_SYMBOL), expectedSymbol).Capitalize();
    }

    public static string GetExpectedMessage(string firstExpectedSymbol, string secondExpectedSymbol)
    {
      return String.Format(GetString(IDS_EXPECTED_TWO_SYMBOLS), firstExpectedSymbol, secondExpectedSymbol).Capitalize();
    }
  }
}
