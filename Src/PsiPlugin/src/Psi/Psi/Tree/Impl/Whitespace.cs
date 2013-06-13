using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl
{
  internal abstract class WhitespaceBase : PsiTokenBase, IWhitespaceNode
  {
    private readonly string myText;

    protected WhitespaceBase(string text)
    {
      myText = text;
    }

    public override int GetTextLength()
    {
      return myText.Length;
    }

    public override string GetText()
    {
      return myText;
    }

    public override bool IsFiltered()
    {
      return true;
    }

    public override string ToString()
    {
      return base.ToString() + " spaces:" + "\"" + GetText() + "\"";
    }
  }

  internal sealed class Whitespace : WhitespaceBase
  {
    public Whitespace(string text) : base(text) { }

    public override NodeType NodeType
    {
      get { return PsiTokenType.WHITE_SPACE; }
    }
  }

  internal class NewLine : WhitespaceBase
  {
    public NewLine(string text) : base(text) { }

    public override NodeType NodeType
    {
      get { return PsiTokenType.NEW_LINE; }
    }
  }
}
