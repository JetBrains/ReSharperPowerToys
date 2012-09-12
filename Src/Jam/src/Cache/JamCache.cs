using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DataFlow;
using JetBrains.DocumentManagers.impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util.Caches;
using JetBrains.Util;
using JetBrains.Util.DataStructures;
using JetBrains.Util.Special;

namespace JetBrains.ReSharper.Psi.Jam.Cache
{
  [PsiComponent]
  public sealed class JamSymbolsCache : ICache
  {
    private SymbolsPersistentCache myPersistentCache;

    private readonly object mySyncObj = new object();

    private readonly IShellLocks myShellLocks;

    private readonly IPsiConfiguration myPsiConfiguration;

    private readonly IPersistentIndexManager myPersistentIndexManager;

    private readonly JetHashSet<IPsiSourceFile> myDirtyFiles = new JetHashSet<IPsiSourceFile>();

    private readonly CompactMap<IPsiSourceFile, CompactOneToListMap<string, IJamSymbol>> mySymbolsByFile;

    public JamSymbolsCache(Lifetime lifetime, IShellLocks shellLocks, CacheManager cacheManager, IPsiConfiguration psiConfiguration, IPersistentIndexManager persistentIndexManager)
    {
      myPsiConfiguration = psiConfiguration;
      myShellLocks = shellLocks;
      myPersistentIndexManager = persistentIndexManager;

      mySymbolsByFile = new CompactMap<IPsiSourceFile, CompactOneToListMap<string, IJamSymbol>>();

      lifetime.AddBracket(() => cacheManager.RegisterCache(this), () =>
      {
        ((ICache) this).Release();
        cacheManager.UnregisterCache(this);
      });
    }

    private int Version
    {
      get { return 1; }
    }

    private string CacheDirectoryName
    {
      get { return "JamSymbols"; }
    }

    private string LoadSaveProgressText
    {
      get { return "Jam symbols"; }
    }

    public bool HasDirtyFiles
    {
      get
      {
        lock (mySyncObj)
          return myDirtyFiles.Any();
      }
    }

    public IEnumerable<string> GetAllNames()
    {
      return mySymbolsByFile.SelectMany(pair => pair.Value.Keys).Distinct(StringComparer.Ordinal);
    }

    public IEnumerable<IJamSymbol> GetSymbols(string name)
    {
      return mySymbolsByFile.SelectMany(pair => pair.Value[name]);
    }

    public void MarkAsDirty(IPsiSourceFile sourceFile)
    {
      if (IsSupport(sourceFile))
      {
        lock (mySyncObj)
          myDirtyFiles.Add(sourceFile);
      }
    }

    bool ICache.UpToDate(IPsiSourceFile sourceFile)
    {
      myShellLocks.AssertReadAccessAllowed();

      if (!IsSupport(sourceFile))
        return true;

      lock (mySyncObj)
      {
        return !myDirtyFiles.Contains(sourceFile) && mySymbolsByFile.ContainsKey(sourceFile);
      }
    }

    object ICache.Build(IPsiSourceFile sourceFile, bool isStartup)
    {
      if (!sourceFile.IsValid())
        return null;

      return sourceFile.PrimaryPsiLanguage.IsExactly<JamLanguage>() ? BuilSymbols(sourceFile) : null;
    }

    void ICache.Merge(IPsiSourceFile sourceFile, object builtPart)
    {
      myShellLocks.AssertWriteAccessAllowed();

      var data = (CompactOneToListMap<string, IJamSymbol>)builtPart;
      if (data == null)
        return;

      if (myPersistentCache != null)
        myPersistentCache.AddDataToSave(sourceFile, data);

      lock (mySyncObj)
      {
        mySymbolsByFile[sourceFile] = data;
        myDirtyFiles.Remove(sourceFile);
      }
    }

    object ICache.Build(IPsiAssembly assembly)
    {
      return null;
    }

    void ICache.Merge(IPsiAssembly assembly, object part)
    {
    }

    void ICache.OnAssemblyRemoved(IPsiAssembly assembly)
    {
    }

    void ICache.OnFileRemoved(IPsiSourceFile sourceFile)
    {
      myShellLocks.AssertWriteAccessAllowed();

      lock (mySyncObj)
      {
        mySymbolsByFile.Remove(sourceFile);
        myDirtyFiles.Remove(sourceFile);

        if (myPersistentCache != null)
          myPersistentCache.MarkDataToDelete(sourceFile);
      }
    }

    IEnumerable<IPsiSourceFile> ICache.OnProjectModelChange(ProjectModelChange change)
    {
      return EmptyList<IPsiSourceFile>.InstanceList;
    }

    IEnumerable<IPsiSourceFile> ICache.OnPsiModulePropertiesChange(IPsiModule module)
    {
      return EmptyList<IPsiSourceFile>.InstanceList;
    }

    void ICache.SyncUpdate(bool underTransaction)
    {
      myShellLocks.AssertReadAccessAllowed();

      var dirtyFiles = GetDirtyFiles();
      if (dirtyFiles.Any())
      {
        using (WriteLockCookie.Create())
        {
          foreach (var projectFile in dirtyFiles)
            ((ICache) this).Merge(projectFile, (((ICache)this).Build(projectFile, false)));
        }
      }
    }

    object ICache.Load(IProgressIndicator progress, bool enablePersistence)
    {
      if (!enablePersistence)
        return null;

      Assertion.Assert(myPersistentCache == null, "myPersistentCache == null");
      myPersistentCache = new SymbolsPersistentCache(myShellLocks, Version, CacheDirectoryName, LoadSaveProgressText, myPsiConfiguration);

      var data = new CompactMap<IPsiSourceFile, CompactOneToListMap<string, IJamSymbol>>();
      var loadResult = myPersistentCache.Load(progress, myPersistentIndexManager, ReadSymbols, (file, collection) => file.WithNotNull(f => data.Add(f, collection)));

      if (loadResult != LoadResult.OK)
      {
        ((ICache) this).Release();
        return null;
      }

      data.Compact();
      return data;
    }

    public void MergeLoaded(object data)
    {
      var parts = (CompactMap<IPsiSourceFile, CompactOneToListMap<String, IJamSymbol>>) data;

      foreach (var pair in parts)
      {
        if (pair.Key.IsValid() && !myDirtyFiles.Contains(pair.Key))
          ((ICache) this).Merge(pair.Key, pair.Value);
      }
    }

    void ICache.Save(IProgressIndicator progress, bool enablePersistence)
    {
      if (!enablePersistence)
        return;

      Assertion.Assert(myPersistentCache != null, "myPersistentCache != null");
      myPersistentCache.Save(progress, myPersistentIndexManager, WriteSymbols);
      myPersistentCache.Dispose();
      myPersistentCache = null;
    }

    void ICache.Release()
    {
      lock (mySyncObj)
      {
        myDirtyFiles.Clear();
        mySymbolsByFile.Clear();
      }
    }

    void ICache.OnSandBoxPsiChange(ITreeNode elementContainingChanges)
    {
    }

    void ICache.OnPsiChange(ITreeNode elementContainingChanges, PsiChangedElementType type)
    {
      if (elementContainingChanges != null)
      {
        myShellLocks.AssertWriteAccessAllowed();

        lock (mySyncObj)
        {
          var sourceFile = elementContainingChanges.GetSourceFile();
          if (sourceFile != null && IsSupport(sourceFile))
            myDirtyFiles.Add(sourceFile);
        }
      }
    }

    void ICache.OnDocumentChange(ProjectFileDocumentCopyChange args)
    {
      myShellLocks.AssertWriteAccessAllowed();

      lock (mySyncObj)
        myDirtyFiles.AddRange(args.ProjectFile.ToSourceFiles().Where(IsSupport));
    }

    void ICache.OnSandBoxCreated(SandBox sandBox)
    {
    }

    private ICollection<IPsiSourceFile> GetDirtyFiles()
    {
      if (myDirtyFiles.Any())
      {
        lock (mySyncObj)
        {
          if (myDirtyFiles.Any())
          {
            return myDirtyFiles.ToList();
          }
        }
      }

      return EmptyList<IPsiSourceFile>.InstanceList;
    }

    private CompactOneToListMap<string, IJamSymbol> ReadSymbols(IPsiSourceFile file, BinaryReader reader)
    {
      if (file == null)
        return null;

      using (ReadLockCookie.Create())
      {
        var symbols = new CompactOneToListMap<string, IJamSymbol>();

        var count = reader.ReadInt32();
        while (count --> 0)
        {
          var jamSymbol = ReadSymbol(file, reader);
          symbols.AddValue(jamSymbol.Name, jamSymbol);
        }

        return symbols;
      }
    }

    private void WriteSymbols(BinaryWriter writer, IPsiSourceFile file, CompactOneToListMap<string, IJamSymbol> data)
    {
      myShellLocks.AssertReadAccessAllowed();

      var jamSymbols = data.AllValues;

      writer.Write(jamSymbols.Count);

      foreach (var jamSymbol in jamSymbols)
      {
        writer.Write((byte) jamSymbol.SymbolType);
        writer.WriteCompressedInt32(jamSymbol.Offset);
        writer.Write(jamSymbol.Name);
      }
    }

    private IJamSymbol ReadSymbol(IPsiSourceFile file, BinaryReader reader)
    {
      return new JamSymbol((JamSymbolType) reader.ReadByte(), reader.ReadCompressedInt32(), reader.ReadString(), file);
    }

    private CompactOneToListMap<string, IJamSymbol> BuilSymbols(IPsiSourceFile sourceFile)
    {
      var jamFile = sourceFile.GetNonInjectedPsiFile<JamLanguage>() as IJamFile;
      Assertion.AssertNotNull(jamFile, "jamFile is null");

      return JamSymbolsBuilder.Build(jamFile);
    }

    private static bool IsSupport([NotNull] IPsiSourceFile sourceFile)
    {
      return sourceFile.PrimaryPsiLanguage.IsExactly<JamLanguage>();
    }

    private class SymbolsPersistentCache : SimplePersistentCache<CompactOneToListMap<string, IJamSymbol>>
    {
      private readonly string myProgressText;

      public SymbolsPersistentCache(IShellLocks locks, int version, string directoryName, string progressText, IPsiConfiguration psiConfiguration) : base(locks, version, directoryName, psiConfiguration)
      {
        myProgressText = progressText;
      }

      protected override string LoadSaveProgressText
      {
        get { return myProgressText; }
      }
    }

    public IEnumerable<string> GetNames(JamSymbolType jamSymbolType)
    {
      lock (mySyncObj)
      {
        return new HashSet<string>(EnumerableNames(jamSymbolType), StringComparer.Ordinal);
      }
    }

    public IEnumerable<IJamSymbol> GetSymbols(JamSymbolType jamSymbolType)
    {
      return mySymbolsByFile.Values.SelectMany(map => map.AllValues).Where(symbol => symbol.SymbolType == jamSymbolType);
    }

    private IEnumerable<string> EnumerableNames(JamSymbolType jamSymbolType)
    {
      foreach (var map in mySymbolsByFile.Values)
      {
        foreach (var pair in map)
        {
          if (pair.Value.Any(symbol => symbol.SymbolType == jamSymbolType))
            yield return pair.Key;
        }
      }
    }
  }
}