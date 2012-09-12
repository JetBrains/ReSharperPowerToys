using JetBrains.ReSharper.Psi.Jam.Cache;

namespace JetBrains.ReSharper.Psi.Jam.Resolve
{
  internal class JamGlobalVariableSymbolTable : JamCacheSymbolTableBase
  {
    public JamGlobalVariableSymbolTable(JamSymbolsCache symbolsCache) : base(symbolsCache, JamSymbolType.GlobalVariable) {}
  }
}