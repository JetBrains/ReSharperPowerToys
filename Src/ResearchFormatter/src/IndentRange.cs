using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.ResearchFormatter
{
  internal class IndentRange
  {
    private IndentRange myParent;
    private ITreeNode[] myNodes;
    private readonly IndentingRule myRule;
    private string myIndent;
    private IList<IndentRange> myChildRanges = new List<IndentRange>();
    private bool myHasNewLine;

    public IndentRange(ITreeNode[] nodes, IndentingRule rule)
    {
      myNodes = nodes;
      myRule = rule;
      Parent = null;
      Indent = null;
      myHasNewLine = false;
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

    public IndentingRule Rule
    {
      get { return myRule; }
    }

    public string Indent
    {
      get { return myIndent; }
      set { myIndent = value; }
    }

    public bool HasNewLine
    {
      get { return myHasNewLine; }
      set { myHasNewLine = value; }
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
      ITreeNode lastNode = myNodes[myNodes.Length - 1];
      /*if (myRule is BoundIndentingRule)
      {
        if (Nodes.Length > 2)
        {
          lastNode = myNodes[myNodes.Length - 2];
        }
        else
        {
          return false;
        }
      }*/
      if(myRule.Inside == IndentType.Both)
      {
        if (Nodes.Length > 2)
        {
          firstNode = myNodes[1];
          lastNode = myNodes[myNodes.Length - 2];
        } else
        {
          return false;
        }
        return ((offset.Offset >= firstNode.GetTreeTextRange().StartOffset.Offset) && (offset.Offset < lastNode.GetTreeTextRange().EndOffset.Offset));
      } else if(myRule.Inside == IndentType.Right)
      {
        if (Nodes.Length > 1)
        {
          lastNode = myNodes[myNodes.Length - 2];
        }
        else
        {
          return false;
        }        
      } else if(myRule.Inside == IndentType.Left)
      {
        if (Nodes.Length > 1)
        {
          firstNode = myNodes[1];
        }
        else
        {
          return false;
        }        
      }
      var token = firstNode.GetPreviousToken();
      while((token != null) && (token.IsWhitespaceToken()))
      {
        firstNode = token;
        token = token.GetPrevToken();
      }
      return ((offset.Offset >= firstNode.GetTreeTextRange().StartOffset.Offset) && (offset.Offset < lastNode.GetTreeTextRange().EndOffset.Offset));      
    }

    public bool ContainsNewLine(TreeOffset offset)
    {
      var firstNode = myNodes[0];
      var lastNode = myNodes[myNodes.Length - 1];
      /*if(myRule.Inside == IndentType.Both)
      {
        if (Nodes.Length > 2)
        {
          firstNode = myNodes[1];
          lastNode = myNodes[myNodes.Length - 2];
        } else
        {
          return false;
        }
        return ((offset.Offset >= firstNode.GetTreeTextRange().StartOffset.Offset) && (offset.Offset < lastNode.GetTreeTextRange().EndOffset.Offset));
      } else if(myRule.Inside == IndentType.Right)
      {
        if (Nodes.Length > 1)
        {
          lastNode = myNodes[myNodes.Length - 2];
        }
        else
        {
          return false;
        }        
      } else if(myRule.Inside == IndentType.Left)
      {
        if (Nodes.Length > 1)
        {
          firstNode = myNodes[1];
        }
        else
        {
          return false;
        }        
      }*/
      var token = firstNode.GetPreviousToken();
      while((token != null) && (token.IsWhitespaceToken()))
      {
        firstNode = token;
        token = token.GetPrevToken();
      }
      return ((offset.Offset >= firstNode.GetTreeTextRange().StartOffset.Offset) && (offset.Offset < lastNode.GetTreeTextRange().EndOffset.Offset));      
    }
  }
}