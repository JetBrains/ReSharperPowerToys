using JetBrains.ReSharper.Psi.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Tree
{
 public partial interface IRuleName
 {
   void SetName(string shortName);
   IReference RuleNameReference { get; }
 }
}
