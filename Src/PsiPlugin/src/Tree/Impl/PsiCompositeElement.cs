using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
    internal class PsiCompositeElement : CompositeElement, IPsiTreeNode
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

        /*public IPsiNamespaceDeclaration GetContainingNamespaceDeclaration()
        {
            return GetContainingNode<IPsiNamespaceDeclaration>(false);
        }

        public IPsiTypeMemberDeclaration GetContainingTypeMemberDeclaration()
        {
            return GetContainingNode<IPsiTypeMemberDeclaration>(false);
        }

        public IPsiTypeDeclaration GetContainingTypeDeclaration()
        {
            return GetContainingNode<IPsiTypeDeclaration>(false);
        }*/

        public IPsiStatement GetContainingStatement()
        {
            return GetContainingNode<IPsiStatement>(false);
        }

        public PsiLanguageType PresentationLanguage
        {
            get { return Language; }
        }

        public override NodeType NodeType
        {
            get { throw new NotImplementedException(); }
        }
    }
}
