using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiIndentVisitor : TreeNodeVisitor<FormattingStageContext, string>
  {
    private PsiCodeFormattingSettings myFormattingSettings;
    private PsiIndentCache myIndentCache;
    private string myContIndent;

    public PsiIndentVisitor(PsiCodeFormattingSettings formattingSettings, PsiIndentCache indentCache)
    {
      myFormattingSettings = formattingSettings;
      myIndentCache = indentCache;
      myContIndent = formattingSettings.GlobalSettings.InsertTabs ? new string('\t', 1) : new string(' ', 1 * formattingSettings.GlobalSettings.IndentSize);
    }

    public override string VisitExtrasDefinition(IExtrasDefinition extrasDefinitionParam, FormattingStageContext context)
    {
      return myIndentCache.GetNodeIndent(extrasDefinitionParam);
    }

    public override string VisitOptionsDefinition(IOptionsDefinition optionsDefinitionParam, FormattingStageContext context)
    {
      return myIndentCache.GetNodeIndent(optionsDefinitionParam);
    }

    public override string VisitRuleBody(IRuleBody ruleBodyParam, FormattingStageContext context)
    {
      return myIndentCache.GetNodeIndent(ruleBodyParam);
    }

    public override string VisitRuleDeclaration(IRuleDeclaration ruleDeclarationParam, FormattingStageContext context)
    {
      return myIndentCache.GetNodeIndent(ruleDeclarationParam);
    }
  }
}