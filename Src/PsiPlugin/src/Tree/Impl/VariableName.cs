using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  internal partial class VariableName
  {
    private bool myInitReference;
    private IReference myReference;

    #region IVariableName Members

    public override ReferenceCollection GetFirstClassReferences()
    {
      if (!myInitReference)
      {
        myReference = new PsiVariableReference(this);
        myInitReference = true;
      }
      return new ReferenceCollection(myReference);
    }

    public void SetName(string shortName)
    {
      ((PsiVariableReference)myReference).SetName(shortName);
    }

    #endregion

    public ResolveResultWithInfo Resolve()
    {
      if (!myInitReference)
      {
        myReference = new PsiVariableReference(this);
        myInitReference = true;
      }
      return myReference.Resolve();
    }

    public IReference Reference
    {
      get
      {
        if (!myInitReference)
        {
          myReference = new PsiPathReference(this);
          myInitReference = true;
        }
        return myReference;
      }
    }
  }
}
