using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Psi.Lex.Parsing;
using JetBrains.ReSharper.PsiPlugin.Psi.Lex.Tree.Impl;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Lex.Parsing
{
  public interface ILexTokenNodeType
  {
  }

  public partial class LexTokenType
  {
   public static readonly TokenNodeType NEW_LINE = new LexTokenType.NewLineNodeType();
    public static readonly TokenNodeType WHITE_SPACE = new LexTokenType.WhitespaceNodeType();
    public static readonly TokenNodeType IDENTIFIER = new LexTokenType.IdentifierNodeType();
    public static readonly TokenNodeType INTEGER_LITERAL = new LexTokenType.GenericTokenNodeType("INTEGER_LITERAL", "integer literal");
    public static readonly TokenNodeType STRING_LITERAL = new LexTokenType.GenericTokenNodeType("STRING_LITERAL", "string literal");
    public static readonly TokenNodeType END_OF_LINE_COMMENT = new LexTokenType.EndOfLineCommentNodeType();
    public static readonly TokenNodeType C_STYLE_COMMENT = new LexTokenType.CommentNodeType("C_STYLE_COMMENT");

    public static readonly TokenNodeType EOF = new LexTokenType.GenericTokenNodeType("EOF");

    public static readonly NodeTypeSet KEYWORDS;
    public static readonly NodeTypeSet LITERALS;

    static LexTokenType()
    {
      KEYWORDS = new NodeTypeSet(
        new NodeType[]
        {
          USING_KEYWORD,
          INIT_KEYWORD,
          INCLUDE_KEYWORD,
          EOFVAL_KEYWORD,
          VIRTUAL_KEYWORD,
          FUNCTION_KEYWORD,
          IMPLEMENTS_KEYWORD,
          PUBLIC_KEYWORD,
          CLASS_KEYWORD,
          NAMESPACE_KEYWORD,
          RETURN_KEYWORD,
          NULL_KEYWORD,
          STATE_KEYWORD
        }
        );
      LITERALS = new NodeTypeSet(
        new NodeType[]
        {
          INTEGER_LITERAL, STRING_LITERAL
        }
        );
    }

    #region Nested type: CommentNodeType

    private class CommentNodeType : LexTokenType.GenericTokenNodeType
    {
      public CommentNodeType(string s)
        : base(s)
      {
      }

      public override bool IsComment
      {
        get { return true; }
      }

      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new Comment(this, buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
      }
    }

    #endregion

    #region Nested type: EndOfLineCommentNodeType

    private sealed class EndOfLineCommentNodeType : LexTokenType.CommentNodeType
    {
      public EndOfLineCommentNodeType()
        : base("END_OF_LINE_COMMENT")
      {
      }

      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new Comment(this, buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
      }
    }

    #endregion

    #region Nested type: FixedTokenElement

    private class FixedTokenElement : LexTokenBase
    {
      private readonly LexTokenType.FixedTokenNodeType myKeywordTokenNodeType;

      public FixedTokenElement(LexTokenType.FixedTokenNodeType keywordTokenNodeType)
      {
        myKeywordTokenNodeType = keywordTokenNodeType;
      }

      public override NodeType NodeType
      {
        get { return myKeywordTokenNodeType; }
      }

      public override int GetTextLength()
      {
        return myKeywordTokenNodeType.TokenRepresentation.Length;
      }

      public override string GetText()
      {
        return myKeywordTokenNodeType.TokenRepresentation;
      }
    }

    #endregion

    #region Nested type: FixedTokenNodeType

    private class FixedTokenNodeType : LexTokenType.GenericTokenNodeType
    {
      private readonly string myRepresentation;

      public FixedTokenNodeType(string s, string representation)
        : base(s)
      {
        myRepresentation = representation;
      }

      public override string TokenRepresentation
      {
        get { return myRepresentation; }
      }

      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new LexTokenType.FixedTokenElement(this);
      }
    }

    #endregion

    #region Nested type: GenericTokenNodeType

    private class GenericTokenNodeType : LexTokenType.LexTokenNodeType
    {
      private readonly string myPresentation;

      public GenericTokenNodeType(string s, string presentation = null)
        : base(s)
      {
        myPresentation = presentation;
      }

      public override string Presentation
      {
        get
        {
          if (myPresentation != null)
          {
            return myPresentation;
          }
          return "'" + LexLexer.GetTokenText(this) + "'";
        }
      }

      public override string TokenRepresentation
      {
        get { return myPresentation; }
      }

      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new LexGenericToken(this, buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
      }
    }

    #endregion

    #region Nested type: IdentifierNodeType

    private sealed class IdentifierNodeType : LexTokenType.LexTokenNodeType
    {
      public IdentifierNodeType()
        : base("IDENTIFIER")
      {
      }

      public override string Presentation
      {
        get { return "identifier"; }
      }

      public override string TokenRepresentation
      {
        get { return "identifier"; }
      }

      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new Identifier(buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
      }
    }

    #endregion

    #region Nested type: KeywordTokenNodeType

    private class KeywordTokenNodeType : LexTokenType.FixedTokenNodeType
    {
      public KeywordTokenNodeType(string s, string representation)
        : base(s, representation)
      {
      }
    }

    #endregion

    #region Nested type: NewLineNodeType

    private sealed class NewLineNodeType : LexTokenType.LexTokenNodeType
    {
      public NewLineNodeType()
        : base("NEW_LINE")
      {
      }

      public override string TokenRepresentation
      {
        get { return "\r\n"; }
      }

      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new NewLine(buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
      }
    }

    #endregion

    #region Nested type: LexTokenNodeType

    private abstract class LexTokenNodeType : TokenNodeType, ILexTokenNodeType
    {
      protected LexTokenNodeType(string s)
        : base(s)
      {
      }

      public virtual string Presentation
      {
        get { return null; }
      }

      public override bool IsWhitespace
      {
        get { return this == WHITE_SPACE || this == NEW_LINE; }
      }

      public override bool IsComment
      {
        get { return false; }
      }

      public override bool IsStringLiteral
      {
        get { return this == STRING_LITERAL; }
      }

      public override bool IsConstantLiteral
      {
        get { return LITERALS[this]; }
      }

      public override bool IsIdentifier
      {
        get { return this == IDENTIFIER; }
      }

      public override bool IsKeyword
      {
        get { return KEYWORDS[this]; }
      }

      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        throw new InvalidOperationException();
      }
    }

    #endregion

    #region Nested type: WhitespaceNodeType

    private sealed class WhitespaceNodeType : LexTokenType.LexTokenNodeType
    {
      public WhitespaceNodeType()
        : base("WHITE_SPACE")
      {
      }

      public override string TokenRepresentation
      {
        get { return " "; }
      }

      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new Whitespace(buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
      }
    }

    #endregion
  }
}
