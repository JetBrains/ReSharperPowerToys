using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  internal partial class OptionName
  {
    private IReference myReference;
    private bool myInitReference;

    public override ReferenceCollection GetFirstClassReferences()
    {
      if (!myInitReference)
      {
        myReference = new PsiOptionReference(this);
        myInitReference = true;
      }
      return new ReferenceCollection(myReference);
    }
  }
}
