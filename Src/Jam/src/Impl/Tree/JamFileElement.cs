using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Jam.Tree;

namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  public abstract class JamFileElement : FileElementBase
  {
    public abstract void Accept(TreeNodeVisitor visitor);
    public abstract void Accept<TContext>(TreeNodeVisitor<TContext> visitor, TContext context);
    public abstract TReturn Accept<TContext, TReturn>(TreeNodeVisitor<TContext, TReturn> visitor, TContext context);
  }
}