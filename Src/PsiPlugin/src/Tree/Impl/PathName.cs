using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  partial class PathName
  {
    private IReference myReference;
    private bool myInitReference;

    public override ReferenceCollection GetFirstClassReferences()
    {
      if (!myInitReference)
      {
        myReference = new PsiPathReference(this);
        myInitReference = true;
      }
      return new ReferenceCollection(myReference);
    }

    public ResolveResultWithInfo Resolve()
    {
      if (!myInitReference)
      {
        myReference = new PsiPathReference(this);
        myInitReference = true;
      }
      return myReference.Resolve();
    }

    public void SetReference(IReference reference)
    {
      myReference = reference;
      myInitReference = true;
    }
  }
}
