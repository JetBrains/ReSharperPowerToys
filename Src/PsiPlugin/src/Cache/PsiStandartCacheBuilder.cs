using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Cach
{
  internal class PsiStandardCacheBuilder : IPsiCustomCacheBuilder
  {

    private readonly IPsiSourceFile mySourceFile;

    private readonly List<ICacheItem> mySymbols = new List<ICacheItem>();

    public PsiStandardCacheBuilder(IPsiSourceFile sourceFile)
    {
      mySourceFile = sourceFile;
    }

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

    public IList<ICacheItem> Symbols
    {
      get { return mySymbols; }
    }
  }
}
