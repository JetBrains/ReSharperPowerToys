using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.ResearchFormatter
{
  internal class IndentRange
  {
    private IndentRange myParent;
    private ITreeNode[] myNodes;
    private IList<IndentRange> myChildRanges = new List<IndentRange>();  

    public IndentRange(ITreeNode[] nodes)
    {
      myNodes = nodes;
      Parent = null;
    }

    public ITreeNode[] Nodes
    {
      get { return myNodes; }
    }

    public IndentRange Parent
    {
      get { return myParent; }
      set { myParent = value; }
    }

    public IEnumerable<IndentRange> ChildRanges
    {
      get { return myChildRanges; }
    }

    public void AddChildRanges(IEnumerable<IndentRange> ranges)
    {
      foreach (var indentRange in ranges)
      {
        indentRange.Parent = this;
      }
      myChildRanges.AddRange(ranges);
    }

    public bool Contains(TreeOffset offset)
    {
      var firstNode = myNodes[0];
      var lastNode = myNodes[myNodes.Length - 1];
      return ((offset.Offset >= firstNode.GetTreeTextRange().StartOffset.Offset) && (offset.Offset < lastNode.GetTreeTextRange().EndOffset.Offset));
    }
  }
}