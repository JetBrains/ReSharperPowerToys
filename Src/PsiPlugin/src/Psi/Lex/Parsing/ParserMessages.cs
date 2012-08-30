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
    public static string IDS_LEXER_OPTION = "lexer option";
    public static string IDS__R_E_DUPL_SYMBOL = " r e dupl symbol";
    public static string IDS__R_E_EXPRESSION = "r e expression";
    public static string IDS_BASIC_REG_EXP = "basic regexp";
    public static string IDS_NONDUPL__R_E = "nondupl r e";
    public static string IDS_ONE_CHARACTER__R_E = "one character r e";
    public static string IDS_SIMPLE__R_E = "simpl r e";
    public static string IDS_CHAR = "char";
    public static string IDS_BRACKET_EXPRESSION = "bracket expression";
    public static string IDS_BRACKET_LIST = "bracket list";
    public static string IDS_COLLATING_SYMBOL = "collating symbol";
    public static string IDS_EXPRESSION_TERM = "expression term";
    public static string IDS_FOLLOW_LIST = "follow list";
    public static string IDS_RANGE_EXPRESSION = "range expression";
    public static string IDS_SINGLE_EXPRESSION = "single expression";
    public static string IDS_REGEXP_LIST_ITEM = "regexp list item";
    public static string IDS_FILE_FULL_NAME = "file full name";
    public static string IDS_PATH_ELEMENT = "path element";
    public static string IDS_TOKEN_TYPE_USAGE = "token type usage";
    public static string IDS_CSHARP_KEYWORD = "csharp keyword";
    public static string IDS_PARSE_ITEM = "parse item";
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
