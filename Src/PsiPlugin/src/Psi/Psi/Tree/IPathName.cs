using JetBrains.ReSharper.Psi.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree
{
  partial interface IPathName
  {
    IReference RuleNameReference { get; }
    void SetReference(IReference reference);
  }
}
