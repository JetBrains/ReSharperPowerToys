using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Tree
{
 public partial interface IRuleName
 {
   void setReference(IReference reference);
   void SetName(string shortName);
 }
}
