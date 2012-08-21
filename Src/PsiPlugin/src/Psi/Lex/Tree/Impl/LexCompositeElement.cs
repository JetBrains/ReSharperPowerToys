using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Lex.Tree.Impl
{
  public abstract class LexCompositeElement : CompositeElement, ILexTreeNode
  {
    #region ILexTreeNode Members

    public virtual void Accept(TreeNodeVisitor visitor)
    {
      visitor.VisitNode(this);
    }

    public virtual void Accept<TContext>(TreeNodeVisitor<TContext> visitor, TContext context)
    {
      visitor.VisitNode(this, context);
    }

    public virtual TReturn Accept<TContext, TReturn>(TreeNodeVisitor<TContext, TReturn> visitor, TContext context)
    {
      return visitor.VisitNode(this, context);
    }


    public override PsiLanguageType Language
    {
      get { return LexLanguage.Instance; }
    }

    #endregion
  }
}
