using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DataFlow;
using JetBrains.DocumentManagers.impl;
using JetBrains.ReSharper.LexPlugin.Grammar;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util.Caches;
using JetBrains.Util;

namespace JetBrains.ReSharper.LexPlugin.Cache
{
  [PsiComponent]
  public class LexCache : ICache
  {
    private const int VERSION = 8;
    private readonly JetHashSet<IPsiSourceFile> myDirtyFiles = new JetHashSet<IPsiSourceFile>();
    private readonly OneToSetMap<string, LexStateSymbol> myNameToSymbolsStateMap = new OneToSetMap<string, LexStateSymbol>();

    private readonly OneToSetMap<string, ILexSymbol> myNameToSymbolsTokenMap = new OneToSetMap<string, ILexSymbol>();
    private readonly OneToSetMap<string, LexIncludeFileSymbol> myNameToSymbolsIncludeFileMap = new OneToSetMap<string, LexIncludeFileSymbol>();
    private readonly IPersistentIndexManager myPersistentIdIndex;
    private readonly OneToListMap<IPsiSourceFile, LexStateSymbol> myProjectFileToSymbolsStateMap = new OneToListMap<IPsiSourceFile, LexStateSymbol>();
    private readonly OneToListMap<IPsiSourceFile, LexTokenSymbol> myProjectFileToSymbolsTokenMap = new OneToListMap<IPsiSourceFile, LexTokenSymbol>();
    private readonly OneToListMap<IPsiSourceFile, LexIncludeFileSymbol> myProjectFileToSymbolsIncludeFileMap = new OneToListMap<IPsiSourceFile, LexIncludeFileSymbol>();
    private readonly IPsiConfiguration myPsiConfiguration;
    private readonly IShellLocks myShellLocks;
    private LexPersistentCache<CacheData> myPersistentCache;

    public LexCache(Lifetime lifetime,
      IShellLocks shellLocks,
      IPsiConfiguration psiConfiguration, IPersistentIndexManager persistentIdIndex)
    {
      myPsiConfiguration = psiConfiguration;
      myPersistentIdIndex = persistentIdIndex;
      myShellLocks = shellLocks;
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
        myPersistentCache = new LexPersistentCache<CacheData>(myShellLocks, VERSION, "LexCache", myPsiConfiguration);
      }

      var data = new Dictionary<IPsiSourceFile, CacheData>();

      if (myPersistentCache.Load(progress, myPersistentIdIndex,
        (file, reader) =>
        {
          using (ReadLockCookie.Create())
          {
            return LexCacheBuilder.Read(reader, file);
          }
        },
        (projectFile, psiSymbols) =>
        {
          if (projectFile != null)
          {
            data[projectFile] = psiSymbols;
          }
        }) != LoadResult.OK)

        return data;
      return null;
    }

    public void MergeLoaded(object data)
    {
      var parts = (Dictionary<IPsiSourceFile, CacheData>)data;
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
        LexCacheBuilder.Write(data, writer));
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
      return !myDirtyFiles.Contains(sourceFile) && myProjectFileToSymbolsTokenMap.ContainsKey(sourceFile) && myProjectFileToSymbolsStateMap.ContainsKey(sourceFile);
    }

    public object Build(IPsiSourceFile sourceFile, bool isStartup)
    {
      return LexCacheBuilder.Build(sourceFile);
    }

    public void Merge(IPsiSourceFile sourceFile, object builtPart)
    {
      myShellLocks.AssertWriteAccessAllowed();

      var data = builtPart as IList<ILexSymbol>;

      if (data != null)
      {
        var ruleData = new List<LexTokenSymbol>();
        var optionData = new List<LexStateSymbol>();
        var includeFileData = new List<LexIncludeFileSymbol>();
        foreach (ILexSymbol symbol in data)
        {
          var lexTokenSymbol = symbol as LexTokenSymbol;
          if (lexTokenSymbol != null)
          {
            ruleData.Add(lexTokenSymbol);
          }
          var lexStateSymbol = symbol as LexStateSymbol;
          if (lexStateSymbol != null)
          {
            optionData.Add(lexStateSymbol);
          }
          var includeFileSymbol = symbol as LexIncludeFileSymbol;
          if(includeFileSymbol != null)
          {
            includeFileData.Add(includeFileSymbol);
          }
        }
        if (myPersistentCache != null)
        {
          myPersistentCache.AddDataToSave(sourceFile, new CacheData(ruleData, optionData, includeFileData));
        }

        // clear old declarations cache...
        //tokens
        if (myProjectFileToSymbolsTokenMap.ContainsKey(sourceFile))
        {
          foreach (LexTokenSymbol oldDeclaration in myProjectFileToSymbolsTokenMap[sourceFile])
          {
            string oldName = oldDeclaration.Name;
            myNameToSymbolsTokenMap.Remove(oldName, oldDeclaration);
          }
        }

        //option
        if (myProjectFileToSymbolsStateMap.ContainsKey(sourceFile))
        {
          foreach (LexStateSymbol oldDeclaration in myProjectFileToSymbolsStateMap[sourceFile])
          {
            string oldName = oldDeclaration.Name;
            myNameToSymbolsStateMap.Remove(oldName, oldDeclaration);
          }
        }

        if (myProjectFileToSymbolsIncludeFileMap.ContainsKey(sourceFile))
        {
          foreach (LexIncludeFileSymbol oldDeclaration in myProjectFileToSymbolsIncludeFileMap[sourceFile])
          {
            string oldName = oldDeclaration.Name;
            myNameToSymbolsIncludeFileMap.Remove(oldName, oldDeclaration);
          }
        }

        //myProjectFileToSymbolsStateMap.AddValueRange(sourceFile, optionData);
        myDirtyFiles.Remove(sourceFile);

        myProjectFileToSymbolsTokenMap.RemoveKey(sourceFile);
        myProjectFileToSymbolsStateMap.RemoveKey(sourceFile);
        myProjectFileToSymbolsIncludeFileMap.RemoveKey(sourceFile);

        // add to projectFile to data map...
        myProjectFileToSymbolsTokenMap.AddValueRange(sourceFile, ruleData);
        myProjectFileToSymbolsStateMap.AddValueRange(sourceFile, optionData);
        myProjectFileToSymbolsIncludeFileMap.AddValueRange(sourceFile, includeFileData);
        foreach (LexTokenSymbol declaration in ruleData)
        {
          myNameToSymbolsTokenMap.Add(declaration.Name, declaration);
        }
        foreach (LexStateSymbol declaration in optionData)
        {
          myNameToSymbolsStateMap.Add(declaration.Name, declaration);
        }
        foreach (LexIncludeFileSymbol declaration in includeFileData)
        {
          myNameToSymbolsIncludeFileMap.Add(declaration.Name, declaration);
        }
      }
    }

    public void Drop(IPsiSourceFile sourceFile)
    {
      myShellLocks.AssertWriteAccessAllowed();

      myDirtyFiles.Remove(sourceFile);
      if (myProjectFileToSymbolsTokenMap.ContainsKey(sourceFile))
      {
        foreach (LexTokenSymbol oldDeclaration in myProjectFileToSymbolsTokenMap[sourceFile])
        {
          string oldName = oldDeclaration.Name;
          myNameToSymbolsTokenMap.Remove(oldName, oldDeclaration);
        }
      }
      if (myProjectFileToSymbolsStateMap.ContainsKey(sourceFile))
      {
        foreach (LexStateSymbol oldDeclaration in myProjectFileToSymbolsStateMap[sourceFile])
        {
          string oldName = oldDeclaration.Name;
          myNameToSymbolsStateMap.Remove(oldName, oldDeclaration);
        }
      }
      if (myProjectFileToSymbolsIncludeFileMap.ContainsKey(sourceFile))
      {
        foreach (LexIncludeFileSymbol oldDeclaration in myProjectFileToSymbolsIncludeFileMap[sourceFile])
        {
          string oldName = oldDeclaration.Name;
          myNameToSymbolsIncludeFileMap.Remove(oldName, oldDeclaration);
        }
      }
      myProjectFileToSymbolsTokenMap.RemoveKey(sourceFile);
      myProjectFileToSymbolsIncludeFileMap.RemoveKey(sourceFile);
      if (myPersistentCache != null)
      {
        myPersistentCache.MarkDataToDelete(sourceFile);
      }
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

    public void OnDocumentChange(IPsiSourceFile sourceFile, ProjectFileDocumentCopyChange change)
    {
      myShellLocks.AssertWriteAccessAllowed();
      if (Accepts(sourceFile))
      {
        myShellLocks.AssertWriteAccessAllowed();
        myDirtyFiles.Add(sourceFile);
      }
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
            ICollection<ILexSymbol> ret = LexCacheBuilder.Build(projectFile);
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

    public bool HasDirtyFiles
    {
      get { return !myDirtyFiles.IsEmpty(); }
    }

    private static bool Accepts(IPsiSourceFile sourceFile)
    {
      return sourceFile.IsLanguageSupported<LexLanguage>();
    }

    public IEnumerable<ILexSymbol> GetTokenSymbols(string name)
    {
      return myNameToSymbolsTokenMap[name];
    }

    public IEnumerable<ILexSymbol> GetAllTokenSymbols()
    {
      return myNameToSymbolsTokenMap.Values;
    }

    public IEnumerable<LexStateSymbol> GetStateSymbols(string name)
    {
      return myNameToSymbolsStateMap[name];
    }

    public IEnumerable<LexStateSymbol> GetAllStateSymbols()
    {
      return myNameToSymbolsStateMap.Values;
    }

    public IEnumerable<LexIncludeFileSymbol> GetIncludeFileSymbols(string name)
    {
      return myNameToSymbolsIncludeFileMap[name];
    }

    public IEnumerable<LexIncludeFileSymbol> GetAllIncludeFileSymbols()
    {
      return myNameToSymbolsIncludeFileMap.Values;
    }

    public IEnumerable<IPsiSourceFile> GetAllFiles()
    {
      return myProjectFileToSymbolsIncludeFileMap.Keys;
    }

    public IEnumerable<ILexSymbol> GetAllSymbols()
    {
      List<ILexSymbol> symbols = new List<ILexSymbol>();
      symbols.AddRange(myNameToSymbolsTokenMap.Values);
      symbols.AddRange(myNameToSymbolsStateMap.Values);
      symbols.AddRange(myNameToSymbolsIncludeFileMap.Values);
      return symbols;
    } 

    public IEnumerable<LexTokenSymbol> GetTokenSymbolsDeclaredInFile(IPsiSourceFile sourceFile)
    {
      var symbols = myProjectFileToSymbolsTokenMap.GetValuesCollection(sourceFile);
      return symbols;
    }

    public IEnumerable<LexStateSymbol> GetStateSymbolsDeclaredInFile(IPsiSourceFile sourceFile)
    {
      var symbols = myProjectFileToSymbolsStateMap.GetValuesCollection(sourceFile);
      return symbols;
    }

    public IEnumerable<LexIncludeFileSymbol> GetIncludeFileSymbolsDeclaredInFile(IPsiSourceFile sourceFile)
    {
      var symbols = myProjectFileToSymbolsIncludeFileMap.GetValuesCollection(sourceFile);
      return symbols;
    }

    #region Nested type: LexPersistentCache

    private class LexPersistentCache<T> : SimplePersistentCache<T>
    {
      public LexPersistentCache(IShellLocks locks, int formatVersion, string cacheDirectoryName, IPsiConfiguration psiConfiguration)
        : base(locks, formatVersion, cacheDirectoryName, psiConfiguration)
      {
      }

      protected override string LoadSaveProgressText
      {
        get { return "Lex Caches"; }
      }
    }

    #endregion
  }

  public class CacheData
  {
    private readonly IList<LexStateSymbol> myStates;
    private readonly IList<LexTokenSymbol> myTokens;
    private readonly IList<LexIncludeFileSymbol> myIncludeFiles;

    public CacheData(IList<LexTokenSymbol> tokens, IList<LexStateSymbol> states, IList<LexIncludeFileSymbol> includeFiles )
    {
      myTokens = tokens;
      myStates = states;
      myIncludeFiles = includeFiles;
    }

    public IList<LexTokenSymbol> Tokens
    {
      get { return myTokens; }
    }

    public IList<LexStateSymbol> States
    {
      get { return myStates; }
    }

    public IList<LexIncludeFileSymbol> IncludeFiles
    {
      get { return myIncludeFiles; }
    }
  }
}
