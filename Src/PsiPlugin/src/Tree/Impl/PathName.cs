using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  internal partial class PathName
  {
    private bool myInitReference;
    private IReference myReference;

    #region IPathName Members

    public override ReferenceCollection GetFirstClassReferences()
    {
      if (!myInitReference)
      {
        myReference = new PsiPathReference(this);
        myInitReference = true;
      }
      return new ReferenceCollection(myReference);
    }

    public IReference RuleNameReference
    {
      get
      {
        lock (this)
          return myReference ?? (myReference = new PsiPathReference(this));
      }
    }

    public void SetReference(IReference reference)
    {
      myReference = reference;
      myInitReference = true;
    }

    #endregion

    public ResolveResultWithInfo Resolve()
    {
      if (!myInitReference)
      {
        myReference = new PsiPathReference(this);
        myInitReference = true;
      }
      return myReference.Resolve();
    }
  }
}
