using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Lex.Parsing;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Lex.Tree.Impl
{
  internal abstract class WhitespaceBase : LexTokenBase, IWhitespaceNode
  {
    private readonly string myText;

    protected WhitespaceBase(string text)
    {
      myText = text;
    }

    #region IWhitespaceNode Members

    public override int GetTextLength()
    {
      return myText.Length;
    }

    public override string GetText()
    {
      return myText;
    }

    #endregion

    public override String ToString()
    {
      return base.ToString() + " spaces:" + "\"" + GetText() + "\"";
    }
  }

  internal sealed class Whitespace : WhitespaceBase
  {
    public Whitespace(string text)
      : base(text)
    {
    }

    public override NodeType NodeType
    {
      get { return LexTokenType.WHITE_SPACE; }
    }
  }

  internal class NewLine : WhitespaceBase
  {
    public NewLine(string text)
      : base(text)
    {
    }

    public override NodeType NodeType
    {
      get { return LexTokenType.NEW_LINE; }
    }
  }
}
