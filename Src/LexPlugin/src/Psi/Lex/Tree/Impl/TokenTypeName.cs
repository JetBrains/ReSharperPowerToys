using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.LexPlugin.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree.Impl
{
  internal partial class TokenTypeName
  {
    private LexTokenReference myTokenNameReference;

    public LexTokenReference RuleNameReference
    {
      get
      {
        lock (this)
          return myTokenNameReference ?? (myTokenNameReference = new LexTokenReference(this));
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
