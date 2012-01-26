using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  partial class PathName
  {
    private IReference myReference;
    private bool initReference = false;

    public override ReferenceCollection GetFirstClassReferences()
    {
      if (!initReference)
      {
        myReference = new PsiPathReference(this);
        initReference = true;
      }
      return new ReferenceCollection(myReference);
    }

    public ResolveResultWithInfo Resolve()
    {
      if (!initReference)
      {
        myReference = new PsiPathReference(this);
        initReference = true;
      }
      return myReference.Resolve();
    }

    public void SetReference(IReference reference)
    {
      myReference = reference;
      initReference = true;
    }

    public void SetName(string shortName)
    {
      //((PsiPathReference)myReference).SetName(shortName);
    }
  }
}
