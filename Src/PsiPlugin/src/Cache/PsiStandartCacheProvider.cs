using System;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.Cache
{
  [SolutionComponent]
  internal class PsiStandartCacheProvider : IPsiCacheProvider
  {
    public int Version
    {
      get
      {
        return 8;
      }
    }

    public IPsiCustomCacheBuilder CreateCustomBuilder(IPsiSourceFile sourceFile)
    {
      return new PsiStandardCacheBuilder();
    }

    public Func<IPsiSourceFile, IPersistentCacheItem> CreateItemConstructor()
    {
      return null;
    }
  }
}
