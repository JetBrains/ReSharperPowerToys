using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.Cache
{
  /// <summary>
  ///   Customization point for pdi properties
  /// </summary>
  [CannotApplyEqualityOperator]
  public interface IPsiSymbol
  {
    /// <summary>
    ///   Offset of symbol declaration in the source fileFullName tree
    /// </summary>
    int Offset { get; }

    /// <summary>
    ///   Owner PSI source fileFullName
    /// </summary>
    IPsiSourceFile SourceFile { get; }

    string Name { get;}
  }
}
