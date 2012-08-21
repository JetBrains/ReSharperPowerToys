using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Lex.Parsing
{
  public interface ILexParser : IParser
  {
    TreeElement ParseStatement();
  }
}
