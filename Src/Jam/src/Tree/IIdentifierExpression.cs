using JetBrains.ReSharper.Psi.Resolve;

namespace JetBrains.ReSharper.Psi.Jam.Tree
{
  public partial interface IIdentifierExpression
  {
    IReference Reference { get; }
  }
}