using System;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiIndentingStage
  {
    private readonly PsiCodeFormattingSettings myFormattingSettings;
    private readonly CodeFormattingContext myContext;
    private readonly PsiIndentVisitor myIndentVisitor;
    private readonly bool myDontStickComments;

    private PsiIndentingStage(PsiCodeFormattingSettings formattingSettings, CodeFormattingContext context, bool dontStickComments)
    {
      myFormattingSettings = formattingSettings;
      myContext = context;
      myDontStickComments = dontStickComments;
    }

    public static void DoIndent(PsiCodeFormattingSettings formattingSettings, CodeFormattingContext context, bool dontStickComments, IProgressIndicator progress)
    {
      var stage = new PsiIndentingStage(formattingSettings, context, dontStickComments);
      var nodePairs = context.SequentialEnumNodes().Where(p => context.CanModifyInsideNodeRange(p.First, p.Last)).ToList();
      var indents = nodePairs.
        Select(range => new FormatResult<string>(range, stage.CalcIndent(new FormattingStageContext(range)))).
        Where(res => res.ResultValue != null);

      FormatterImplHelper.ForeachResult(
        indents,
        progress,
        res => res.Range.Last.MakeIndent(res.ResultValue));
    }

    private string CalcIndent(FormattingStageContext context)
    {
      var parent = context.Parent;

      var rChild = context.RightChild;
      if (!context.LeftChild.HasLineFeedsTo(rChild))
        return null;

      //var customIndent = myIndentCache.GetCustomIndent(rChild, CustomIndentType.DirectCalculation);
      //if (customIndent != null)
        //return customIndent;

      /*var comment = rChild as IJavaScriptCommentNode;
      if (comment != null
          && rChild.GetLineIndent(myIndentCache) == ""
          && myFormattingSettings.Other.STICK_COMMENT
          && !myDontStickComments
          && !Context.IsStickless(rChild))
        return null;*/

      /*if (rChild is IErrorElement)
        return parent.GetLineIndent(myIndentCache);*/

      // Regular processing
      var cSharpTreeNode = context.Parent as IPsiTreeNode;

      return cSharpTreeNode != null
          ? cSharpTreeNode.Accept(myIndentVisitor, context)
          : myIndentVisitor.VisitNode(parent, context);
    }
  }

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