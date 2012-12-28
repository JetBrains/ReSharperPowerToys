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
    private readonly IndentType myIndentType;

    public BoundIndentingRule(CompositeNodeType parentType, string leftTokenText, string rightTokenText, IndentType indentType = IndentType.None)
    {
      myParentType = parentType;
      myLeftTokenText = leftTokenText;
      myRightTokenText = rightTokenText;
      myIndentType = indentType;
    }

    public override IndentType Inside
    {
      get { return myIndentType; }
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
    private readonly IndentType myIndentType;

    public IndentingSimpleRule(CompositeNodeType parentType, IndentType indentType = IndentType.None)
    {
      myParentType = parentType;
      myIndentType = indentType;
    }

    #region Overrides of IndentingRule

    public override IndentType Inside
    {
      get { return myIndentType; }
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

    public AlignmentIndentingRule(NodeType parentType, string leftTokenText, string rightTokenText)
    {
      myParentType = parentType;
      myLeftTokenText = leftTokenText;
      myRightTokenText = rightTokenText;
    }
    #region Overrides of IndentingRule

    public override IndentType Inside
    {
      get { return IndentType.None; }
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
    public abstract IndentType Inside { get;  }
    public abstract ITreeNode Match(ITreeNode node);
  }

  public enum IndentType
  {
    Left,
    Right,
    Both,
    None
  }
}