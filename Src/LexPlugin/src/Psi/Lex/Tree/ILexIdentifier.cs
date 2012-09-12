using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree
{
  public interface ILexIdentifier : IIdentifier, ITokenNode, ILexTreeNode
  {
  }
}
