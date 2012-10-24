using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiIndentingStage
  {
    private readonly bool myInTypingAssist;
    private static PsiIndentVisitor _indentVisitor;

    private PsiIndentingStage(bool inTypingAssist = false)
    {
      myInTypingAssist = inTypingAssist;
    }

    public static void DoIndent(CodeFormattingContext context, IProgressIndicator progress, bool inTypingAssist)
    {
      var indentCache = new PsiIndentCache();
      _indentVisitor = CreateIndentVisitor(indentCache, inTypingAssist);
      var stage = new PsiIndentingStage(inTypingAssist);
      //List<FormattingRange> nodePairs = context.SequentialEnumNodes().Where(p => context.CanModifyInsideNodeRange(p.First, p.Last)).ToList();
      List<FormattingRange> nodePairs = context.GetNodePairs().Where(p => context.CanModifyInsideNodeRange(p.First, p.Last)).ToList();
      IEnumerable<FormatResult<string>> indents = nodePairs.
        Select(range => new FormatResult<string>(range, stage.CalcIndent(new FormattingStageContext(range)))).
        Where(res => res.ResultValue != null);

      FormatterImplHelper.ForeachResult(
        indents,
        progress,
        res => res.Range.Last.MakeIndent(res.ResultValue));
    }

    private string CalcIndent(FormattingStageContext context)
    {
      CompositeElement parent = context.Parent;

      ITreeNode rChild = context.RightChild;
      if ((!context.LeftChild.HasLineFeedsTo(rChild)) && (!myInTypingAssist))
      {
        return null;
      }

      var psiTreeNode = context.Parent as IPsiTreeNode;

      return psiTreeNode != null
        ? psiTreeNode.Accept(_indentVisitor, context)
        : _indentVisitor.VisitNode(parent, context);
    }

    [NotNull]
    private static PsiIndentVisitor CreateIndentVisitor([NotNull] PsiIndentCache indentCache, bool inTypingAssist)
    {
      return new PsiIndentVisitor(indentCache, inTypingAssist);
    }
  }
}
