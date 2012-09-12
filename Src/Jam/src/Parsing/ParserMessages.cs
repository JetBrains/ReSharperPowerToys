using System;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Jam.Parsing
{
  public static class ParserMessages
  {
    private const string IDS_EXPECTED_SYMBOL = "{0} expected";
    private const string IDS_EXPECTED_TWO_SYMBOLS = "{0} or {1} expected";
    private const string IDS_UNEXPECTED_TOKEN = "Unexpected token";

// ReSharper disable InconsistentNaming
    public const string IDS_WS = "whitespace";
    public const string IDS__IDENTIFIER = "identifier";

    public const string IDS_JAM_DECLARATION = "declaration";
    public const string IDS_JAM_STATEMENT = "statement";
    public const string IDS_JAM_EXPRESSION = "expression";
    public const string IDS_PRIMITIVE_EXPRESSION = "expression";
    public const string IDS_LITERAL_EXPRESSION = "expression";
// ReSharper restore InconsistentNaming

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