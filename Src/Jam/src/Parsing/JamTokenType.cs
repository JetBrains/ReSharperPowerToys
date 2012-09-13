using System;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Jam.Impl.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Jam.Parsing
{
  public class JamTokenType : TokenNodeType
  {
    // ReSharper disable InconsistentNaming
    public static readonly TokenNodeType EOF = new JamTokenType("OEF");
    public static readonly TokenNodeType BAD_CHARACTER = new JamTokenType("BAD_CHARACTER", "a");

    public static readonly TokenNodeType NEW_LINE = new JamNewLineTokenType();
    public static readonly TokenNodeType WHITE_SPACE = new JamWhiteSpaceTokenType();

    public static readonly TokenNodeType INTEGER_LITERAL = new JamLiteralTokenType("INTEGER_LITERAL", "0");
    public static readonly TokenNodeType REAL_LITERAL = new JamLiteralTokenType("REAL_LITERAL", "0.0");

    public static readonly TokenNodeType STRING_LITERAL = new JamLiteralTokenType("STRING_LITERAL", "\"string\"");

    public static readonly TokenNodeType IDENTIFIER = new JamIdentifierNodeType();

    public static readonly TokenNodeType COMMENT = new JamCommentTokenType("COMMENT", "/*comment*/");

    public static readonly TokenNodeType SEMICOLON = new JamTokenType("SEMICOLON", ";");
    public static readonly TokenNodeType EQEQ = new JamTokenType("EQEQ", "==");
    public static readonly TokenNodeType NEQ = new JamTokenType("NEQ", "!=");
    public static readonly TokenNodeType EQUALS = new JamTokenType("EQUALS", "=");
    public static readonly TokenNodeType COMMA = new JamTokenType("COMMA", ",");
    public static readonly TokenNodeType PLUS = new JamTokenType("PLUS", "+");
    public static readonly TokenNodeType MINUS = new JamTokenType("MINUS", "-");
    public static readonly TokenNodeType DIVIDE = new JamTokenType("DIVIDE", "/");
    public static readonly TokenNodeType MULTIPLY = new JamTokenType("MULTIPLY", "*");
    public static readonly TokenNodeType GE = new JamTokenType("GE", ">=");
    public static readonly TokenNodeType LE = new JamTokenType("LE", "<=");
    public static readonly TokenNodeType GT = new JamTokenType("GT", ">");
    public static readonly TokenNodeType LT = new JamTokenType("LT", "<");
    public static readonly TokenNodeType LPAREN = new JamTokenType("LPAREN", "(");
    public static readonly TokenNodeType RPAREN = new JamTokenType("RPAREN", ")");
    public static readonly TokenNodeType LBRACE = new JamTokenType("LBRACE", "{");
    public static readonly TokenNodeType RBRACE = new JamTokenType("RBRACE", "}");

    public static readonly TokenNodeType IF_KEYWORD = new JamKeywordTokenType("IF_KEYWORD", "if");
    public static readonly TokenNodeType ELSE_KEYWORD = new JamKeywordTokenType("ELSE_KEYWORD", "else");
    public static readonly TokenNodeType VAR_KEYWORD = new JamKeywordTokenType("VAR_KEYWORD", "var");
    public static readonly TokenNodeType SUB_KEYWORD = new JamKeywordTokenType("SUB_KEYWORD", "sub");
    public static readonly TokenNodeType RETURN_KEYWORD = new JamKeywordTokenType("RETURN_KEYWORD", "return");
    // ReSharper restore InconsistentNaming

    private readonly string myRepresentation;

    public JamTokenType(string s, string representation = "") : base(s)
    {
      myRepresentation = representation;
    }

    public override bool IsKeyword
    {
      get { return false; }
    }

    public override string TokenRepresentation
    {
      get { return myRepresentation; }
    }

    public override bool IsStringLiteral
    {
      get { return false; }
    }

    public override bool IsConstantLiteral
    {
      get { return false; }
    }

    public override bool IsComment
    {
      get { return false; }
    }

    public override bool IsWhitespace
    {
      get { return false; }
    }

    public override bool IsIdentifier
    {
      get { return false; }
    }

    public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
    {
      return new JamToken(this, buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
    }

    #region Nested type: JamCommentTokenType

    private class JamCommentTokenType : JamTokenType
    {
      public JamCommentTokenType(string name, string representation = "") : base(name, representation) {}

      public override bool IsComment
      {
        get { return true; }
      }

      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new JamComment(this, buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
      }
    }

    #endregion

    #region Nested type: JamIdentifierNodeType

    public class JamIdentifierNodeType : JamTokenType
    {
      public JamIdentifierNodeType() : base("IDENTIFIER", "-identifier") {}

      public override bool IsIdentifier
      {
        get { return true; }
      }

      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new JamIdentifierToken(buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
      }
    }

    #endregion

    #region Nested type: JamKeywordTokenType

    private class JamKeywordTokenType : JamTokenType
    {
      public JamKeywordTokenType(string name, string representation) : base(name, representation) {}

      public override bool IsKeyword
      {
        get { return true; }
      }

      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new JamKeyword(this, buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
      }
    }

    #endregion

    #region Nested type: JamLiteralTokenType

    private class JamLiteralTokenType : JamTokenType
    {
      public JamLiteralTokenType(string name, string represenatation) : base(name, represenatation) {}

      public override bool IsConstantLiteral
      {
        get { return true; }
      }

      public override bool IsStringLiteral
      {
        get { return this == STRING_LITERAL; }
      }

      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new JamLiteralToken(this, buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
      }
    }

    #endregion

    #region Nested type: JamNewLineTokenType

    private class JamNewLineTokenType : JamTokenType
    {
      public JamNewLineTokenType() : base("NEW_LINE", Environment.NewLine) {}

      public override bool IsWhitespace
      {
        get { return true; }
      }

      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new JamNewLineToken(buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
      }
    }

    #endregion

    #region Nested type: JamWhiteSpaceTokenType

    private class JamWhiteSpaceTokenType : JamTokenType
    {
      public JamWhiteSpaceTokenType() : base("WHITE_SPACE", " ") {}

      public override bool IsWhitespace
      {
        get { return true; }
      }

      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new JamWhitespaceToken(buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
      }
    }

    #endregion
  }
}