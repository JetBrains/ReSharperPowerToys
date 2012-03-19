using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DataFlow;
using JetBrains.DocumentManagers.impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Dependencies;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util.Caches;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Cache
{
  [PsiComponent]
  public class PsiCache : ICache
  {
    protected readonly JetHashSet<IPsiSourceFile> DirtyFiles = new JetHashSet<IPsiSourceFile>();

    protected readonly OneToSetMap<string, IPsiSymbol> NameToSymbolsMap = new OneToSetMap<string, IPsiSymbol>();

    protected readonly OneToListMap<IPsiSourceFile, IPersistentCacheItem> ProjectFileToSymbolsMap = new OneToListMap<IPsiSourceFile, IPersistentCacheItem>();

    public IPsiServices PsiServices { get; private set; }
    private PsiPersistentCache myPersistentCache;

    protected readonly IShellLocks ShellLocks;
    protected readonly IEnumerable<IPsiCacheProvider> Providers;
    private readonly IPsiConfiguration myPsiConfiguration;
    public DependencyStore DependencyStore { get; private set; }

    private const int VERSION = 7;

    public PsiCache(Lifetime lifetime,
      IPsiServices psiServices,
      DependencyStore dependencyStore,
      IShellLocks shellLocks,
      CacheManager cacheManager,
      IEnumerable<IPsiCacheProvider> providers,
      IPsiConfiguration psiConfiguration)
    {
      myPsiConfiguration = psiConfiguration;
      Providers = providers;
      DependencyStore = dependencyStore;
      PsiServices = psiServices;
      ShellLocks = shellLocks;
      lifetime.AddBracket(() => cacheManager.RegisterCache(this), () => cacheManager.UnregisterCache(this));
    }

    protected int Version
    {
      get { return VERSION; }
    }

    private static bool Accepts(IPsiSourceFile sourceFile)
    {
      return sourceFile.GetAllPossiblePsiLanguages().Any(x => x.Is<PsiLanguage>());
    }

    public object Load(IProgressIndicator progress, bool enablePersistence, PersistentIdIndex persistentIdIndex)
    {
      if (!enablePersistence)
        return null;

      Assertion.Assert(myPersistentCache == null, "myPersistentCache == null");

      using (ReadLockCookie.Create())
        myPersistentCache = new PsiPersistentCache(ShellLocks, Version, "PsiCache", myPsiConfiguration);

      var data = new Dictionary<IPsiSourceFile, IList<IPersistentCacheItem>>();

      if (myPersistentCache.Load(progress, persistentIdIndex,
        (file, reader) =>
        {
          using (ReadLockCookie.Create())
          {
            return PsiCacheBuilder.Read(reader, file, Providers);
          }
        },
        (projectFile, javaScriptCacheData) =>
        {
          if (projectFile != null)
            data[projectFile] = javaScriptCacheData;
        }) != LoadResult.OK)
      {
        // clear all...
        ((ICache)this).Release();
        return null;
      }

      return data;
    }

    public void MergeLoaded(object data)
    {
      var parts = (Dictionary<IPsiSourceFile, IList<IPersistentCacheItem>>)data;
      foreach (var pair in parts)
      {
        if (pair.Key.IsValid() && !DirtyFiles.Contains(pair.Key))
          ((ICache)this).Merge(pair.Key, pair.Value);
      }
    }

    public void Save(IProgressIndicator progress, bool enablePersistence, PersistentIdIndex persistentIdIndex)
    {
      if (!enablePersistence)
        return;

      Assertion.Assert(myPersistentCache != null, "myPersistentCache != null");
      myPersistentCache.Save(progress, persistentIdIndex, (writer, file, data) => PsiCacheBuilder.Write(data, writer));
      myPersistentCache.Dispose();
      myPersistentCache = null;
    }

    public void MarkAsDirty(IPsiSourceFile sourceFile)
    {
      if (Accepts(sourceFile))
        DirtyFiles.Add(sourceFile);
    }

    public bool UpToDate(IPsiSourceFile sourceFile)
    {
      ShellLocks.AssertReadAccessAllowed();
      if (!Accepts(sourceFile))
        return true;
      return !DirtyFiles.Contains(sourceFile) && ProjectFileToSymbolsMap.ContainsKey(sourceFile);
    }

    public object Build(IPsiSourceFile sourceFile, bool isStartup)
    {
      var ret = PsiCacheBuilder.Build(sourceFile, Providers);
      if (ret == null)
        return null;
      return ret.OfType<IPersistentCacheItem>().ToList();
    }

    public void Merge(IPsiSourceFile sourceFile, object builtPart)
    {
      ShellLocks.AssertWriteAccessAllowed();

      var data = builtPart as IList<IPersistentCacheItem>;
      if (data == null)
        return;

      if (myPersistentCache != null)
        myPersistentCache.AddDataToSave(sourceFile, data);

      // clear old declarations cache...
      if (ProjectFileToSymbolsMap.ContainsKey(sourceFile))
      {
        foreach (var oldDeclaration in ProjectFileToSymbolsMap[sourceFile])
        {
          var oldName = oldDeclaration.Name;

          var symbol = oldDeclaration as IPsiSymbol;
          if (symbol != null)
            NameToSymbolsMap.Remove(oldName, symbol);
        }
      }
      ProjectFileToSymbolsMap.RemoveKey(sourceFile);

      // add to projectFile to data map...
      ProjectFileToSymbolsMap.AddValueRange(sourceFile, data);
      foreach (var declaration in data)
      {
        var symbol = declaration as IPsiSymbol;
        if (symbol != null)
          NameToSymbolsMap.Add(declaration.Name, symbol);
      }
      DirtyFiles.Remove(sourceFile);
    }

    public void OnFileRemoved(IPsiSourceFile sourceFile)
    {
      ShellLocks.AssertWriteAccessAllowed();

      DirtyFiles.Remove(sourceFile);
      if (ProjectFileToSymbolsMap.ContainsKey(sourceFile))
      {
        foreach (var oldDeclaration in ProjectFileToSymbolsMap[sourceFile])
        {
          var oldName = oldDeclaration.Name;
          var symbol = oldDeclaration as IPsiSymbol;
          if (symbol != null)
            NameToSymbolsMap.Remove(oldName, symbol);
        }
      }
      ProjectFileToSymbolsMap.RemoveKey(sourceFile);
      if (myPersistentCache != null)
        myPersistentCache.MarkDataToDelete(sourceFile);
    }

    public object Build(IPsiAssembly assembly)
    {
      return null;
    }

    public void Merge(IPsiAssembly assembly, object part)
    {
    }

    public void OnAssemblyRemoved(IPsiAssembly assembly)
    {
    }

    public void OnSandBoxCreated(SandBox sandBox)
    {
    }

    public void OnSandBoxPsiChange(ITreeNode elementContainingChanges)
    {
    }

    public void OnPsiChange(ITreeNode elementContainingChanges, PsiChangedElementType type)
    {
      if (elementContainingChanges != null)
      {
        ShellLocks.AssertWriteAccessAllowed();
        var projectFile = elementContainingChanges.GetSourceFile();
        if (projectFile != null && Accepts(projectFile))
          DirtyFiles.Add(projectFile);
      }
    }

    public void OnDocumentChange(ProjectFileDocumentCopyChange args)
    {
      foreach (var sourceFile in args.ProjectFile.ToSourceFiles())
      {
        ShellLocks.AssertWriteAccessAllowed();
        if (Accepts(sourceFile))
        {
          ShellLocks.AssertWriteAccessAllowed();
          DirtyFiles.Add(sourceFile);
        }
      }
    }

    public IEnumerable<IPsiSourceFile> OnProjectModelChange(ProjectModelChange change)
    {
      return EmptyList<IPsiSourceFile>.InstanceList;
    }

    public IEnumerable<IPsiSourceFile> OnPsiModulePropertiesChange(IPsiModule module)
    {
      return EmptyList<IPsiSourceFile>.InstanceList;
    }

    public void SyncUpdate(bool underTransaction)
    {
      ShellLocks.AssertReadAccessAllowed();

      if (DirtyFiles.Count > 0)
        foreach (var projectFile in new List<IPsiSourceFile>(DirtyFiles))
          using (WriteLockCookie.Create())
          {
            var ret = PsiCacheBuilder.Build(projectFile, Providers);
            //mySymbols.AddRange(ret.OfType<IPsiSymbol>());
            if (ret != null)
              ((ICache)this).Merge(projectFile, ret.OfType<IPersistentCacheItem>().ToList());
            else
              ((ICache)this).Merge(projectFile, null);
          }
    }

    public void Release()
    {
    }

    public bool HasDirtyFiles
    {
      get { return !DirtyFiles.IsEmpty(); }
    }

    public ICollection<IPsiSymbol> GetSymbol(string name)
    {
      return NameToSymbolsMap[name];
    }

    private class PsiPersistentCache : SimplePersistentCache<IList<IPersistentCacheItem>>
    {
      public PsiPersistentCache(IShellLocks locks, int formatVersion, string cacheDirectoryName, IPsiConfiguration psiConfiguration)
        : base(locks, formatVersion, cacheDirectoryName, psiConfiguration) { }

      protected override string LoadSaveProgressText
      {
        get { return "Psi Caches"; }
      }
    }
  }
}
