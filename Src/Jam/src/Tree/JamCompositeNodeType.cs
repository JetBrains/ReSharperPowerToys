using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;

namespace JetBrains.ReSharper.Psi.Jam.Tree
{
  public abstract class JamCompositeNodeType : CompositeNodeType
  {
    protected JamCompositeNodeType(string s) : base(s) {}
  }
}