using System;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Parsing;

namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  internal abstract class JamWhitespaceTokenBase : JamToken, IWhitespaceTokenNode
  {
    protected JamWhitespaceTokenBase(string text, TokenNodeType tokenType) : base(tokenType, text) {}

    #region IWhitespaceTokenNode Members

    public abstract bool IsNewLine { get; }

    #endregion

    public override String ToString()
    {
      return base.ToString() + " spaces:" + "\"" + GetText() + "\"";
    }
  }
}