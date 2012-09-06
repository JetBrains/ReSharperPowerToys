using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.LexPlugin.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree.Impl
{
  internal partial class StateName
  {
    private LexStateReference myStateNameReference;

    public LexStateReference RuleNameReference
    {
      get
      {
        lock (this)
          return myStateNameReference ?? (myStateNameReference = new LexStateReference(this));
      }
    }

    public override ReferenceCollection GetFirstClassReferences()
    {
      return new ReferenceCollection(RuleNameReference);
    }

    public void SetName(string shortName)
    {
      RuleNameReference.SetName(shortName);
    }
  }
}
