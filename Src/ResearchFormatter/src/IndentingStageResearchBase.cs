﻿using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.ResearchFormatter
{
  public abstract class IndentingStageResearchBase
  {
    private string StandardIndent = "  ";
    protected readonly FormatterResearchBase Formatter;

    public IndentingStageResearchBase(FormatterResearchBase formatter)
    {
      Formatter = formatter;
    }

    public void DoIndent(CodeFormattingContext context, IProgressIndicator progress, bool indent = false)
    {
      var indentRanges = BuildRanges(context);
      var nodePairs = context.SequentialEnumNodes().Where(p => context.CanModifyInsideNodeRange(p.First, p.Last)).Where(p => HasLineFeedsTo(p.First, p.Last)).ToList();
      var parent = context.FirstNode.FindCommonParent(context.LastNode);
      CollectNewLines(indentRanges);
      CalcIndents(indentRanges, GetIndent(parent));
      var indents = nodePairs.
        Select(range => new FormatResult<string>(range, CalcIndent(new FormattingStageContext(range), indentRanges, indent))).
        Where(res => res.ResultValue != null);

      FormatterImplHelper.ForeachResult(
        indents,
        progress,
        res => MakeIndent(res.Range.Last, res.ResultValue));
    }

    private void CollectNewLines(IList<IndentRange> indentRanges)
    {
      foreach (var indentRange in indentRanges)
      {
        CollectNewLines(indentRange);
      }
    }

    private string CalcIndent(FormattingStageContext formattingStageContext, IList<IndentRange> indentRanges, bool indent)
    {
      ITreeNode rChild = formattingStageContext.RightChild;
      if ((!HasLineFeedsTo(formattingStageContext.LeftChild, rChild)))
      {
        return null;
      }
      var offset = formattingStageContext.RightChild.GetTreeStartOffset();

      var range = GetCurrentRangeForInsert(offset, indentRanges);

      if(range == null)
      {
        var parent = formattingStageContext.LeftChild.FindCommonParent(formattingStageContext.RightChild);
        return GetIndent(parent);
      }
      else
      {
        return range.Indent;
      }
    }

    private IndentRange GetCurrentRangeForInsert(TreeOffset offset, IEnumerable<IndentRange> indentRanges)
    {
      IndentRange range = null;
      foreach (var indentRange in indentRanges)
      {
        if (indentRange.Contains(offset))
        {
          range = GetCurrentRangeForInsert(offset, indentRange);
        }
      }
      return range;
    }

    private IndentRange GetCurrentRangeForInsert(TreeOffset offset, IndentRange indentRange)
    {
      if (indentRange.ChildRanges.Count() == 0)
      {
        return indentRange;
      }
      var range = GetCurrentRangeForInsert(offset, indentRange.ChildRanges);
      if (range == null)
      {
        return indentRange;
      }
      return range;
    }

    private IndentRange GetCurrentRange(TreeOffset offset, IEnumerable<IndentRange> indentRanges)
    {
      IndentRange range = null;
      foreach (var indentRange in indentRanges)
      {
        if (indentRange.ContainsNewLine(offset))
        {
          range = GetCurrentRange(offset, indentRange);
        }
      }
      return range;
    }

    private IndentRange GetCurrentRange(TreeOffset offset, IndentRange indentRange)
    {
      if(indentRange.ChildRanges.Count() == 0)
      {
        return indentRange;
      }
      var range = GetCurrentRange(offset, indentRange.ChildRanges);
      if(range == null)
      {
        return indentRange;
      }
      return range;
    }

    private void CalcIndents(IList<IndentRange> indentRanges, string parentIndent = "")
    {
      foreach (var indentRange in indentRanges)
      {
        CalcIndents(indentRange, parentIndent);
      }      
    }

    private void CalcIndents(IndentRange indentRange, string parentIndent = "")
    {
      if(indentRange.HasNewLine)
      {
        if (indentRange.Rule is AlignmentIndentingRule)
        {
          int length = GetLineLength(indentRange.Nodes[0]);
          indentRange.Indent = parentIndent;
          for(int i = 0; i < length; ++i)
          {
            indentRange.Indent += " ";
          }
        }
        else
        {
          indentRange.Indent = parentIndent + StandardIndent;
        }
      } else
      {
        indentRange.Indent = parentIndent;
      }

      foreach (var range in indentRange.ChildRanges)
      {
        CalcIndents(range, indentRange.Indent);
      }
    }

    private int GetLineLength(ITreeNode treeNode)
    {
      var token = treeNode.GetFirstTokenIn();
      int result = 0;
      token = token.GetPrevToken();
      while((token != null) && ( token.GetTokenType() != NewLineType))
      {
        result += token.GetTextLength();
        token = token.GetPrevToken();
      }
      return result;
    }

    private void CollectNewLines(IndentRange indentRange)
    {
      IList<TreeOffset> newLineOffsets = new List<TreeOffset>();
      var token = indentRange.Nodes[0].GetFirstTokenIn();
      var prevToken = token.GetPrevToken();
      while ((prevToken != null) && (prevToken.IsWhitespaceToken()))
      {
        if (prevToken.GetTokenType() == NewLineType)
        {
          newLineOffsets.Add(prevToken.GetTreeEndOffset());
          break;
        }
        prevToken = prevToken.GetPrevToken();
      }
      while ((token != null) && (indentRange.ContainsNewLine(token.GetTreeEndOffset())))
      {
        if (token.GetTokenType() == NewLineType)
        {
          newLineOffsets.Add(token.GetTreeEndOffset());
          var range = GetCurrentRange(token.GetTreeEndOffset(), indentRange);
          range.HasNewLine = true;
        }
        token = token.GetNextToken();
      }
    }


    private IList<IndentRange> BuildRanges(CodeFormattingContext context)
    {
      var parent = context.FirstNode.FindCommonParent(context.LastNode);
      return BuildRanges(parent);
    }

    private IList<IndentRange> BuildRanges(ITreeNode node)
    {
      IList<IndentRange> ranges = new List<IndentRange>();

      var child = node.FirstChild;
      while(child != null)
      {
        bool match = false;
        foreach (var rule in Formatter.IndentingRules)
        {
          var nextChild = Match(child, rule);
          if(nextChild != child)
          {
            IList<ITreeNode> nodes = new List<ITreeNode>();
            var tempNode = child;
            while (tempNode != nextChild)
            {
              nodes.Add(tempNode);
              tempNode = tempNode.NextSibling;
            }
            var range = new IndentRange(nodes.ToArray(), rule);
            foreach (var treeNode in range.Nodes)
            {
              var childRanges = BuildRanges(treeNode);
              range.AddChildRanges(childRanges);
            }
            ranges.Add(range);
            child = nextChild;
            match = true;
            break;
          }
        }
        if(! match)
        {
          var childRange = BuildRanges(child);
          ranges.AddRange(childRange);
          child = child.NextSibling;
        }
      }
      return ranges;
    }

    private ITreeNode Match(ITreeNode child, IndentingRule rule)
    {
      return rule.Match(child);
    }

    private string GetIndent(ITreeNode treeNode)
    {
      string s = "";
      var token = treeNode.GetPreviousToken();
      while ((token != null) && (token.GetTokenType() != NewLineType))
      {
        token = token.GetPrevToken();
      }
      if (token == null)
      {
        return s;
      }
      token = token.GetNextToken();
      while ((token != null) && (token.GetTokenType() == WhiteSpaceType))
      {
        s = s + token.GetText();
        token = token.GetNextToken();
      }
      return s;
    }

    public void MakeIndent(ITreeNode indentNode, string indent)
    {
      var lastSpace = AsWhitespaceNode(indentNode.PrevSibling);
      if (lastSpace != null && lastSpace.GetTokenType() != NewLineType)
      {
        var firstSpace = LeftWhitespaces(lastSpace).TakeWhile(ws => ws.GetTokenType() != NewLineType).LastOrDefault() ?? lastSpace;

        if (firstSpace != lastSpace || lastSpace.GetText() != indent)
          if (indent.IsEmpty())
            LowLevelModificationUtil.DeleteChildRange(firstSpace, lastSpace);
          else
            LowLevelModificationUtil.ReplaceChildRange(firstSpace, lastSpace, CreateSpace(indent));
      }
      else if (!indent.IsEmpty())
        LowLevelModificationUtil.AddChildBefore(indentNode, CreateSpace(indent));
    }

    protected abstract ITreeNode[] CreateSpace(string indent);

    public virtual TokenNodeType NewLineType { get; private set; }
    public virtual TokenNodeType WhiteSpaceType { get; private set; }
    public abstract ITreeNode AsWhitespaceNode(ITreeNode node);


    private IEnumerable<ITreeNode> LeftWhitespaces(ITreeNode node)
    {
      return node.LeftSiblings().Select(sib => AsWhitespaceNode(sib)).TakeWhile(sib => sib != null);
    }

    public bool HasLineFeedsTo(ITreeNode fromNode, ITreeNode toNode)
    {
      return GetLineFeedsTo(fromNode, toNode).Any();
    }

    private IEnumerable<ITreeNode> GetLineFeedsTo(ITreeNode fromNode, ITreeNode toNode)
    {
      return fromNode.GetWhitespacesTo(toNode).Where(wsNode => (wsNode.GetTokenType() == NewLineType) && (AsWhitespaceNode(wsNode) != null));
    }
  }
}
