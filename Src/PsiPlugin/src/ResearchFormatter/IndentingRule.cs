using System;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.ResearchFormatter.Psi;

namespace JetBrains.ReSharper.PsiPlugin.ResearchFormatter
{
  public class BoundIndentingRule : IndentingRule
  {
    private Type myParentType;
    private string myLeftTokenText;
    private string myRightTokenText;
    private bool myInside;

    public BoundIndentingRule(Type parentType, string leftTokenText, string rightTokenText, bool inside = true)
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
      if(!myParentType.IsInstanceOfType(node.Parent))
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
    private Type myParentType;

    public IndentingSimpleRule(Type parentType)
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
      if (!myParentType.IsInstanceOfType(node.Parent))
      {
        return node;
      }
      var currentNode = node.NextSibling;
      while((currentNode != null) && (myParentType.IsInstanceOfType(currentNode.Parent)))
      {
        currentNode = currentNode.NextSibling;
      }
      return currentNode;
    }

    #endregion
  }

  public abstract class IndentingRule
  {
    public abstract bool Inside { get;  }
    public abstract ITreeNode Match(ITreeNode node);
  }
}