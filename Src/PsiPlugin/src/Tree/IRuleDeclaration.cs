using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Tree
{
  public partial interface IRuleDeclaration : IDeclaration, IChameleonNode
  {
    ISymbolTable VariableSymbolTable { get; }
  }
}
