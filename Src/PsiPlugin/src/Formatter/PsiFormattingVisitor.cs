using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiFormattingVisitor: Tree.TreeNodeVisitor<PsiFmtStageContext, IEnumerable<string>>{
    [NotNull] private readonly FormattingStageData myData;

    private readonly bool myIsGenerated;

    public PsiFormattingVisitor(FormattingStageData data)
    {
      myData = data;
      var node = data.Context.FirstNode;
      var projectFile = node.GetSourceFile();
      if (projectFile != null)
        myIsGenerated = !Equals(projectFile.PrimaryPsiLanguage, node.Language);
    }

    public override IEnumerable<string> VisitPsiFile(IPsiFile psiFile, PsiFmtStageContext context)
    {
      if (!myIsGenerated)
        return base.VisitPsiFile(psiFile, context);

      return base.VisitPsiFile(psiFile, context);
    }

    public override IEnumerable<string> VisitRuleDeclaration(IRuleDeclaration ruleDeclarationParam, PsiFmtStageContext context)
    {
      return base.VisitRuleDeclaration(ruleDeclarationParam, context) ?? FormattingStageUtil.GetNodesSpace(0, 0, 0, false, context, myData);
    }

    public override IEnumerable<string> VisitRuleBody(IRuleBody ruleBodyParam, PsiFmtStageContext context)
    {
      return base.VisitRuleBody(ruleBodyParam, context) ?? FormattingStageUtil.GetNodesSpace(0, 0, 0, false, context, myData);
    }

    public override IEnumerable<string> VisitOptionsDefinition(IOptionsDefinition optionsDefinitionParam, PsiFmtStageContext context)
    {
      return base.VisitOptionsDefinition(optionsDefinitionParam, context) ?? FormattingStageUtil.GetNodesSpace(0, 0, 0, false, context, myData);
    }
 
    public override IEnumerable<string> VisitExtrasDefinition(IExtrasDefinition extrasDefinitionParam, PsiFmtStageContext context)
    {
      return base.VisitExtrasDefinition(extrasDefinitionParam, context) ?? FormattingStageUtil.GetNodesSpace(0, 0, 0, false, context, myData);
    }

  }
}