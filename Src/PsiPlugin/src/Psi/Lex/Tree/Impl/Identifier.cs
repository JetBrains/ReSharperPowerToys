using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Lex.Parsing;
using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Lex.Tree.Impl
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
      get { return PsiResolveUtil.ReferenceName(myText); }
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
