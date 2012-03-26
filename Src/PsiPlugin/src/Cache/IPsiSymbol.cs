using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.Cache
{
  /// <summary>
  /// Customization point for pdi properties
  /// </summary>
  [CannotApplyEqualityOperator]
  public interface IPsiSymbol
  {
    /// <summary>
    /// Offset of symbol declaration in the source file tree
    /// </summary>
    int Offset { get; }
    /// <summary>
    /// Owner PSI source file
    /// </summary>
    IPsiSourceFile SourceFile { get; }
  }
}
