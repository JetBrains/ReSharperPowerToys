using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Tree
{
  public partial interface IRuleDeclaration:IDeclaration, IChameleonNode
  {
    ISymbolTable VariableSymbolTable { get; }
  }
}
