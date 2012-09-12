using System;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;

namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  public class JamToken : JamTokenBase
  {
    private readonly string myText;
    private readonly TokenNodeType myTokenType;

    public JamToken(TokenNodeType tokenType, string text)
    {
      myTokenType = tokenType;
      myText = text ?? String.Empty;
    }

    public override NodeType NodeType
    {
      get { return myTokenType; }
    }

    public override int GetTextLength()
    {
      return myText.Length;
    }

    public override string GetText()
    {
      return myText;
    }

    public override IBuffer GetTextAsBuffer()
    {
      return new StringBuffer(myText);
    }
  }
}