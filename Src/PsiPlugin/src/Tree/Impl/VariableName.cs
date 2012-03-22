using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  partial class VariableName
  {
    private IReference myReference;
    private bool myInitReference;
    public override ReferenceCollection GetFirstClassReferences()
    {
      if (!myInitReference)
      {
        myReference = new PsiVariableReference(this);
        myInitReference = true;
      }
      return new ReferenceCollection(myReference);
    }

    public ResolveResultWithInfo Resolve()
    {
      if (!myInitReference)
      {
        myReference = new PsiVariableReference(this);
        myInitReference = true;
      }
      return myReference.Resolve();
    }

    public void SetName(string shortName)
    {
      ((PsiVariableReference)myReference).SetName(shortName);
    }
  }
}
