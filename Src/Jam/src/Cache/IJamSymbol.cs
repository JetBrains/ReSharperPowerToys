using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.Cache
{
  [CannotApplyEqualityOperator]
  public interface IJamSymbol 
  {
    JamSymbolType SymbolType { get; }
    int Offset { get; }
    [NotNull] string Name { get; }
    [NotNull] IPsiSourceFile PsiSourceFile { get; }

    [CanBeNull] IDeclaration GetDeclaration();
  }
}