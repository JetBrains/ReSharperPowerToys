using JetBrains.ReSharper.Psi.Impl;

namespace JetBrains.ReSharper.Psi.Jam.Tree
{
  public partial interface IProcedureDeclaration : IResolveIsolationScope
  {
    new IProcedureDeclaredElement DeclaredElement { get; }
  }
}