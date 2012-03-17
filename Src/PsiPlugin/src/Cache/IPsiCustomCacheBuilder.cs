using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Cache
{
  public interface IPsiCustomCacheBuilder
  {
    Action ScanBeforeChildren(ITreeNode node);
    IEnumerable<ICacheItem> Symbols { get; }
  }
}
