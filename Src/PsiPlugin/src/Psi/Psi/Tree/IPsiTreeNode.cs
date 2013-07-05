using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree
{
  public interface IPsiTreeNode : ITreeNode
  {
    void Accept(TreeNodeVisitor visitor);
    void Accept<TContext>(TreeNodeVisitor<TContext> visitor, TContext context);
    TResult Accept<TContext, TResult>(TreeNodeVisitor<TContext, TResult> visitor, TContext context);
  }
}
