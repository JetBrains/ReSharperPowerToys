using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;

namespace JetBrains.ReSharper.PsiPlugin.Parsing
{
    public interface IPsiParser : IParser
    {
      TreeElement ParseStatement();
    }
}
