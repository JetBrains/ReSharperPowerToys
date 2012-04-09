using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  [Language(typeof(PsiLanguage))]
  public class PsiCodeFormatter : CodeFormatterBase
  {
    private readonly PsiLanguage myLanguage;
    private readonly ISettingsOptimization mySettingsOptimization;
    private IEnumerable<IPsiCodeFormatterExtension> myExtensions;

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

      if ((leftToken == PsiTokenType.COLON || leftToken == PsiTokenType.SEMICOLON) &&
      (!(rightToken.GetTokenType() == PsiTokenType.C_STYLE_COMMENT || rightToken.GetTokenType() == PsiTokenType.END_OF_LINE_COMMENT)))
      {
        return PsiFormatterHelper.CreateNewLine();
      }

      return PsiFormatterHelper.CreateSpace(" ");
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
      var solution = firstNode.GetSolution();
      var globalSettings = GlobalFormatSettingsHelper.GetService(solution).GetSettingsForLanguage(myLanguage);
      var contextBoundSettingsStore = GetProperContextBoundSettingsStore(overrideSettingsStore, firstNode);
      var formatterSettings = new PsiCodeFormattingSettings(contextBoundSettingsStore, mySettingsOptimization, globalSettings);
      using (pi.SafeTotal(4))
      {
        var context = new CodeFormattingContext(this, firstNode, lastNode, NullProgressIndicator.Instance);
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
  }
}
