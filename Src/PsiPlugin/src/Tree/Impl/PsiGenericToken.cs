using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  internal class PsiGenericToken : PsiTokenBase
  {
    private readonly TokenNodeType myNodeType;
    private readonly string myText;

    public PsiGenericToken(TokenNodeType nodeType, string text)
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
