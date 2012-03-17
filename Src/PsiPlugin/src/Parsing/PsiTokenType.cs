using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Parsing
{
    public interface IPsiTokenNodeType
    {
    }

    public static partial class PsiTokenType
    {

        private abstract class PsiTokenNodeType : TokenNodeType, IPsiTokenNodeType
        {
            protected PsiTokenNodeType(string s)
                : base(s)
            {
            }

            public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
            {
                throw new InvalidOperationException();
            }

            public virtual string Presentation
            {
                get { return null; }
            }

            public override PsiLanguageType LanguageType
            {
                get { return PsiLanguage.Instance; }
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
        }

        private sealed class WhitespaceNodeType : PsiTokenNodeType
        {
            public WhitespaceNodeType() : base("WHITE_SPACE") { }

            public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
            {
                return new Whitespace(buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
            }
        }

        private sealed class NewLineNodeType : PsiTokenNodeType
        {
            public NewLineNodeType() : base("NEW_LINE") { }

            public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
            {
                return new NewLine(buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
            }
        }

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
                        return myPresentation;
                    return "'" + PsiLexer.GetTokenText(this) + "'";
                }
            }

            public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
            {
                return new PsiGenericToken(this, buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
            }
        }

        private class CommentNodeType : GenericTokenNodeType
        {
            public CommentNodeType(string s) : base(s) { }

            public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
            {
                return new Comment(this, buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
            }

            public override bool IsComment
            {
                get { return true; }
            }
        }

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

        private sealed class IdentifierNodeType : PsiTokenNodeType
        {
            public IdentifierNodeType() : base("IDENTIFIER") { }

            public override string Presentation
            {
                get { return "identifier"; }
            }

            public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
            {
                return new Identifier(buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
            }
        }

        private abstract class KeywordTokenNodeType : PsiTokenNodeType
        {
            protected KeywordTokenNodeType(string s) : base(s) { }

            public override string Presentation
            {
                get { return "'" + PsiLexer.GetTokenText(this) + "'"; }
            }
        }

        public static readonly TokenNodeType NEW_LINE = new NewLineNodeType();
        public static readonly TokenNodeType WHITE_SPACE = new WhitespaceNodeType ();
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
            INTERFACE,
            OPTIONS,
            PRIVATE,
            PATHS
          }
              );
            LITERALS = new NodeTypeSet(
              new NodeType[]
          {
            INTEGER_LITERAL, STRING_LITERAL, CHARACTER_LITERAL
          }
          );
        }
    }
}
