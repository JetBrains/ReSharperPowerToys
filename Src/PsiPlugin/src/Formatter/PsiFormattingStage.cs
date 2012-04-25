using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiFormattingStage
  {
    private readonly CodeFormattingContext myContext;
    private readonly PsiFormattingVisitor myFmtVisitor;

    private PsiFormattingStage(FormattingStageData data)
    {
      myContext = data.Context;
      myFmtVisitor = CreateFormattingVisitor(data);
    }

    private CodeFormattingContext Context
    {
      get { return myContext; }
    }

    private static PsiFormattingVisitor CreateFormattingVisitor(FormattingStageData data)
    {
      var sourceFile = data.Context.FirstNode.GetSourceFile();
      if (sourceFile != null)
      {
        var projectFileTypeServices = Shell.Instance.GetComponent<IProjectFileTypeServices>();
        var factory = projectFileTypeServices.TryGetService<IPsiCodeFormatterFactory>(sourceFile.LanguageType);
        if (factory != null)
          return factory.CreateFormattingVisitor(data);
      }

      return new PsiFormattingVisitor(data);
    }

    public static void DoFormat(FormattingStageData data, IProgressIndicator pi)
    {
      if (data.Context.FirstNode == data.Context.LastNode)
        return;

      var stage = new PsiFormattingStage(data);

      var nodePairs = GetNodePairs(data.Context);

      var spaces = nodePairs.Select(
        range => new FormatResult<IEnumerable<string>>(range, stage.CalcSpaces(new PsiFmtStageContext(range))));

      FormatterImplHelper.ForeachResult(spaces, pi, res => stage.MakeFormat(res.Range, res.ResultValue));
    }

    private static IEnumerable<FormattingRange> GetNodePairs(CodeFormattingContext context)
    {
      var firstNode = context.FirstNode;
      var lastNode = context.LastNode;
      IList<FormattingRange> list = new List<FormattingRange>();
      GetNodePairs(firstNode, lastNode, list);
      return list;
    }

    private static void GetNodePairs(ITreeNode firstNode, ITreeNode lastNode, IList<FormattingRange> list)
    {
      ITreeNode firstChild = firstNode;
      ITreeNode lastChild = lastNode;
      var commonParent = firstNode.FindCommonParent(lastNode);
      while(firstChild.Parent != commonParent)
      {
        firstChild = firstChild.Parent;
      }
      while (lastChild.Parent != commonParent)
      {
        lastChild = lastChild.Parent;
      }
      var node = firstChild;
      while(node != lastChild.NextSibling)
      {
        if (! node.IsWhitespaceToken())
        {
          GetNodePairs(node, list, commonParent);
        }
        node = node.NextSibling;
      }
    }

    private static void GetNodePairs(ITreeNode treeNode, IList<FormattingRange> list, ITreeNode parent)
    {
      var node = treeNode;
      if(node.FirstChild == null)
      {
        if(! node.IsWhitespaceToken())
        {
          ITreeNode nextNode = null;
          while(nextNode == null)
          {
            var sibling = node.NextSibling;
            while(sibling != null && sibling.IsWhitespaceToken())
            {
              sibling = sibling.NextSibling;
            }
            if(sibling == null)
            {
              node = node.Parent;
              if(node == parent)
              {
                break;
              }
            } else
            {
              nextNode = sibling;
            }
          }
          if (nextNode != null)
          {
            list.Add(new FormattingRange(node, nextNode));
          }
        }
      } else
      {
        var child = node.FirstChild;
        {
          while(child != null)
          {
            if(! child.IsWhitespaceToken())
            {
              GetNodePairs(child, list,parent);
            }
            child = child.NextSibling;
          }
        }
      }
    }

    private IEnumerable<string> CalcSpaces(PsiFmtStageContext context)
    {
      var psiTreeNode = context.Parent as IPsiTreeNode;
      return psiTreeNode != null ? psiTreeNode.Accept(myFmtVisitor, context) : null;
    }

    private void MakeFormat(FormattingRange range, IEnumerable<string> space)
    {
      PsiFormatterHelper.ReplaceSpaces(range.First, range.Last, space);

      // TODO: Move antiglueing logic into CalcSpaces()
      ITokenNode nextToken;
      var prevToken = range.First.FindLastTokenIn();
      if (prevToken != null)
        nextToken = prevToken.GetNextToken();
      else
      {
        nextToken = range.First.NextSibling.FindFirstTokenIn();
        if (nextToken != null)
          prevToken = nextToken.GetPrevToken();
      }

      if (prevToken != null && nextToken != null)
      {
        var separator = Context.CodeFormatter.GetMinimalSeparator(prevToken, nextToken);
        if (separator != null)
          LowLevelModificationUtil.AddChildAfter(range.First, separator);
      }
    }
  }
}
