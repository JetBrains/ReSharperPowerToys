using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace JetBrains.ReSharper.PsiPlugin.Cach
{
  [PsiComponent]
  public class PsiCache : ICache
  {
    protected readonly JetHashSet<IPsiSourceFile> myDirtyFiles = new JetHashSet<IPsiSourceFile>();

    protected readonly OneToSetMap<string, IPsiSymbol> myNameToSymbolsMap = new OneToSetMap<string, IPsiSymbol>();

    protected readonly OneToListMap<IPsiSourceFile, IPersistentCacheItem> myProjectFileToSymbolsMap = new OneToListMap<IPsiSourceFile, IPersistentCacheItem>();

    public IPsiServices PsiServices { get; private set; }
    private PsiPersistentCache myPersistentCache;

    protected readonly IShellLocks myShellLocks;
    protected readonly IEnumerable<IPsiCacheProvider> myProviders;
    private readonly IPsiConfiguration myPsiConfiguration;
    public DependencyStore DependencyStore { get; private set; }

    //private IList<IPsiSymbol> mySymbols = new List<IPsiSymbol>();

    private const int VERSION = 4;

    public PsiCache(Lifetime lifetime,
      IPsiServices psiServices,
      DependencyStore dependencyStore,
      IShellLocks shellLocks,
      CacheManager cacheManager,
      IEnumerable<IPsiCacheProvider> providers,
      IPsiConfiguration psiConfiguration)
    {
      myPsiConfiguration = psiConfiguration;
      myProviders = providers;
      DependencyStore = dependencyStore;
      PsiServices = psiServices;
      myShellLocks = shellLocks;
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
        myPersistentCache = new PsiPersistentCache(myShellLocks, Version, "PsiCache", myPsiConfiguration);

      var data = new Dictionary<IPsiSourceFile, IList<IPersistentCacheItem>>();

      if (myPersistentCache.Load(progress, persistentIdIndex,
        (file, reader) =>
        {
          using (ReadLockCookie.Create())
          {
            return PsiCacheBuilder.Read(reader, file, myProviders);
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
        if (pair.Key.IsValid() && !myDirtyFiles.Contains(pair.Key))
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
        myDirtyFiles.Add(sourceFile);
    }

    public bool UpToDate(IPsiSourceFile sourceFile)
    {
      myShellLocks.AssertReadAccessAllowed();
      if (!Accepts(sourceFile))
        return true;
      return !myDirtyFiles.Contains(sourceFile) && myProjectFileToSymbolsMap.ContainsKey(sourceFile);
    }

    public object Build(IPsiSourceFile sourceFile, bool isStartup)
    {
      var ret = PsiCacheBuilder.Build(sourceFile, false, myProviders);
      if (ret == null)
        return null;
      //mySymbols.AddRange(ret.OfType<IPsiSymbol>());
      return ret.OfType<IPersistentCacheItem>().ToList();
    }

    public void Merge(IPsiSourceFile sourceFile, object builtPart)
    {
      myShellLocks.AssertWriteAccessAllowed();

      var data = builtPart as IList<IPersistentCacheItem>;
      if (data == null)
        return;

      if (myPersistentCache != null)
        myPersistentCache.AddDataToSave(sourceFile, data);

      // clear old declarations cache...
      if (myProjectFileToSymbolsMap.ContainsKey(sourceFile))
      {
        foreach (var oldDeclaration in myProjectFileToSymbolsMap[sourceFile])
        {
          var oldName = oldDeclaration.Text;

          var symbol = oldDeclaration as IPsiSymbol;
          if (symbol != null)
            myNameToSymbolsMap.Remove(oldName, symbol);
          //var typeBinding = oldDeclaration as IPsiTypeBinding;
          //if (typeBinding != null)
            //myNameToBindingMap.Remove(oldName, typeBinding);
        }
      }
      myProjectFileToSymbolsMap.RemoveKey(sourceFile);

      // add to projectFile to data map...
      myProjectFileToSymbolsMap.AddValueRange(sourceFile, data);
      foreach (var declaration in data)
      {
        var symbol = declaration as IPsiSymbol;
        if (symbol != null)
          myNameToSymbolsMap.Add(declaration.Text, symbol);
        //var binding = declaration as IPsiTypeBinding;
        //if (binding != null)
          //myNameToBindingMap.Add(declaration.Text, binding);
      }
      myDirtyFiles.Remove(sourceFile);
    }

    public void OnFileRemoved(IPsiSourceFile sourceFile)
    {
      myShellLocks.AssertWriteAccessAllowed();

      myDirtyFiles.Remove(sourceFile);
      if (myProjectFileToSymbolsMap.ContainsKey(sourceFile))
      {
        foreach (var oldDeclaration in myProjectFileToSymbolsMap[sourceFile])
        {
          var oldName = oldDeclaration.Text;
          var symbol = oldDeclaration as IPsiSymbol;
          if (symbol != null)
            myNameToSymbolsMap.Remove(oldName, symbol);
          //var typeBinding = oldDeclaration as IPsiTypeBinding;
          //if (typeBinding != null)
            //myNameToBindingMap.Remove(oldName, typeBinding);
        }
      }
      myProjectFileToSymbolsMap.RemoveKey(sourceFile);
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
        myShellLocks.AssertWriteAccessAllowed();
        var projectFile = elementContainingChanges.GetSourceFile();
        if (projectFile != null && Accepts(projectFile))
          myDirtyFiles.Add(projectFile);
      }
    }

    public void OnDocumentChange(ProjectFileDocumentCopyChange args)
    {
      foreach (var sourceFile in args.ProjectFile.ToSourceFiles())
      {
        myShellLocks.AssertWriteAccessAllowed();
        if (Accepts(sourceFile))
        {
          myShellLocks.AssertWriteAccessAllowed();
          myDirtyFiles.Add(sourceFile);
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
      myShellLocks.AssertReadAccessAllowed();

      if (myDirtyFiles.Count > 0)
        foreach (var projectFile in new List<IPsiSourceFile>(myDirtyFiles))
          using (WriteLockCookie.Create())
          {
            var ret = PsiCacheBuilder.Build(projectFile, false, myProviders);
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
      get { return !myDirtyFiles.IsEmpty(); }
    }

    public ICollection<IPsiSymbol> GetSymbol(string name)
    {
      /*IList<IPsiSymbol> list = new List<IPsiSymbol>();
      foreach(IPsiSymbol symbol in mySymbols)
      {
        if(name.Equals(symbol.Text))
        {
          list.Add(symbol);
        }
      }
      return list;*/
      return myNameToSymbolsMap[name];
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
