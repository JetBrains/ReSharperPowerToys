using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  internal partial class RuleName
  {
    private PsiRuleReference myRuleNameReference;

    #region IRuleName Members

    public IReference RuleNameReference
    {
      get
      {
        lock (this)
          return myRuleNameReference ?? (myRuleNameReference = new PsiRuleReference(this));
      }
    }

    public override ReferenceCollection GetFirstClassReferences()
    {
      return new ReferenceCollection(RuleNameReference);
    }

    public void SetName(string shortName)
    {
      ((PsiRuleReference)RuleNameReference).SetName(shortName);
    }

    #endregion
  }
}
