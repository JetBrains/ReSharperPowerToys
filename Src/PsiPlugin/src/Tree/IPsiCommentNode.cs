using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Tree
{
    public enum CommentType : byte
    {
        END_OF_LINE_COMMENT, // example: //
        MULTILINE_COMMENT,   // example: /* */
        DOC_COMMENT          // example: ///
    }

    public interface IPsiCommentNode : ICommentNode
    {
        CommentType CommentType { get; }
    }

    public interface IDocCommentNode : IPsiCommentNode
    {
        IDocCommentNode ReplaceBy(IDocCommentNode docCommentNode);
    }
}
