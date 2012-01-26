using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  public class PsiResolveInfo : IResolveInfo
  {
    public PsiResolveInfo()
    {
      
    }

    public ResolveErrorType ResolveErrorType
    {
      get { return ResolveErrorType.OK; }
    }
  }
}
