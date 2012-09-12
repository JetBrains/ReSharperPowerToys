using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree
{
  public abstract class PsiCompositeNodeType : CompositeNodeType
  {
    protected PsiCompositeNodeType(string s)
      : base(s)
    {
    }
  }
}
