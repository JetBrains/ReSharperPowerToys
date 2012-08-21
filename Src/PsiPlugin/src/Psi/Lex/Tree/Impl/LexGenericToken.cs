using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Lex.Tree.Impl
{
  internal class LexGenericToken : LexTokenBase
  {
    private readonly TokenNodeType myNodeType;
    private readonly string myText;

    public LexGenericToken(TokenNodeType nodeType, string text)
    {
      myNodeType = nodeType;
      myText = text;
    }

    public override NodeType NodeType
    {
      get { return myNodeType; }
    }

    public override int GetTextLength()
    {
      return myText.Length;
    }

    public override string GetText()
    {
      return myText;
    }
  }
}
