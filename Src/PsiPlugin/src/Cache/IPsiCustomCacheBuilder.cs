using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Cach
{
  public interface IPsiCustomCacheBuilder
  {
    Action ScanBeforeChildren(ITreeNode node);
    IList<ICacheItem> Symbols { get; }
  }
}
