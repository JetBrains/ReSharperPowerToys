using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  internal partial class RoleName
  {
    private bool myInitReference;
    private IReference myReference;

    #region IRoleName Members

    public override ReferenceCollection GetFirstClassReferences()
    {
      if (!myInitReference)
      {
        myReference = new PsiRoleReference(this);
        myInitReference = true;
      }
      return new ReferenceCollection(myReference);
    }

    #endregion
  }
}
