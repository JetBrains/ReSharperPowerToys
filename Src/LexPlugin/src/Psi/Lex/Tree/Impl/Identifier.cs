using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.LexPlugin.Psi.Lex.Parsing;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;

namespace JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree.Impl
{
  internal class Identifier : LexTokenBase, ILexIdentifier
  {
    private readonly string myText;

    public Identifier(string text)
    {
      myText = text;
    }

    public override NodeType NodeType
    {
      get { return LexTokenType.IDENTIFIER; }
    }

    #region IPsiIdentifier Members

    public string Name
    {
      get { return LexResolveUtil.ReferenceName(myText); }
    }

    public override int GetTextLength()
    {
      return myText.Length;
    }

    public override string GetText()
    {
      return myText;
    }

    #endregion    
  }
}
