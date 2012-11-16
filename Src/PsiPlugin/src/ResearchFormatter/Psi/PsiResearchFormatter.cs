using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.ResearchFormatter.Psi
{
  [Language(typeof(PsiLanguage))]
  public class PsiResearchFormatter : FormatterResearchBase
  {
    private readonly PsiLanguageType myLanguage;
    private readonly ISettingsOptimization mySettingsOptimization;
    private static readonly IEnumerable<IFormattingRule> OurFormattingRules = new List<IFormattingRule>()
      {
        new FormattingRuleAfterToken(typeof(IRuleDeclaration),":","\n"),
        new FormattingRuleBeforeToken(typeof(IRuleDeclaration),":","\n"),
        new FormattingRuleBeforeToken(typeof(IRuleDeclaration),";","\n"),
        new FormattingRuleBeforeToken(typeof(IChoiceTail),"|","\n"),
        new FormattingRuleBeforeToken("<",null),
        new FormattingRuleBeforeToken(">",null),
        new FormattingRuleAfterToken("<", null),
        new FormattingRuleAfterToken(">",null),
        new FormattingRuleAfterToken(":",null),
        new FormattingRuleBeforeToken(":",null),
        new FormattingRuleAfterToken("extras","\n"),
        new FormattingRuleAfterToken(typeof(IExtrasDefinition),"{","\n"),
        new FormattingRuleBeforeToken(typeof(IExtrasDefinition),"}","\n"),
        new FormattingRule(typeof(IPsiExpression),"\n"),
        new FormattingRule(typeof(IPsiFile),new[]{"\n","\n"}),
        new FormattingRule(typeof(IPsiFile), typeof(ICommentNode), typeof(ICommentNode),"\n"),
        new FormattingRule(typeof(IInterfacesDefinition),new[]{"\n","\n"}),
        new FormattingRule(typeof(IInterfacesDefinition), typeof(ICommentNode), typeof(ICommentNode),"\n"),
        new FormattingRule(typeof(ISequence),"\n"),
        new FormattingRule(typeof(IRuleDeclaration)," "),
        new FormattingRule(typeof(IExtrasDefinition),"\n"),
        new FormattingRule(typeof(IParenExpression),"\n"),
        new FormattingRule(typeof(IOptionsDefinition),"\n"),
        new FormattingRule(typeof(IRuleDeclaration), null, typeof(IExtrasDefinition),"\n"),
        new FormattingRuleBeforeNode(typeof(IQuantifier), null),
        new FormattingRuleBeforeNode(typeof(IRuleParameters), null),
        new FormattingRuleBeforeNode(typeof(IRuleBracketTypedParameters), null)
      };

    private static readonly IEnumerable<IndentingRule> OurIndentingRules = new List<IndentingRule>()
      {
        new BoundIndentingRule(typeof(IRuleDeclaration), ":", ";", false),
        new BoundIndentingRule(typeof(IParenExpression), "(", ")"),
        new BoundIndentingRule(typeof(IInterfacesDefinition), "{", "}"),
        new IndentingSimpleRule(typeof(IRuleBody))
      };

    public PsiResearchFormatter(PsiLanguageType language, ISettingsStore settingsStore, ISettingsOptimization settingsOptimization) : base(settingsStore)
    {
      myLanguage = language;
      mySettingsOptimization = settingsOptimization;
    }

    #region Overrides of CodeFormatterBase

    public override bool IsWhitespaceToken(ITokenNode token)
    {
      return false;
    }

    protected override bool IsFormatNextSpaces(CodeFormatProfile profile)
    {
      return false;
    }

    public override ITokenNode GetMinimalSeparator(ITokenNode leftToken, ITokenNode rightToken)
    {
      return null;
    }

    protected override PsiLanguageType LanguageType
    {
      get { return PsiLanguage.Instance; }
    }

    public override ITreeNode[] CreateSpace(string indent, ITreeNode rightNonSpace, ITreeNode replacedSpace)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region Overrides of FormatterResearchBase

    public override IEnumerable<IFormattingRule> FormattingRules
    {
      get { return OurFormattingRules; }
    }

    public override IEnumerable<IndentingRule> IndentingRules
    {
      get { return OurIndentingRules; }
    }

    protected override IndentingStageResearchBase IndentingStage()
    {
      return new PsiIndentingStageResearch(this);
    }

    protected override FormattingStageResearchBase FormattingStage(CodeFormattingContext context)
    {
      return new PsiFormattingStageResearch(context, this);
    }

    #endregion
  }
}
