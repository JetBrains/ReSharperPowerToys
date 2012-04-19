using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiFormattingVisitor: Tree.TreeNodeVisitor<PsiFmtStageContext, IEnumerable<string>>{
    [NotNull] private readonly FormattingStageData myData;

    private readonly bool myIsGenerated;

    private const int MaxLineLength = 50;

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
      return new string[]{"\r\n"};
    }

    public override IEnumerable<string> VisitRuleBody(IRuleBody ruleBodyParam, PsiFmtStageContext context)
    {
      return FormattingStageUtil.GetNodesSpace(1, 1, 1, false, context, myData);
    }

    public override IEnumerable<string> VisitOptionsDefinition(IOptionsDefinition optionsDefinitionParam, PsiFmtStageContext context)
    {
      return FormattingStageUtil.GetNodesSpace(1, 1, 1, false, context, myData);
    }
 
    public override IEnumerable<string> VisitExtrasDefinition(IExtrasDefinition extrasDefinitionParam, PsiFmtStageContext context)
    {
      return FormattingStageUtil.GetNodesSpace(1, 1, 1, false, context, myData);
    }

    public override IEnumerable<string> VisitRuleDeclaredName(IRuleDeclaredName ruleDeclaredNameParam, PsiFmtStageContext context)
    {
      return FormattingStageUtil.GetNodesSpace(1, 1, 1, false, context, myData);
    }

    public override IEnumerable<string> VisitSequence(ISequence sequenceParam, PsiFmtStageContext context)
    {
      var node = context.RightChild;
      var child = sequenceParam.FirstChild;
      int length = 0;
      while(child != node)
      {
        if(! ( child is IWhitespaceNode))
        {
          if(length > MaxLineLength)
          {
            length = 0;
          } 
          length += child.GetTextLength();
        }
        child = child.NextSibling;
      }
      if (length < MaxLineLength)
      {
        return new string[] { " " };
      }
      else
      {
        return new string[] {"\r\n"};
      }
    }

    public override IEnumerable<string> VisitExtraDefinition(IExtraDefinition extraDefinitionParam, PsiFmtStageContext context)
    {
      return new string[] { " " };
    }
  }
}