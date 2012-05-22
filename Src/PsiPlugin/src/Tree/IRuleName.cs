using JetBrains.ReSharper.Psi.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Tree
{
  public partial interface IRuleName
  {
    IReference RuleNameReference { get; }
    void SetName(string shortName);
  }
}
