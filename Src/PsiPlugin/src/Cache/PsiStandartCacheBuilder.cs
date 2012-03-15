using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.Cache
{
  internal class PsiStandardCacheBuilder : IPsiCustomCacheBuilder
  {
    private readonly List<ICacheItem> mySymbols = new List<ICacheItem>();

    public Action ScanBeforeChildren(ITreeNode treeNode)
    {
      if(treeNode is RuleDeclaration)
      {
        mySymbols.Add(new PsiSymbol(treeNode));
      } else if (treeNode is OptionDefinition)
      {
        mySymbols.Add(new PsiSymbol(treeNode));
      }
      return null;
    }

    public IEnumerable<ICacheItem> Symbols
    {
      get { return mySymbols; }
    }
  }
}
