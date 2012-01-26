using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
    internal class Comment : PsiGenericToken, IPsiCommentNode, IChameleonNode
    {
        public Comment(TokenNodeType nodeType, string text)
            : base(nodeType, text)
        {
        }

        public override String ToString()
        {
            return base.ToString() + "(CommentType:" + Enum.Format(typeof(CommentType), CommentType, "G") + ")";
        }

        public CommentType CommentType
        {
            get
            {
                if (NodeType == PsiTokenType.C_STYLE_COMMENT)
                    return CommentType.MULTILINE_COMMENT;
                string text = GetText();
                if (text.StartsWith("///") && !text.StartsWith("////")) return CommentType.DOC_COMMENT;
                return CommentType.END_OF_LINE_COMMENT;
            }
        }

        public string CommentText
        {
            get
            {
                string text = GetText();
                switch (CommentType)
                {
                    case CommentType.END_OF_LINE_COMMENT:
                        return text.Substring(2);
                    case CommentType.DOC_COMMENT:
                        return text.Substring(3);
                    case CommentType.MULTILINE_COMMENT:
                        {
                            var length = text.Length - (text.EndsWith("*/") ? 4 : 2);
                            if (length <= 0)
                                return String.Empty;
                            return text.Substring(2, length);
                        }
                }
                return string.Empty;
            }
        }

        public TreeTextRange GetCommentRange()
        {
            TreeOffset startOffset = GetTreeStartOffset();
            switch (CommentType)
            {
                case CommentType.END_OF_LINE_COMMENT:
                    return new TreeTextRange(startOffset + 2, startOffset + GetTextLength());
                case CommentType.DOC_COMMENT:
                    return new TreeTextRange(startOffset + 3, startOffset + GetTextLength());
                case CommentType.MULTILINE_COMMENT:
                    {
                        string text = GetText();
                        var length = text.Length - (text.EndsWith("*/") ? 4 : 2);
                        if (length <= 0)
                            return TreeTextRange.InvalidRange;
                        return new TreeTextRange(startOffset + 2, startOffset + 2 + length);
                    }
            }
            return TreeTextRange.InvalidRange;
        }

        public override IChameleonNode FindChameleonWhichCoversRange(TreeTextRange textRange)
        {
            return textRange.ContainedIn(TreeTextRange.FromLength(GetTextLength())) ? this : null;
        }

        public virtual IChameleonNode ReSync(CachingLexer cachingLexer, TreeTextRange changedRange, int insertedTextLen)
        {
            TreeTextRange oldRange = this.GetTreeTextRange();

            Logger.Assert(changedRange.ContainedIn(oldRange), "The condition “changedRange.ContainedIn(oldRange)” is false.");

            int newLength = oldRange.Length - changedRange.Length + insertedTextLen;
            // Find starting comment
            if (!cachingLexer.FindTokenAt(oldRange.StartOffset.Offset) ||
                cachingLexer.TokenType != GetTokenType() ||
                cachingLexer.TokenStart != oldRange.StartOffset.Offset ||
                cachingLexer.TokenEnd != oldRange.StartOffset.Offset + newLength)
                return null;

            var element = TreeElementFactory.CreateLeafElement(cachingLexer.TokenType, new ProjectedBuffer(cachingLexer.Buffer, new TextRange(cachingLexer.TokenStart, cachingLexer.TokenStart + newLength)), 0, newLength);
            var comment = element as Comment;
            if (comment == null || CommentType != comment.CommentType)
                return null;
            return comment;
        }

        public bool IsOpened
        {
            get { return true; }
        }
    }
}
