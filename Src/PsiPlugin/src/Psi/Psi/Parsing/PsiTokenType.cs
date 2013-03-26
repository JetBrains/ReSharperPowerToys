using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing
{
  public interface IPsiTokenNodeType
  {
  }

  public static partial class PsiTokenType
  {
    public static readonly TokenNodeType NEW_LINE = new NewLineNodeType();
    public static readonly TokenNodeType WHITE_SPACE = new WhitespaceNodeType();
    public static readonly TokenNodeType IDENTIFIER = new IdentifierNodeType();
    public static readonly TokenNodeType INTEGER_LITERAL = new GenericTokenNodeType("INTEGER_LITERAL", "integer literal");
    public static readonly TokenNodeType STRING_LITERAL = new GenericTokenNodeType("STRING_LITERAL", "string literal");
    public static readonly TokenNodeType END_OF_LINE_COMMENT = new EndOfLineCommentNodeType();
    public static readonly TokenNodeType C_STYLE_COMMENT = new CommentNodeType("C_STYLE_COMMENT");
    public static readonly TokenNodeType CHARACTER_LITERAL = new GenericTokenNodeType("CHARACTER_LITERAL", "char literal");

    public static readonly TokenNodeType EOF = new GenericTokenNodeType("EOF");

    public static readonly NodeTypeSet KEYWORDS;
    public static readonly NodeTypeSet LITERALS;

    static PsiTokenType()
    {
      KEYWORDS = new NodeTypeSet(
        new NodeType[]
        {
          ABSTRACT,
          ERRORHANDLING,
          EXTRAS,
          GET,
          GETTER,
          OPTIONS,
          INTERFACE,
          INTERFACES,
          ISCACHED,
          PRIVATE,
          PATHS,
          RETURN_TYPE,
          ROLE_KEYWORD,
          CACHED,
          NULL_KEYWORD,
          LIST_KEYWORD,
          SEP_KEYWORD
        }
        );
      LITERALS = new NodeTypeSet(
        new NodeType[]
        {
          INTEGER_LITERAL, STRING_LITERAL, CHARACTER_LITERAL
        }
        );
    }

    #region Nested type: CommentNodeType

    private class CommentNodeType : GenericTokenNodeType
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

    private sealed class EndOfLineCommentNodeType : CommentNodeType
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

    private class FixedTokenElement : PsiTokenBase
    {
      private readonly FixedTokenNodeType myKeywordTokenNodeType;

      public FixedTokenElement(FixedTokenNodeType keywordTokenNodeType)
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

    private class FixedTokenNodeType : GenericTokenNodeType
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
        return new FixedTokenElement(this);
      }
    }

    #endregion

    #region Nested type: GenericTokenNodeType

    private class GenericTokenNodeType : PsiTokenNodeType
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
          return "'" + PsiLexer.GetTokenText(this) + "'";
        }
      }

      public override string TokenRepresentation
      {
        get { return myPresentation; }
      }

      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new PsiGenericToken(this, buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
      }
    }

    #endregion

    #region Nested type: IdentifierNodeType

    private sealed class IdentifierNodeType : PsiTokenNodeType
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

    private class KeywordTokenNodeType : FixedTokenNodeType
    {
      public KeywordTokenNodeType(string s, string representation)
        : base(s, representation)
      {
      }
    }

    #endregion

    #region Nested type: NewLineNodeType

    private sealed class NewLineNodeType : PsiTokenNodeType
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

    #region Nested type: PsiTokenNodeType

    private abstract class PsiTokenNodeType : TokenNodeType, IPsiTokenNodeType
    {
      protected PsiTokenNodeType(string s)
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

    private sealed class WhitespaceNodeType : PsiTokenNodeType
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
