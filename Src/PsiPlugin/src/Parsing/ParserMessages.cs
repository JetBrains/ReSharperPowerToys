using System;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Parsing
{
    public class ParserMessages
    {
    public const string IDS_EXPECTED_SYMBOL = "{0} expected";
    public const string IDS_EXPECTED_TWO_SYMBOLS = "{0} or {1} expected";

    public const string IDS_UNEXPECTED_TOKEN = "Unexpected token";

    public const string IDS_MODIFIER = "modofier";
    public const string IDS_NOT_CHOICE_EXPRESSION = "not choice expression";
    public const string IDS_QUANTIFIER = "quantifier";
    public const string IDS_SIMPLE_EXPRESSION = "simple expression";
    public const string IDS_OPTION_NAME = "option name";
    public const string IDS_RULE_PARAMETERS = "rule parameters";
    public const string IDS_PAREN_EXPRESSION = "paren expression";
    public const string IDS_ROLE_NAME = "role name";
    public const string IDS_VARIABLE_NAME = "variable name";
    public const string IDS_PREDICATED = "predicated";
    public const string IDS_RULE_NAME = "rule name";

    public static string GetString (string id)
    {
      return id;
    }   

    public static string GetUnexpectedTokenMessage ()
    {
      return IDS_UNEXPECTED_TOKEN;
    }

      public static string GetExpectedMessage (string expectedSymbol)
    {
      return String.Format(GetString(IDS_EXPECTED_SYMBOL), expectedSymbol).Capitalize();
    }

    public static string GetExpectedMessage (string firstExpectedSymbol, string secondExpectedSymbol)
    {
      return String.Format(GetString(IDS_EXPECTED_TWO_SYMBOLS), firstExpectedSymbol, secondExpectedSymbol).Capitalize();
    }
  }
}
