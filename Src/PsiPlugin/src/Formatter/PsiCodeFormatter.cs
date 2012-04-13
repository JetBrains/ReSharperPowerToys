using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Util;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  [Language(typeof(PsiLanguage))]
  public class PsiCodeFormatter : CodeFormatterBase
  {
    private readonly PsiLanguage myLanguage;
    private readonly ISettingsOptimization mySettingsOptimization;
    private IEnumerable<IPsiCodeFormatterExtension> myExtensions;
    private readonly ElementsCache<TokenTypePair, bool> myGlueingCache = new ElementsCache<TokenTypePair, bool>(IsTokensGlued);

    public PsiCodeFormatter(Lifetime lifetime, PsiLanguage language, ISettingsStore settingsStore, ISettingsOptimization settingsOptimization, IViewable<IPsiCodeFormatterExtension> extensions)
      : base(settingsStore)
    {
      myLanguage = language;
      mySettingsOptimization = settingsOptimization;
      myExtensions = extensions.ToLiveEnumerable(lifetime);
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
        return null;

      if (leftToken.GetTokenType() == PsiTokenType.END_OF_LINE_COMMENT && rightToken.GetTokenType() != PsiTokenType.NEW_LINE)
        return PsiFormatterHelper.CreateNewLine();

      if (rightToken is IWhitespaceNode || rightToken.GetTokenType() == PsiTokenType.WHITE_SPACE)
        return null;

      if (leftToken.GetTokenType() == PsiTokenType.LBRACE || leftToken.GetTokenType() == PsiTokenType.RBRACE || leftToken.GetTokenType() == PsiTokenType.RBRACE || leftToken.GetTokenType() == PsiTokenType.RBRACKET || leftToken.GetTokenType() == PsiTokenType.LPARENTH || leftToken.GetTokenType() == PsiTokenType.RPARENTH || leftToken.GetTokenType() == PsiTokenType.LT || leftToken.GetTokenType() == PsiTokenType.GT)
      {
        return null;
      }

      if (rightToken.GetTokenType() == PsiTokenType.LBRACE || rightToken.GetTokenType() == PsiTokenType.RBRACE || rightToken.GetTokenType() == PsiTokenType.RBRACE || rightToken.GetTokenType() == PsiTokenType.RBRACKET || rightToken.GetTokenType() == PsiTokenType.LPARENTH || rightToken.GetTokenType() == PsiTokenType.RPARENTH || rightToken.GetTokenType() == PsiTokenType.LT || rightToken.GetTokenType() == PsiTokenType.GT)
      {
        return null;
      }

      if(rightToken.GetTokenType() == PsiTokenType.ASTERISK || rightToken.GetTokenType() == PsiTokenType.QUEST)
      {
        return null;
      }

      if ((leftToken == PsiTokenType.COLON || leftToken == PsiTokenType.SEMICOLON) &&
      (!(rightToken.GetTokenType() == PsiTokenType.C_STYLE_COMMENT || rightToken.GetTokenType() == PsiTokenType.END_OF_LINE_COMMENT)))
      {
        return PsiFormatterHelper.CreateNewLine();
      }

      var tokenType1 = leftToken.GetTokenType();
      var tokenType2 = rightToken.GetTokenType();

      if (myGlueingCache.Get(new TokenTypePair(tokenType1, tokenType2)))
        return
          tokenType1 == PsiTokenType.END_OF_LINE_COMMENT
            ? PsiFormatterHelper.CreateNewLine()
            : PsiFormatterHelper.CreateSpace(" ");

      return null;
    }

    protected override PsiLanguageType LanguageType
    {
      get { return myLanguage; }
    }

    public override ITreeNode[] CreateSpace(string indent, ITreeNode rightNonSpace, ITreeNode replacedSpace)
    {
      return new[] { PsiFormatterHelper.CreateSpace(indent) };
    }

    public void Format(ITreeNode firstElement, ITreeNode lastElement, PsiFormatProfile profile, [CanBeNull] IProgressIndicator pi, IContextBoundSettingsStore overrideSettingsStore = null)
    {
      var firstNode = firstElement;
      var lastNode = lastElement;
      var commonParent = firstNode.FindCommonParent(lastNode);
      if(firstNode.NextSibling == null)
      {
        var tempNode = lastNode;
        while( tempNode.Parent != commonParent)
        {
          tempNode = tempNode.Parent;
        }

        while (tempNode.FirstChild != null)
        {
          tempNode = tempNode.FirstChild;
        }
        firstNode = tempNode;
      }
      var solution = firstNode.GetSolution();
      var globalSettings = GlobalFormatSettingsHelper.GetService(solution).GetSettingsForLanguage(myLanguage);
      var contextBoundSettingsStore = GetProperContextBoundSettingsStore(overrideSettingsStore, firstNode);
      var formatterSettings = new PsiCodeFormattingSettings(contextBoundSettingsStore, mySettingsOptimization, globalSettings);
      using (pi.SafeTotal(4))
      {
        var context = new PsiCodeFormattingContext(this, firstNode, lastNode, NullProgressIndicator.Instance);
        if (profile.Profile != CodeFormatProfile.INDENT)
        {
          using (var subPi = pi.CreateSubProgress(1))
          {
            //FormatterImplHelper.DecoratingIterateNodes(context, context.FirstNode, context.LastNode, new PsiDecorationStage(formatterSettings, profile, subPi));
          }

          using (var subPi = pi.CreateSubProgress(1))
          {
            using (subPi.SafeTotal(2))
            {
              var data = new FormattingStageData(formatterSettings, context, profile, myExtensions.ToList());
              PsiFormattingStage.DoFormat(data, subPi.CreateSubProgress(1));
              PsiIndentingStage.DoIndent(formatterSettings, context, false, subPi.CreateSubProgress(1));
            }
          }
        }
        else
        {
          using (var subPi = pi.CreateSubProgress(4))
          {
            PsiIndentingStage.DoIndent(formatterSettings, context, false, subPi);
          }
        }
      }
    }

    public override ITreeRange Format(ITreeNode firstElement, ITreeNode lastElement, CodeFormatProfile profile, IProgressIndicator progressIndicator, IContextBoundSettingsStore overrideSettingsStore = null)
    {
      Format(
        firstElement,
        lastElement,
        new PsiFormatProfile(profile),
        progressIndicator,
        overrideSettingsStore);
      return new TreeRange(firstElement, lastElement);
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
        if (ReferenceEquals(null, obj)) return false;
        if (!(obj is TokenTypePair)) return false;
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
  }

  public class PsiCodeFormattingContext : CodeFormattingContext
  {
    public PsiCodeFormattingContext(PsiCodeFormatter psiCodeFormatter, ITreeNode firstNode, ITreeNode lastNode, NullProgressIndicator instance) : base(psiCodeFormatter, firstNode, lastNode, instance)
    {
    }

    protected override bool CanModifyNode(ITreeNode element, NodeModificationType nodeModificationType)
    {
      return true;
    }
  }
}
