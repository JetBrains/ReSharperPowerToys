using System;
using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.JavaScript.LanguageImpl;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.ResearchFormatter.JavaScript
{
  [Language(typeof(JavaScriptLanguage))]
  public class JavaScriptResearchFormatter : FormatterResearchBase
  {
    public static JavaScriptResearchFormatter Instance;

    private static readonly IEnumerable<IFormattingRule> OurFormattingRules = new List<IFormattingRule>() {};
    private static readonly IEnumerable<IndentingRule> OurIndentingRules = new List<IndentingRule>() {};

    public JavaScriptResearchFormatter(ISettingsStore settingsStore) : base(settingsStore)
    {
      Instance = this;
    }

    #region Overrides of CodeFormatterBase

    public override bool IsWhitespaceToken(ITokenNode token)
    {
      throw new NotImplementedException();
    }

    protected override bool IsFormatNextSpaces(CodeFormatProfile profile)
    {
      throw new NotImplementedException();
    }

    public override ITokenNode GetMinimalSeparator(ITokenNode leftToken, ITokenNode rightToken)
    {
      throw new NotImplementedException();
    }

    protected override PsiLanguageType LanguageType
    {
      get { return JavaScriptLanguage.Instance; }
    }

    public override ITreeNode[] CreateSpace(string indent, ITreeNode rightNonSpace, ITreeNode replacedSpace)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region Overrides of FormatterResearchBase

    public override IEnumerable<IFormattingRule> FormattingRules { get { return OurFormattingRules; } }
    public override IEnumerable<IndentingRule> IndentingRules { get { return OurIndentingRules; } }

    protected override IndentingStageResearchBase IndentingStage()
    {
      return new JavaScriptIndentingStageResearch(this);
    }

    protected override FormattingStageResearchBase FormattingStage(CodeFormattingContext context)
    {
      return new JavaScriptFormattingStageResearch(context, this);
    }

    #endregion
  }
}
