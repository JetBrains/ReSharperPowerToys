using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  [Language(typeof (PsiLanguage))]
  public class PsiCodeFormatter : CodeFormatterBase
  {
    private readonly ElementsCache<TokenTypePair, bool> myGlueingCache = new ElementsCache<TokenTypePair, bool>(IsTokensGlued);
    private readonly PsiLanguage myLanguage;

    public PsiCodeFormatter(Lifetime lifetime, PsiLanguage language, ISettingsStore settingsStore, ISettingsOptimization settingsOptimization)
      : base(settingsStore)
    {
      myLanguage = language;
    }

    protected override PsiLanguageType LanguageType
    {
      get { return myLanguage; }
    }

    public override bool IsWhitespaceToken(ITokenNode token)
    {
      return token.IsWhitespaceToken();
    }

    protected override bool IsFormatNextSpaces(CodeFormatProfile profile)
    {
      return profile != CodeFormatProfile.INDENT;
    }

    public override ITokenNode GetMinimalSeparator(ITokenNode leftToken, ITokenNode rightToken)
    {
      if (leftToken is IWhitespaceNode || leftToken.GetTokenType() == PsiTokenType.WHITE_SPACE)
      {
        return null;
      }

      if (leftToken.GetTokenType() == PsiTokenType.END_OF_LINE_COMMENT && rightToken.GetTokenType() != PsiTokenType.NEW_LINE)
      {
        return PsiFormatterHelper.CreateNewLine("\r\n");
      }

      if (rightToken is IWhitespaceNode || rightToken.GetTokenType() == PsiTokenType.WHITE_SPACE)
      {
        return null;
      }

      if (leftToken.GetTokenType() == PsiTokenType.LBRACE || leftToken.GetTokenType() == PsiTokenType.RBRACE || leftToken.GetTokenType() == PsiTokenType.RBRACE || leftToken.GetTokenType() == PsiTokenType.RBRACKET || leftToken.GetTokenType() == PsiTokenType.LPARENTH || leftToken.GetTokenType() == PsiTokenType.RPARENTH || leftToken.GetTokenType() == PsiTokenType.LT || leftToken.GetTokenType() == PsiTokenType.GT)
      {
        return null;
      }

      if (rightToken.GetTokenType() == PsiTokenType.LBRACE || rightToken.GetTokenType() == PsiTokenType.RBRACE || rightToken.GetTokenType() == PsiTokenType.RBRACE || rightToken.GetTokenType() == PsiTokenType.RBRACKET || rightToken.GetTokenType() == PsiTokenType.LPARENTH || rightToken.GetTokenType() == PsiTokenType.RPARENTH || rightToken.GetTokenType() == PsiTokenType.LT || rightToken.GetTokenType() == PsiTokenType.GT)
      {
        return null;
      }

      if (rightToken.GetTokenType() == PsiTokenType.ASTERISK || rightToken.GetTokenType() == PsiTokenType.QUEST)
      {
        return null;
      }

      if ((leftToken.GetTokenType() == PsiTokenType.COLON || leftToken.GetTokenType() == PsiTokenType.SEMICOLON) &&
        (!(rightToken.GetTokenType() == PsiTokenType.C_STYLE_COMMENT || rightToken.GetTokenType() == PsiTokenType.END_OF_LINE_COMMENT)))
      {
        if(rightToken.Parent is IRoleName)
        {
          return null;
        }
        return PsiFormatterHelper.CreateNewLine("\r\n");
      }

      TokenNodeType tokenType1 = leftToken.GetTokenType();
      TokenNodeType tokenType2 = rightToken.GetTokenType();

      if (myGlueingCache.Get(new TokenTypePair(tokenType1, tokenType2)))
      {
        return
          tokenType1 == PsiTokenType.END_OF_LINE_COMMENT
            ? PsiFormatterHelper.CreateNewLine("\r\n")
            : PsiFormatterHelper.CreateSpace(" ");
      }

      return null;
    }

    public override ITreeNode[] CreateSpace(string indent, ITreeNode rightNonSpace, ITreeNode replacedSpace)
    {
      return new ITreeNode[] { PsiFormatterHelper.CreateSpace(indent) };
    }

    public override ITreeRange Format(ITreeNode firstElement, ITreeNode lastElement, CodeFormatProfile profile, IProgressIndicator pi, IContextBoundSettingsStore overrideSettingsStore = null)
    {
      ITreeNode firstNode;
      ITreeNode lastNode;

      GetFirstAndLastNode(firstElement, lastElement, out firstNode, out lastNode);

      using (pi.SafeTotal(3))
      {
        var context = new PsiCodeFormattingContext(this, firstNode, lastNode, NullProgressIndicator.Instance);
        if (profile != CodeFormatProfile.INDENT)
        {

          using (IProgressIndicator subPi = pi.CreateSubProgress(2))
          {
            using (subPi.SafeTotal(2))
            {
              PsiFormattingStage.DoFormat(context, subPi.CreateSubProgress(1));
              PsiIndentingStage.DoIndent(context, subPi.CreateSubProgress(1), false);
            }
          }
        }
        else
        {
          using (IProgressIndicator subPi = pi.CreateSubProgress(4))
          {
            PsiIndentingStage.DoIndent(context, subPi, true);
          }
        }
      }
      return new TreeRange(firstElement, lastElement);
    }

    private static void GetFirstAndLastNode(ITreeNode firstElement, ITreeNode lastElement, out ITreeNode firstNode, out ITreeNode lastNode)
    {
      firstNode = firstElement;
      lastNode = lastElement;
      if (firstElement != lastElement)
      {
        if (firstElement == null)
        {
          firstNode = lastElement;
        }
        ITreeNode commonParent = firstNode.FindCommonParent(lastNode);
        ITreeNode firstChild = firstNode;
        ITreeNode lastChild = lastElement;
        while ((firstChild.Parent != null) && (firstChild.Parent != commonParent))
        {
          firstChild = firstChild.Parent;
        }
        while ((lastChild.Parent != null) && (lastChild.Parent != commonParent))
        {
          lastChild = lastChild.Parent;
        }

        firstNode = firstChild;
        while (firstNode.FirstChild != null)
        {
          firstNode = firstNode.FirstChild;
        }

        lastNode = lastChild;
        while (lastNode.LastChild != null)
        {
          lastNode = lastNode.LastChild;
        }
      }
      else
      {
        if (firstElement.FirstChild != null)
        {
          firstNode = firstElement.FirstChild;
          lastNode = firstElement.LastChild;
        }
      }
    }

    public override void FormatInsertedNodes(ITreeNode nodeFirst, ITreeNode nodeLast, bool formatSurround)
    {
      Format(nodeFirst, nodeLast, CodeFormatProfile.GENERATOR, null);
    }

    public override ITreeRange FormatInsertedRange(ITreeNode nodeFirst, ITreeNode nodeLast, ITreeRange origin)
    {
      Format(nodeFirst, nodeLast, CodeFormatProfile.GENERATOR, null);
      return new TreeRange(nodeFirst, nodeLast);
    }

    public override void FormatReplacedNode(ITreeNode oldNode, ITreeNode newNode)
    {
      FormatInsertedNodes(newNode, newNode, false);
    }

    public override void FormatDeletedNodes(ITreeNode parent, ITreeNode prevNode, ITreeNode nextNode)
    {
      Format(
        prevNode,
        nextNode,
        CodeFormatProfile.GENERATOR,
        null);
    }

    private static bool IsTokensGlued(TokenTypePair key)
    {
      var lexer = new PsiLexer(new StringBuffer(key.Type1.GetSampleText() + key.Type2.GetSampleText()));
      return lexer.LookaheadToken(1) == null;
    }

    #region Nested type: TokenTypePair

    private struct TokenTypePair
    {
      private readonly TokenNodeType myType1;
      private readonly TokenNodeType myType2;

      public TokenTypePair(TokenNodeType type1, TokenNodeType type2)
      {
        myType1 = type1;
        myType2 = type2;
      }

      public TokenNodeType Type1
      {
        get { return myType1; }
      }

      public TokenNodeType Type2
      {
        get { return myType2; }
      }

      private bool Equals(TokenTypePair other)
      {
        return Equals(other.myType1, myType1) && Equals(other.myType2, myType2);
      }

      public override bool Equals(object obj)
      {
        if (ReferenceEquals(null, obj))
        {
          return false;
        }
        if (!(obj is TokenTypePair))
        {
          return false;
        }
        return Equals((TokenTypePair)obj);
      }

      public override int GetHashCode()
      {
        unchecked
        {
          return ((myType1 != null ? myType1.GetHashCode() : 0) * 397) ^ (myType2 != null ? myType2.GetHashCode() : 0);
        }
      }
    }

    #endregion
  }

  public class PsiCodeFormattingContext : CodeFormattingContext
  {
    public PsiCodeFormattingContext(PsiCodeFormatter psiCodeFormatter, ITreeNode firstNode, ITreeNode lastNode, NullProgressIndicator instance)
      : base(psiCodeFormatter, firstNode, lastNode, instance)
    {
    }

    protected override bool CanModifyNode(ITreeNode element, NodeModificationType nodeModificationType)
    {
      return true;
    }
  }
}
