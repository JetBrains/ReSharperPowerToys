using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
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
    private PsiIndentCache myIndentCache;

    private PsiIndentingStage(PsiCodeFormattingSettings formattingSettings, CodeFormattingContext context, bool dontStickComments)
    {
      myFormattingSettings = formattingSettings;
      myContext = context;
      myDontStickComments = dontStickComments;
      myIndentCache = new PsiIndentCache();
      myIndentVisitor = CreateIndentVisitor(context, formattingSettings, myIndentCache);
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

      /*if (rChild is IErrorElement)
        return parent.GetLineIndent(myIndentCache);*/

      // Regular processing
      var psiTreeNode = context.Parent as IPsiTreeNode;

      return psiTreeNode != null
          ? psiTreeNode.Accept(myIndentVisitor, context)
          : myIndentVisitor.VisitNode(parent, context);
    }

    [NotNull]
    protected PsiIndentVisitor CreateIndentVisitor(CodeFormattingContext context, [NotNull] PsiCodeFormattingSettings formattingSettings, [NotNull] PsiIndentCache indentCache)
    {
      var sourceFile = context.FirstNode.GetSourceFile();


      return new PsiIndentVisitor(formattingSettings, indentCache);
    }
  }
}