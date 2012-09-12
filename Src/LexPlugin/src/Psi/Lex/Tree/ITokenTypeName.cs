using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree
{
  public partial interface ITokenTypeName
  {
    void SetName(string shortName);
  }
}
