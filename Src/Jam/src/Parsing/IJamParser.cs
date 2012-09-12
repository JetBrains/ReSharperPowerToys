using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Parsing;

namespace JetBrains.ReSharper.Psi.Jam.Parsing
{
  public interface IJamParser : IParser
  {
    new IJamFile ParseFile();
    IJamDeclaration ParseDeclaration();
    IJamStatement ParseStatement();
    IJamExpression ParseExpression();
  }
}