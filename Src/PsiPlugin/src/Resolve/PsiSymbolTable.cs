using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.Dependencies;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  public class PsiSymbolTable : ISymbolTable
  {
    public IEnumerable<string> Names()
    {
      throw new NotImplementedException();
    }

    public IList<ISymbolInfo> GetSymbolInfos(string name)
    {
      throw new NotImplementedException();
    }

    public IList<ISymbolInfo> GetSymbolInfosConstitutingResolveResult(string name, out IResolveInfo resolveInfo)
    {
      throw new NotImplementedException();
    }

    public void ForAllSymbolInfos(Action<ISymbolInfo> processor)
    {
      throw new NotImplementedException();
    }

    public ISymbolTableDependencySet GetDependencySet()
    {
      throw new NotImplementedException();
    }
  }
}
