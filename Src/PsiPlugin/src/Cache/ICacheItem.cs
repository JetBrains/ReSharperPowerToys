using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JetBrains.ReSharper.PsiPlugin.Cach
{
  public interface ICacheItem
  {
    /// <summary>
    /// Guid of symbol kind for persisting...
    /// </summary>
    string SymbolTypeGuid { get; }

    /// <summary>
    /// Short name of a symbol
    /// </summary>
    string Text { get; }
  }

  public interface IPersistentCacheItem : ICacheItem
  {

    void Write(BinaryWriter writer);
    void Read(BinaryReader reader);
  }
}
