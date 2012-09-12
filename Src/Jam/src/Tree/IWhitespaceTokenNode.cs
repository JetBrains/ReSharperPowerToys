using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.Tree
{
  public interface IWhitespaceTokenNode : ITokenNode
  {
    bool IsNewLine { get; }
  }
}