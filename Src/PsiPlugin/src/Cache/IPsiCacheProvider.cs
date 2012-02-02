using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.Cach
{
  public interface IPsiCacheProvider
  {
    /// <summary>
    /// version of this provider format (it contributes main cache version.)
    /// </summary>
    int Version { get; }

    IPsiCustomCacheBuilder CreateCustomBuilder(IPsiSourceFile sourceFile, bool isFrameworkFile);

    [CanBeNull]
    Func<IPsiSourceFile, IPersistentCacheItem> CreateItemConstructor(string guid);
  }
}
