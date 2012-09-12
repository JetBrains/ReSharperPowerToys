using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi.Dependencies;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using JetBrains.Util.DataStructures;

namespace JetBrains.ReSharper.Psi.Jam.Resolve
{
  internal class JamParameterSymbolTable : SymbolTableBase
  {
    private readonly CompactOneToListMap<string, IParameterDeclaredElement> myElements = new CompactOneToListMap<string, IParameterDeclaredElement>(StringComparer.Ordinal);

    public JamParameterSymbolTable(ITreeNode node)
    {
      var procedureDeclaration = node.GetContainingNode<IProcedureDeclaration>();
      if (procedureDeclaration != null && procedureDeclaration.ParameterList != null)
      {
        foreach (var element in procedureDeclaration.ParameterList.Parameters.SelectNotNull(d => d.DeclaredElement))
          myElements.AddValue(element.ShortName, element);
      }

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