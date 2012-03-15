using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.Cache
{
  public interface IPsiCacheProvider
  {
    IPsiCustomCacheBuilder CreateCustomBuilder(IPsiSourceFile sourceFile);

    int Version { get; }

    [CanBeNull]
    Func<IPsiSourceFile, IPersistentCacheItem> CreateItemConstructor();
  }
}
