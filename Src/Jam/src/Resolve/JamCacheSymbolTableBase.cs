using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi.Dependencies;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Jam.Cache;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.Util;
using JetBrains.Util.Special;

namespace JetBrains.ReSharper.Psi.Jam.Resolve
{
  internal abstract class JamCacheSymbolTableBase : SymbolTableBase
  {
    private readonly JamSymbolsCache mySymbolsCache;
    private readonly JamSymbolType myJamSymbolType;

    protected JamCacheSymbolTableBase(JamSymbolsCache symbolsCache, JamSymbolType jamSymbolType)
    {
      mySymbolsCache = symbolsCache;
      myJamSymbolType = jamSymbolType;
    }

    public override IEnumerable<string> Names()
    {
      return mySymbolsCache.GetNames(myJamSymbolType);
    }

    public override IList<ISymbolInfo> GetSymbolInfos(string name)
    {
      var jamSymbols = mySymbolsCache.GetSymbols(name).Where(symbol => symbol.SymbolType == myJamSymbolType);
      return jamSymbols.SelectNotNull(symbol => symbol.GetDeclaration().IfNotNull(d => d.DeclaredElement)).Select(element => (ISymbolInfo) new SymbolInfo(element)).ToList();
    }

    public override void ForAllSymbolInfos(Action<ISymbolInfo> processor)
    {
      var jamSymbols = mySymbolsCache.GetSymbols(myJamSymbolType);
      jamSymbols.SelectNotNull(symbol => symbol.GetDeclaration().IfNotNull(d => d.DeclaredElement)).ForEach(element => processor(new SymbolInfo(element)));
    }

    public override ISymbolTableDependencySet GetDependencySet()
    {
      return null;
    }
  }
}