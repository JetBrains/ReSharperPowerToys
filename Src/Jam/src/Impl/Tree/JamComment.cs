using System;
using JetBrains.ReSharper.Psi.Jam.Parsing;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  internal class JamComment : JamToken, IJamCommentNode, IChameleonNode
  {
    public JamComment(JamTokenType tokenType, string text) : base(tokenType, text) {}

    #region IChameleonNode Members

    public bool IsOpened
    {
      get { return true; }
    }

    public virtual IChameleonNode ReSync(CachingLexer cachingLexer, TreeTextRange changedRange, int insertedTextLen)
    {
      return null;
    }

    #endregion

    #region IJamCommentNode Members

    public string CommentText
    {
      get
      {
        string text = GetText();
        return text.Substring(2, text.Length - (text.EndsWith("*/", StringComparison.OrdinalIgnoreCase) ? 4 : 2));
      }
    }

    public TreeTextRange GetCommentRange()
    {
      TreeOffset startOffset = GetTreeStartOffset();
      string text = GetText();
      int length = text.Length - (text.EndsWith("*/", StringComparison.OrdinalIgnoreCase) ? 4 : 2);
      if (length <= 0)
        return TreeTextRange.InvalidRange;
      return new TreeTextRange(startOffset + 2, startOffset + 2 + length);
    }

    #endregion

    public override IChameleonNode FindChameleonWhichCoversRange(TreeTextRange textRange)
    {
      return textRange.ContainedIn(TreeTextRange.FromLength(GetTextLength())) ? this : null;
    }
  }
}