using System;
using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.JavaScript.Impl.Tree;
using JetBrains.ReSharper.Psi.JavaScript.LanguageImpl;
using JetBrains.ReSharper.Psi.JavaScript.Parsing;
using JetBrains.ReSharper.Psi.JavaScript.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.ResearchFormatter.JavaScript
{
  [Language(typeof(JavaScriptLanguage))]
  public class JavaScriptResearchFormatter : FormatterResearchBase
  {
    public static JavaScriptResearchFormatter Instance;

    private static readonly IEnumerable<IFormattingRule> OurFormattingRules = new List<IFormattingRule>()
      {
        new FormattingRule(ElementType.JAVA_SCRIPT_FILE,new[]{"\n","\n"}),
        new FormattingRule(JavaScriptTokenType.END_OF_LINE_COMMENT, JavaScriptTokenType.END_OF_LINE_COMMENT,"\n"),
        new FormattingRuleAfterNode(ElementType.EXPRESSION_STATEMENT,"\n"),
        new FormattingRuleAfterNode(ElementType.JAVA_SCRIPT_FILE_SECTION, ElementType.EXPRESSION_STATEMENT,"\n"),
        new FormattingRuleAfterNode(ElementType.VARIABLE_STATEMENT,"\n"),
        new FormattingRuleAfterNode(ElementType.IF_STATEMENT,"\n"),
        new FormattingRuleAfterNode(ElementType.JAVA_SCRIPT_FILE_SECTION,ElementType.IF_STATEMENT,new[]{"\n","\n"}),
        new FormattingRuleAfterNode(ElementType.WHILE_STATEMENT,"\n"),
        new FormattingRuleAfterNode(ElementType.BREAK_STATEMENT,"\n"),
        new FormattingRuleAfterNode(ElementType.CASE_CASE_CLAUSE,"\n"),
        new FormattingRuleAfterNode(ElementType.CASE_CASE_CLAUSE, JavaScriptTokenType.COLON, "\n"),
        new FormattingRuleAfterNode(ElementType.DEFAULT_CLAUSE, JavaScriptTokenType.COLON, "\n"),
        new FormattingRuleAfterNode(JavaScriptTokenType.LBRACE,"\n"),
        new FormattingRuleBeforeNode(JavaScriptTokenType.RBRACE,"\n"),
        new FormattingRule(ElementType.JAVA_SCRIPT_FILE_SECTION,new[]{"\n","\n"}),
        new FormattingRule(ElementType.BLOCK,new[]{"\n"}),
        new FormattingRule(JavaScriptTokenType.C_STYLE_COMMENT, new[]{"\n"}),
        new FormattingRuleBeforeToken(".",""),
        new FormattingRuleAfterToken(".", ""),
        new FormattingRuleAfterToken(",", " "),
        new FormattingRuleAfterToken(":", " "),
        new FormattingRule(ElementType.BINARY_EXPRESSION," "),
        new FormattingRuleBeforeToken(";",""),
        new FormattingRuleBeforeToken("(",""),
        new FormattingRuleAfterToken(ElementType.OBJECT_PROPERTIES_LIST,",", "\n"),
        new FormattingRuleAfterNode(JavaScriptTokenType.END_OF_LINE_COMMENT,"\n"),
        new FormattingRuleBeforeNode(JavaScriptTokenType.END_OF_LINE_COMMENT,"\n"),
        //new FormattingRule(ElementType.OBJECT_PROPERTIES_LIST,"\n"),
        new FormattingRuleAfterToken(ElementType.OBJECT_LITERAL,"{", "\n"),
        new FormattingRuleBeforeToken(ElementType.OBJECT_LITERAL,"}", "\n"),
        new CustomNewLineFormattingRule(ElementType.FUNCTION_EXPRESSION, "(", ")", JavaScriptTokenType.COMMA, ElementType.FORMAL_PARAMETER," "),
        new CustomNewLineFormattingRule(ElementType.FUNCTION_EXPRESSION, "(", ")", JavaScriptTokenType.LPARENTH, ElementType.FORMAL_PARAMETER_LIST,""),
        new CustomNewLineFormattingRule(ElementType.FUNCTION_EXPRESSION, "(", ")", ElementType.FORMAL_PARAMETER_LIST, JavaScriptTokenType.RPARENTH,"")
      };

    private static readonly IEnumerable<IndentingRule> OurIndentingRules = new List<IndentingRule>()
      {
        new BoundIndentingRule(ElementType.BLOCK,"{","}"),
        //new BoundIndentingRule(ElementType.FUNCTION_EXPRESSION,"(",")"),
        new BoundIndentingRule(ElementType.OBJECT_LITERAL, "{", "}"),
        new BoundIndentingRule(ElementType.SWITCH_STATEMENT, "{", "}"),
        new AlignmentIndentingRule(ElementType.FUNCTION_EXPRESSION,"(",")"),
        new IndentingSimpleRule(ElementType.FORMAL_PARAMETER_LIST),
        new IndentingSimpleRule(ElementType.CASE_CASE_CLAUSE, IndentType.Left),
        new IndentingSimpleRule(ElementType.DEFAULT_CLAUSE, IndentType.Left)
      };

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
