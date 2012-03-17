using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Cache
{
  internal class PsiCacheBuilder
  {
    private readonly IPsiCustomCacheBuilder[] myBuilders;

    private PsiCacheBuilder(IPsiSourceFile sourceFile, IEnumerable<IPsiCacheProvider> providers)
    {
      myBuilders = providers.Select(x => x.CreateCustomBuilder(sourceFile)).ToArray();
    }

    private IEnumerable<ICacheItem> GetSymbols()
    {
      return myBuilders.SelectMany(x => x.Symbols).ToList();
    }

    public static IList<IPersistentCacheItem> Read(BinaryReader reader, IPsiSourceFile sourceFile, IEnumerable<IPsiCacheProvider> providers)
    {
      var groupsCount = reader.ReadUInt32();
      var ret = new List<IPersistentCacheItem>();
      for (var i = 0; i < groupsCount; i++)
      {
        var guid = reader.ReadString();
        var count = reader.ReadInt32();
        Func<IPsiSourceFile, IPersistentCacheItem> constructor = null;
        foreach (var provider in providers)
        {
          constructor = provider.CreateItemConstructor();
          if (constructor != null)
            break;
        }

        Assertion.Assert(constructor != null, string.Format("Can not create item constructor with guid '{0}'", guid));

        if(constructor == null)
        {
          return ret;
        }

        for (int j = 0; j < count; j++)
        {
          var item = constructor(sourceFile);
          if(item != null)
          {
            item.Read(reader);
            ret.Add(item);
          }
        }
      }
      return ret;
    }

    public static void Write(IEnumerable<IPersistentCacheItem> symbolsOfProjectFile, BinaryWriter writer)
    {
      var groups = new OneToListMap<string, IPersistentCacheItem>();

      // prepare groups...
      foreach (var item in symbolsOfProjectFile)
        groups.Add(item.SymbolTypeGuid, item);

      writer.Write(groups.Keys.Count);

      foreach (var pair in groups)
      {
        var giud = pair.Key;
        var items = pair.Value;
        writer.Write(giud);
        writer.Write(items.Count);

        foreach (var item in items)
          item.Write(writer);
      }
    }

    private void Build(ITreeNode node)
    {
      // scan before element
      List<Action> results = null;

      foreach (var builder in myBuilders)
      {
        var result = builder.ScanBeforeChildren(node);
        if (result != null)
        {
          if (results == null)
            results = new List<Action>();
          results.Add(result);
        }
      }

      for (var child = node.FirstChild; child != null; child = child.NextSibling)
        Build(child);

      // scan after element
      if (results != null)
        foreach (var action in results)
          action();
    }

    [CanBeNull]
    public static IEnumerable<ICacheItem> Build(IPsiSourceFile sourceFile, IEnumerable<IPsiCacheProvider> providers)
    {
      var file = sourceFile.GetPsiFile<PsiLanguage>() as IPsiFile;
      if (file == null)
        return null;
      return Build(sourceFile, providers, file);
    }

    [CanBeNull]
    public static IEnumerable<ICacheItem> Build(IPsiSourceFile sourceFile, IEnumerable<IPsiCacheProvider> providers, ITreeNode rootNode)
    {
      var ret = new PsiCacheBuilder(sourceFile, providers);
      ret.Build(rootNode);
      return ret.GetSymbols();
    }
  }
}
