using System.Collections.Generic;
using System.Linq;
using JetBrains.Application; 
using JetBrains.Application.Progress;
using JetBrains.DataFlow;
using JetBrains.DocumentManagers.impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util.Caches;
using JetBrains.ReSharper.PsiPlugin.PsiGrammar;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Cache
{
  [PsiComponent]
  public class PsiCache : ICache
  {
    private const int VERSION = 8;
    private readonly JetHashSet<IPsiSourceFile> myDirtyFiles = new JetHashSet<IPsiSourceFile>();
    private readonly OneToSetMap<string, PsiOptionSymbol> myNameToSymbolsOptionMap = new OneToSetMap<string, PsiOptionSymbol>();

    private readonly OneToSetMap<string, IPsiSymbol> myNameToSymbolsRuleMap = new OneToSetMap<string, IPsiSymbol>();
    private readonly IPersistentIndexManager myPersistentIdIndex;
    private readonly OneToListMap<IPsiSourceFile, PsiOptionSymbol> myProjectFileToSymbolsOptionMap = new OneToListMap<IPsiSourceFile, PsiOptionSymbol>();
    private readonly OneToListMap<IPsiSourceFile, PsiRuleSymbol> myProjectFileToSymbolsRuleMap = new OneToListMap<IPsiSourceFile, PsiRuleSymbol>();
    private readonly IPsiConfiguration myPsiConfiguration;
    private readonly IShellLocks myShellLocks;
    private PsiPersistentCache<CachePair> myPersistentCache;

    public PsiCache(Lifetime lifetime,
      IPsiServices psiServices,
      IShellLocks shellLocks,
      CacheManager cacheManager,
      IPsiConfiguration psiConfiguration, IPersistentIndexManager persistentIdIndex)
    {
      myPsiConfiguration = psiConfiguration;
      myPersistentIdIndex = persistentIdIndex;
      myShellLocks = shellLocks;
      //using (ReadLockCookie.Create())
      //{
        lifetime.AddBracket(() => cacheManager.RegisterCache(this), () => cacheManager.UnregisterCache(this));
      //}
    }

    public object Load(IProgressIndicator progress, bool enablePersistence)
    {
      if (!enablePersistence)
      {
        return null;
      }

      Assertion.Assert(myPersistentCache == null, "myPersistentCache == null");

      using (ReadLockCookie.Create())
      {
        myPersistentCache = new PsiPersistentCache<CachePair>(myShellLocks, VERSION, "PsiCache", myPsiConfiguration);
      }

      var data = new Dictionary<IPsiSourceFile, CachePair>();

      if (myPersistentCache.Load(progress, myPersistentIdIndex,
        (file, reader) =>
        {
          using (ReadLockCookie.Create())
          {
            return PsiCacheBuilder.Read(reader, file);
          }
        },
        (projectFile, psiSymbols) =>
        {
          if (projectFile != null)
          {
            data[projectFile] = psiSymbols;
          }
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
      var parts = (Dictionary<IPsiSourceFile, CachePair>)data;
      foreach (var pair in parts)
      {
        if (pair.Key.IsValid() && !myDirtyFiles.Contains(pair.Key))
        {
          ((ICache)this).Merge(pair.Key, pair.Value);
        }
      }
    }

    public void Save(IProgressIndicator progress, bool enablePersistence)
    {
      if (!enablePersistence)
      {
        return;
      }

      Assertion.Assert(myPersistentCache != null, "myPersistentCache != null");
      myPersistentCache.Save(progress, myPersistentIdIndex, (writer, file, data) =>
        PsiCacheBuilder.Write(data, writer));
      myPersistentCache.Dispose();
      myPersistentCache = null;
    }

    public void MarkAsDirty(IPsiSourceFile sourceFile)
    {
      if (Accepts(sourceFile))
      {
        myDirtyFiles.Add(sourceFile);
      }
    }

    public bool UpToDate(IPsiSourceFile sourceFile)
    {
      myShellLocks.AssertReadAccessAllowed();

      if (!Accepts(sourceFile))
      {
        return true;
      }
      return !myDirtyFiles.Contains(sourceFile) && myProjectFileToSymbolsRuleMap.ContainsKey(sourceFile) && myProjectFileToSymbolsOptionMap.ContainsKey(sourceFile);
    }

    public object Build(IPsiSourceFile sourceFile, bool isStartup)
    {
      return PsiCacheBuilder.Build(sourceFile);
    }

    public void Merge(IPsiSourceFile sourceFile, object builtPart)
    {
      myShellLocks.AssertWriteAccessAllowed();

      var data = builtPart as IList<IPsiSymbol>;

      if (data != null)
      {
        var ruleData = new List<PsiRuleSymbol>();
        var optionData = new List<PsiOptionSymbol>();
        foreach (IPsiSymbol symbol in data)
        {
          var psiRuleSymbol = symbol as PsiRuleSymbol;
          if (psiRuleSymbol != null)
          {
            ruleData.Add(psiRuleSymbol);
          }
          var psiOptionSymbol = symbol as PsiOptionSymbol;
          if (psiOptionSymbol != null)
          {
            optionData.Add(psiOptionSymbol);
          }
        }
        if (myPersistentCache != null)
        {
          myPersistentCache.AddDataToSave(sourceFile, new CachePair(ruleData, optionData));
        }

        // clear old declarations cache...
        //rules
        if (myProjectFileToSymbolsRuleMap.ContainsKey(sourceFile))
        {
          foreach (PsiRuleSymbol oldDeclaration in myProjectFileToSymbolsRuleMap[sourceFile])
          {
            string oldName = oldDeclaration.Name;
            myNameToSymbolsRuleMap.Remove(oldName, oldDeclaration);
          }
        }

        //option
        if (myProjectFileToSymbolsOptionMap.ContainsKey(sourceFile))
        {
          foreach (PsiOptionSymbol oldDeclaration in myProjectFileToSymbolsOptionMap[sourceFile])
          {
            string oldName = oldDeclaration.Name;
            myNameToSymbolsOptionMap.Remove(oldName, oldDeclaration);
          }
        }

        //myProjectFileToSymbolsOptionMap.AddValueRange(sourceFile, optionData);
        myDirtyFiles.Remove(sourceFile);

        myProjectFileToSymbolsRuleMap.RemoveKey(sourceFile);
        myProjectFileToSymbolsOptionMap.RemoveKey(sourceFile);

        // add to projectFile to data map...
        myProjectFileToSymbolsRuleMap.AddValueRange(sourceFile, ruleData);
        myProjectFileToSymbolsOptionMap.AddValueRange(sourceFile, optionData);
        foreach (PsiRuleSymbol declaration in ruleData)
        {
          myNameToSymbolsRuleMap.Add(declaration.Name, declaration);
        }
        foreach (PsiOptionSymbol declaration in optionData)
        {
          myNameToSymbolsOptionMap.Add(declaration.Name, declaration);
        }
      }
    }

    public void OnFileRemoved(IPsiSourceFile sourceFile)
    {
      myShellLocks.AssertWriteAccessAllowed();

      myDirtyFiles.Remove(sourceFile);
      if (myProjectFileToSymbolsRuleMap.ContainsKey(sourceFile))
      {
        foreach (PsiRuleSymbol oldDeclaration in myProjectFileToSymbolsRuleMap[sourceFile])
        {
          string oldName = oldDeclaration.Name;
          myNameToSymbolsRuleMap.Remove(oldName, oldDeclaration);
        }
      }
      if (myProjectFileToSymbolsOptionMap.ContainsKey(sourceFile))
      {
        foreach (PsiOptionSymbol oldDeclaration in myProjectFileToSymbolsOptionMap[sourceFile])
        {
          string oldName = oldDeclaration.Name;
          myNameToSymbolsOptionMap.Remove(oldName, oldDeclaration);
        }
      }
      myProjectFileToSymbolsRuleMap.RemoveKey(sourceFile);
      myProjectFileToSymbolsOptionMap.RemoveKey(sourceFile);
      if (myPersistentCache != null)
      {
        myPersistentCache.MarkDataToDelete(sourceFile);
      }
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
        IPsiSourceFile projectFile = elementContainingChanges.GetSourceFile();
        if (projectFile != null && Accepts(projectFile))
        {
          myDirtyFiles.Add(projectFile);
        }
      }
    }

    public void OnDocumentChange(ProjectFileDocumentCopyChange args)
    {
      foreach (IPsiSourceFile sourceFile in args.ProjectFile.ToSourceFiles())
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
      {
        foreach (IPsiSourceFile projectFile in new List<IPsiSourceFile>(myDirtyFiles))
        {
          using (WriteLockCookie.Create())
          {
            ICollection<IPsiSymbol> ret = PsiCacheBuilder.Build(projectFile);
            if (ret != null)
            {
              ((ICache)this).Merge(projectFile, ret.ToList());
            }
            else
            {
              ((ICache)this).Merge(projectFile, null);
            }
          }
        }
      }
    }

    public void Release()
    {
    }

    public bool HasDirtyFiles
    {
      get { return !myDirtyFiles.IsEmpty(); }
    }

    private static bool Accepts(IPsiSourceFile sourceFile)
    {
      return sourceFile.GetAllPossiblePsiLanguages().Any(x => x.Is<PsiLanguage>());
    }

    public IEnumerable<IPsiSymbol> GetSymbols(string name)
    {
      return myNameToSymbolsRuleMap[name];
    }

    public IEnumerable<PsiOptionSymbol> GetOptionSymbols(string name)
    {
      return myNameToSymbolsOptionMap[name];
    }

    public IEnumerable<IPsiSymbol> GetSymbolsDeclaredInFile(IPsiSourceFile sourceFile)
    {
      var symbols = myProjectFileToSymbolsRuleMap.GetValuesCollection(sourceFile);
      return symbols;
    }

    #region Nested type: PsiPersistentCache

    private class PsiPersistentCache<T> : SimplePersistentCache<T>
    {
      public PsiPersistentCache(IShellLocks locks, int formatVersion, string cacheDirectoryName, IPsiConfiguration psiConfiguration)
        : base(locks, formatVersion, cacheDirectoryName, psiConfiguration)
      {
      }

      protected override string LoadSaveProgressText
      {
        get { return "Psi Caches"; }
      }
    }

    #endregion
  }

  public class CachePair
  {
    private readonly IList<PsiOptionSymbol> myOptions;
    private readonly IList<PsiRuleSymbol> myRules;

    public CachePair(IList<PsiRuleSymbol> rules, IList<PsiOptionSymbol> options)
    {
      myRules = rules;
      myOptions = options;
    }

    public IList<PsiRuleSymbol> Rules
    {
      get { return myRules; }
    }

    public IList<PsiOptionSymbol> Options
    {
      get { return myOptions; }
    }
  }
}
