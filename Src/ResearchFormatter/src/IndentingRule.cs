using System;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.ResearchFormatter
{
  public class BoundIndentingRule : IndentingRule
  {
    private CompositeNodeType myParentType;
    private string myLeftTokenText;
    private string myRightTokenText;
    private bool myInside;

    public BoundIndentingRule(CompositeNodeType parentType, string leftTokenText, string rightTokenText, bool inside = true)
    {
      myParentType = parentType;
      myLeftTokenText = leftTokenText;
      myRightTokenText = rightTokenText;
      myInside = inside;
    }

    public override bool Inside
    {
      get { return myInside; }
    }

    public override ITreeNode Match(ITreeNode node)
    {
      var parent = node.Parent as CompositeElement;
      if(!((parent != null) && (parent.NodeType == myParentType )))
      {
        return node;
      }

      var currentNode = node;
      if(currentNode.GetText() != myLeftTokenText)
      {
        return node;
      }

      currentNode = currentNode.NextSibling;

      while((currentNode != null)){
        if(currentNode.GetText() == myRightTokenText)
        {
          return currentNode.NextSibling;
        }
        currentNode = currentNode.NextSibling;
      }

      return node;
    }
  }

  public class IndentingSimpleRule : IndentingRule
  {
    private CompositeNodeType myParentType;

    public IndentingSimpleRule(CompositeNodeType parentType)
    {
      myParentType = parentType;
    }

    #region Overrides of IndentingRule

    public override bool Inside
    {
      get { return false; }
    }

    public override ITreeNode Match(ITreeNode node)
    {
      var parent = node.Parent as CompositeElement;
      if (!((parent != null) && (parent.NodeType == myParentType)))
      {
        return node;
      }
      var currentNode = node.NextSibling;
      while (currentNode != null)
      {
        currentNode = currentNode.NextSibling;
      }
      return currentNode;
    }

    #endregion
  }

  public class AlignmentIndentingRule : IndentingRule
  {
    private NodeType myParentType;
    private string myLeftTokenText;
    private string myRightTokenText;
    private bool myInside;

    public AlignmentIndentingRule(NodeType parentType, string leftTokenText, string rightTokenText, bool inside = true)
    {
      myParentType = parentType;
      myLeftTokenText = leftTokenText;
      myRightTokenText = rightTokenText;
      myInside = inside;
    }
    #region Overrides of IndentingRule

    public override bool Inside
    {
      get { return false; }
    }

    public override ITreeNode Match(ITreeNode node)
    {
      var parent = node.Parent as CompositeElement;
      if (!((parent != null) && (parent.NodeType == myParentType)))
      {
        return node;
      }

      var currentNode = node;
      if (currentNode.GetText() != myLeftTokenText)
      {
        return node;
      }

      currentNode = currentNode.NextSibling;

      while ((currentNode != null))
      {
        if (currentNode.GetText() == myRightTokenText)
        {
          return currentNode.NextSibling;
        }
        currentNode = currentNode.NextSibling;
      }

      return node;
    }

    #endregion
  }

  public abstract class IndentingRule
  {
    public abstract bool Inside { get;  }
    public abstract ITreeNode Match(ITreeNode node);
  }
}