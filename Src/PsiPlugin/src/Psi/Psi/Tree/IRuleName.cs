using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree
{
  public partial interface IRuleName
  {
    PsiRuleReference RuleNameReference { get; }
    void SetName(string shortName);
  }
}
