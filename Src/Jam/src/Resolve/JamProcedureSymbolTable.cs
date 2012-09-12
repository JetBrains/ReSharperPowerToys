using JetBrains.ReSharper.Psi.Jam.Cache;

namespace JetBrains.ReSharper.Psi.Jam.Resolve
{
  internal class JamProcedureSymbolTable : JamCacheSymbolTableBase
  {
    public JamProcedureSymbolTable(JamSymbolsCache symbolsCache) : base(symbolsCache, JamSymbolType.Procedure) {}
  }
}