using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Lex.Tree
{
  public enum CommentType : byte
  {
    EndOfLineComment, // example: //
    MultilineComment, // example: /* */
    DocComment // example: ///
  }

  public interface ILexCommentNode : ICommentNode
  {
  }
}
