using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  partial class RoleName
  {
    private IReference myReference;
    private bool myInitReference;
    public override ReferenceCollection GetFirstClassReferences()
    {
      if (!myInitReference)
      {
        myReference = new PsiRoleReference(this);
        myInitReference = true;
      }
      return new ReferenceCollection(myReference);
    }
  }
}
