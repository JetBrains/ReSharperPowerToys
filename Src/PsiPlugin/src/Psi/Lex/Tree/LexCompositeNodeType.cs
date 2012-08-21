using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Lex.Tree
{
  public abstract class LexCompositeNodeType : CompositeNodeType
  {
    protected LexCompositeNodeType(string s)
      : base(s)
    {
    }
  }
}
