using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Lex.Tree
{
  public interface ILexTreeNode : ITreeNode
  {
    void Accept(TreeNodeVisitor visitor);
    void Accept<TContext>(TreeNodeVisitor<TContext> visitor, TContext context);
    TResult Accept<TContext, TResult>(TreeNodeVisitor<TContext, TResult> visitor, TContext context);
  }
}
