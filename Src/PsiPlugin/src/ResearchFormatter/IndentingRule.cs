using System;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.ResearchFormatter
{
  public class IndentingRule
  {
    private Type myParentType;
    private Type myNodeType;

    public IndentingRule(Type nodeType)
    {
      myNodeType = nodeType;
      myParentType = null;
    }

    public IndentingRule(Type nodeType, Type parentType)
    {
      myNodeType = nodeType;
      myParentType = parentType;
    }

    public bool Match(ITreeNode node)
    {
      
      if(myParentType != null)
      {
        if(!(myParentType.IsInstanceOfType(node.Parent)))
        {
          return false;
        }
      }

      return (myNodeType.IsInstanceOfType(node));
    }
  }
}