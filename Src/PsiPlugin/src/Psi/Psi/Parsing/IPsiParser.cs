using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing
{
  public interface IPsiParser : IParser
  {
    TreeElement ParseStatement();
  }
}
