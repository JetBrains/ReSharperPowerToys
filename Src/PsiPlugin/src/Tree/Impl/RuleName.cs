using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  internal partial class RuleName
  {
    private IReference myReference;
    private bool initReference = false;

    public override ReferenceCollection GetFirstClassReferences()
    {
      if (!initReference)
      {
        myReference = new PsiRuleReference(this);
        initReference = true;
      }
      return new ReferenceCollection(myReference);
    }

    public ResolveResultWithInfo Resolve()
    {
      if (!initReference)
      {
        myReference = new PsiRuleReference(this);
        initReference = true;
      }
      return myReference.Resolve();     
    }

    public void setReference(IReference reference)
    {
      myReference = reference;
      initReference = true;
    }

    public void SetName(string shortName)
    {
      ((PsiRuleReference)myReference).SetName(shortName);
    }
  }
}
