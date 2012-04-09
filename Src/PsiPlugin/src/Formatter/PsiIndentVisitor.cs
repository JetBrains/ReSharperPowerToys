using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  internal class PsiIndentVisitor : TreeNodeVisitor<FormattingStageContext, string>
  {
    public override string VisitExtrasDefinition(IExtrasDefinition extrasDefinitionParam, FormattingStageContext context)
    {
      return base.VisitExtrasDefinition(extrasDefinitionParam, context);
    }

    public override string VisitOptionsDefinition(IOptionsDefinition optionsDefinitionParam, FormattingStageContext context)
    {
      return base.VisitOptionsDefinition(optionsDefinitionParam, context);
    }

    public override string VisitRuleBody(IRuleBody ruleBodyParam, FormattingStageContext context)
    {
      return base.VisitRuleBody(ruleBodyParam, context);
    }

    public override string VisitRuleDeclaration(IRuleDeclaration ruleDeclarationParam, FormattingStageContext context)
    {
      return base.VisitRuleDeclaration(ruleDeclarationParam, context);
    }
  }
}