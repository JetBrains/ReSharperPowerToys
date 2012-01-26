using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
    public abstract class PsiFileElement : FileElementBase
    {
        public abstract void Accept(TreeNodeVisitor visitor);
        public abstract void Accept<TContext>(TreeNodeVisitor<TContext> visitor, TContext context);
        public abstract TReturn Accept<TContext, TReturn>(TreeNodeVisitor<TContext, TReturn> visitor, TContext context);
    }
}