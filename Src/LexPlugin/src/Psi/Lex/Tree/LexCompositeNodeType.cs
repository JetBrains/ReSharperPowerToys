using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;

namespace JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree
{
  public abstract class LexCompositeNodeType : CompositeNodeType
  {
    protected LexCompositeNodeType(string s, int index)
      : base(s, index)
    {
    }
  }
}
