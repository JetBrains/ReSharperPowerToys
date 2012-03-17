using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Tree
{
  partial interface IPathName
  {
    void SetName(string shortName);
    void SetReference(IReference reference);
  }
}
