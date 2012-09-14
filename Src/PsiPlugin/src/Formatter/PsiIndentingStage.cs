using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.Util;

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
      List<FormattingRange> nodePairs = context.SequentialEnumNodes().Where(p => context.CanModifyInsideNodeRange(p.First, p.Last)).ToList();
      //List<FormattingRange> nodePairs = GetNodePairs(context).ToList();
      IEnumerable<FormatResult<string>> indents = nodePairs.
        Select(range => new FormatResult<string>(range, stage.CalcIndent(new FormattingStageContext(range)))).
        Where(res => res.ResultValue != null);

      FormatterImplHelper.ForeachResult(
        indents,
        progress,
        res => res.Range.Last.MakeIndent(res.ResultValue));
    }

    private static IEnumerable<FormattingRange> GetNodePairs(CodeFormattingContext context)
    {
      ITreeNode firstNode = context.FirstNode;
      ITreeNode lastNode = context.LastNode;
      IList<FormattingRange> list = new List<FormattingRange>();
      GetNodePairs(firstNode, lastNode, list);
      return list;
    }

    private static void GetNodePairs(ITreeNode firstNode, ITreeNode lastNode, IList<FormattingRange> list)
    {
      var firstChild = firstNode;
      var lastChild = lastNode;
      var commonParent = firstNode.FindCommonParent(lastNode);
      while (firstChild != null && firstChild.Parent != commonParent)
      {
        firstChild = firstChild.Parent;
      }
      while (lastChild != null && lastChild.Parent != commonParent)
      {
        lastChild = lastChild.Parent;
      }
      Assertion.Assert(firstChild != null, "firstChild != null");
      Assertion.Assert(lastChild != null, "lastChild != null");
      var node = firstChild;
      while (node != null && node != lastChild.NextSibling)
      {
        if (!node.IsWhitespaceToken())
        {
          GetNodePairs(node, list, commonParent);
        }
        node = node.NextSibling;
      }
    }

    private static void GetNodePairs(ITreeNode treeNode, IList<FormattingRange> list, ITreeNode parent)
    {
      ITreeNode node = treeNode;
      if (node.FirstChild == null)
      {
        if (!node.IsWhitespaceToken())
        {
          ITreeNode nextNode = null;
          while ((node != null) && (nextNode == null))
          {
            var sibling = node.NextSibling;
            while (sibling != null && sibling.IsWhitespaceToken())
            {
              sibling = sibling.NextSibling;
            }
            if (sibling == null)
            {
              node = node.Parent;
              if (node == parent)
              {
                break;
              }
            }
            else
            {
              nextNode = sibling;
            }
          }
          if (nextNode != null)
          {
            list.Add(new FormattingRange(node, nextNode));
          }
        }
      }
      else
      {
        ITreeNode child = node.FirstChild;
        {
          while (child != null)
          {
            if (!child.IsWhitespaceToken())
            {
              GetNodePairs(child, list, parent);
            }
            child = child.NextSibling;
          }
        }
      }
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
