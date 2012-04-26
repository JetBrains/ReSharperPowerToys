using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiIndentingStage
  {
    private readonly bool myInTypingAssist;
    private readonly PsiIndentVisitor myIndentVisitor;
    private readonly PsiIndentCache myIndentCache;

    private PsiIndentingStage(PsiCodeFormattingSettings formattingSettings, bool inTypingAssist)
    {
      myInTypingAssist = inTypingAssist;
      myIndentCache = new PsiIndentCache();
      myIndentVisitor = CreateIndentVisitor(formattingSettings, myIndentCache);
    }

    public static void DoIndent(PsiCodeFormattingSettings formattingSettings, CodeFormattingContext context, IProgressIndicator progress, bool inTypingAssist)
    {
      var stage = new PsiIndentingStage(formattingSettings, inTypingAssist);
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
        if ((!context.LeftChild.HasLineFeedsTo(rChild))&&(!myInTypingAssist))
        //if ((!context.LeftChild.HasLineFeedsTo(rChild)))
          return null;

        var psiTreeNode = context.Parent as IPsiTreeNode;

        return psiTreeNode != null
          ? psiTreeNode.Accept(myIndentVisitor, context)
          : myIndentVisitor.VisitNode(parent, context); 
    }

    [NotNull]
    private PsiIndentVisitor CreateIndentVisitor([NotNull] PsiCodeFormattingSettings formattingSettings, [NotNull] PsiIndentCache indentCache)
    {

      return new PsiIndentVisitor(indentCache);
    }
  }
}