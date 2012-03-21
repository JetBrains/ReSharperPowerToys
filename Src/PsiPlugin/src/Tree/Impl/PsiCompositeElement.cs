using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
    internal abstract class PsiCompositeElement : CompositeElement, IPsiTreeNode
    {
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
          get { return PsiLanguage.Instance; }
        }
    }
}
