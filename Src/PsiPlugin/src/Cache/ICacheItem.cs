using System.IO;

namespace JetBrains.ReSharper.PsiPlugin.Cache
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
    string Name { get; }

    string Value { get; }
  }

  public interface IPersistentCacheItem : ICacheItem
  {

    void Write(BinaryWriter writer);
    void Read(BinaryReader reader);
  }
}
