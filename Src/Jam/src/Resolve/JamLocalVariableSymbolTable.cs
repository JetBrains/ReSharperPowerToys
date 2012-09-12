using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi.Dependencies;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using System.Linq;
using JetBrains.Util;
using JetBrains.Util.DataStructures;

namespace JetBrains.ReSharper.Psi.Jam.Resolve
{
  internal class JamLocalVariableSymbolTable : SymbolTableBase
  {
    private readonly CompactOneToListMap<string, ILocalVariableDeclaredElement> myElements = new CompactOneToListMap<string, ILocalVariableDeclaredElement>(StringComparer.Ordinal);

    public JamLocalVariableSymbolTable(ITreeNode node)
    {
      var processor = new RecursiveElementProcessor<ILocalVariableDeclaration>(declaration => myElements.AddValue(declaration.DeclaredName, declaration.DeclaredElement)) {InteriorShouldBeProcessedHandler = treeNode => !(treeNode is IResolveIsolationScope)};
      
      foreach (var scope in node.SelfAndPathToRoot().OfType<IResolveIsolationScope>())
        scope.ProcessDescendants(processor);

      myElements.Compact();
    }

    public override IEnumerable<string> Names()
    {
      return myElements.Keys;
    }

    public override IList<ISymbolInfo> GetSymbolInfos(string name)
    {
      return myElements[name].Select(element => (ISymbolInfo) new SymbolInfo(element)).ToList();
    }

    public override void ForAllSymbolInfos(Action<ISymbolInfo> processor)
    {
      myElements.AllValues.ForEach(element => processor(new SymbolInfo(element)));
    }

    public override ISymbolTableDependencySet GetDependencySet()
    {
      return null;
    }
  }
}