using System;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.ResearchFormatter
{
  public class IndentingRule
  {
    private Type myParentType;
    private string myLeftTokenText;
    private string myRightTokenText;
    private bool myInside;

    public IndentingRule(Type parentType, string leftTokenText, string rightTokenText, bool inside = true)
    {
      myParentType = parentType;
      myLeftTokenText = leftTokenText;
      myRightTokenText = rightTokenText;
      myInside = inside;
    }

    public bool Inside
    {
      get { return myInside; }
    }

    public ITreeNode Match(ITreeNode node)
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
}