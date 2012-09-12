using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Jam.Cache;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.Resolve
{
  internal static class JamSymbolTableBuilder 
  {
    public static ISymbolTable BuildLocalVariableTable(ITreeNode node)
    {
      return new JamLocalVariableSymbolTable(node);
    }

    public static ISymbolTable BuildGlobalVariableTable(JamSymbolsCache symbolsCache)
    {
      return new JamGlobalVariableSymbolTable(symbolsCache);
    }

    public static ISymbolTable BuildProcedureTable(JamSymbolsCache symbolsCache)
    {
      return new JamProcedureSymbolTable(symbolsCache);
    }

    public static ISymbolTable BuildParameterTable(ITreeNode node)
    {
      return new JamParameterSymbolTable(node);
    }
  }
}