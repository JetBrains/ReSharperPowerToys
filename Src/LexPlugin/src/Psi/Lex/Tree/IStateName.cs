using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.LexPlugin.Resolve;

namespace JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree
{
  public partial interface IStateName
  {
    LexStateReference StateNameReference { get; }
  }
}
