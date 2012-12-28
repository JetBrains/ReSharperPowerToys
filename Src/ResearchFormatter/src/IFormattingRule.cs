using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.ResearchFormatter
{
  public interface IFormattingRule
  {
    IEnumerable<string> Space(FormattingStageContext context, FormattingStageResearchBase stage);
    bool Match(FormattingStageContext context);
    int GetPriority();
  }

  public class FormattingRule : IFormattingRule
  {
    private NodeType myParentType;
    private NodeType myLeftChildType;
    private NodeType myRightChildType;

    private IEnumerable<string> mySpace;

    public FormattingRule([NotNull] NodeType parent, NodeType leftChild, [NotNull]  NodeType rightChild, string space)
    {
      myParentType = parent;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = new string[]{space};
    }

    public FormattingRule([NotNull] NodeType parent, string space)
    {
      myParentType = parent;
      myLeftChildType = null;
      myRightChildType = null;
      mySpace = new string[]{space};
    }

    public FormattingRule([NotNull] NodeType leftChild, [NotNull]  NodeType rightChild, string space)
    {
      myParentType = null;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = new[] {space};
    }

    public FormattingRule([NotNull] NodeType parent, NodeType leftChild, [NotNull]  NodeType rightChild, IEnumerable<string> space)
    {
      myParentType = parent;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = space;
    }

    public FormattingRule([NotNull] NodeType parent, IEnumerable<string> space)
    {
      myParentType = parent;
      myLeftChildType = null;
      myRightChildType = null;
      mySpace = space;
    }

    public FormattingRule([NotNull] NodeType leftChild, [NotNull]  NodeType rightChild, IEnumerable<string> space)
    {
      myParentType = null;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = space;
    }

    public IEnumerable<string> Space(FormattingStageContext context, FormattingStageResearchBase stage)
    {
      return mySpace;
    }

    public bool Match(FormattingStageContext context)
    {
      if(myParentType != null)
      {
        if(context.Parent.NodeType != myParentType)
        {
          return false;
        }
      }
      if(myLeftChildType != null)
      {
        var leftChild = context.LeftChild as TreeElement;
        if(!((leftChild != null) && ( leftChild.NodeType == myLeftChildType)))
        {
          return false;
        }
      }
      if(myRightChildType!= null)
      {
        var rightChild = context.RightChild as TreeElement;
        if(!((rightChild != null) && (rightChild.NodeType == myRightChildType)))
        {
          return false;
        }
      }
      return true;
    }

    public int GetPriority()
    {
      if((myParentType!= null) && (myLeftChildType == null) && (myRightChildType == null))
      {
        return 1;
      }

      if((myParentType != null) && (myRightChildType != null) && (myLeftChildType == null))
      {
        return 3;
      }

      if((myParentType == null) && (myLeftChildType != null) && (myRightChildType != null))
      {
        return 4;
      }

      if((myParentType != null) && (myLeftChildType != null) && (myRightChildType != null))
      {
        return 5;
      }
      return 0;
    }
  }

  public class FormattingRuleAfterToken : IFormattingRule
  {
    private NodeType myParentType;
    private readonly string myTokenText;
    private IEnumerable<string> mySpace;

    #region Implementation of IFormattingRule

    public FormattingRuleAfterToken(string tokenText, string space)
    {
      myParentType = null;
      myTokenText = tokenText;
      mySpace = new[] {space};      
    }

    public FormattingRuleAfterToken([NotNull] NodeType parent, string tokenText, string space)
    {
      myParentType = parent;
      myTokenText = tokenText;
      mySpace = new[] {space};
    }

    /*public FormattingRuleAfterToken(string tokenText, IEnumerable<string> space)
    {
      myParentType = null;
      myTokenText = tokenText;
      mySpace = space;
    }

    public FormattingRuleAfterToken([NotNull] Type parent, string tokenText, IEnumerable<string> space)
    {
      myParentType = parent;
      myTokenText = tokenText;
      mySpace = space;
    }*/


    public IEnumerable<string> Space(FormattingStageContext context, FormattingStageResearchBase stage)
    {
      return mySpace;
    }

    public bool Match(FormattingStageContext context)
    {
      if(myParentType != null)
      {
        if(context.Parent.NodeType != myParentType)
        {
          return false;
        }
      }
      if(!(context.LeftChild is ITokenNode))
      {
        return false;
      }
      return context.LeftChild.GetText() == myTokenText;
    }

    public int GetPriority()
    {
      if(myParentType != null)
      {
        return 2;
      } else
      {
        return 1;
      }
    }

    #endregion
  }

  public class FormattingRuleBeforeToken : IFormattingRule
  {
    private NodeType myParentType;
    private readonly string myTokenText;
    private IEnumerable<string> mySpace;

    #region Implementation of IFormattingRule

    public FormattingRuleBeforeToken(string tokenText, string space)
    {
      myParentType = null;
      myTokenText = tokenText;
      mySpace = new[] {space};      
    }

    public FormattingRuleBeforeToken([NotNull] NodeType parent, string tokenText, string space)
    {
      myParentType = parent;
      myTokenText = tokenText;
      mySpace = new[] {space};
    }

    public IEnumerable<string> Space(FormattingStageContext context, FormattingStageResearchBase stage)
    {
      return mySpace;
    }

    public bool Match(FormattingStageContext context)
    {
      if (myParentType != null)
      {
        if (context.Parent.NodeType != myParentType)
        {
          return false;
        }
      }
      if (!(context.RightChild is ITokenNode))
      {
        return false;
      }
      return context.RightChild.GetText() == myTokenText;
    }

    public int GetPriority()
    {
      if (myParentType != null)
      {
        return 2;
      }
      else
      {
        return 1;
      }
    }

    #endregion
  }

  public class FormattingRuleBeforeNode : IFormattingRule
  {
    private readonly NodeType myType;
    private readonly NodeType myParentType;
    private readonly IEnumerable<string> mySpace;

    public FormattingRuleBeforeNode([NotNull] NodeType type, string space)
    {
      myType = type;
      myParentType = null;
      mySpace = new[] {space};
    }

    public FormattingRuleBeforeNode([NotNull] NodeType parentType, [NotNull] NodeType type, string space)
    {
      myType = type;
      myParentType = parentType;
      mySpace = new[] { space };
    }

    #region Implementation of IFormattingRule

    public IEnumerable<string> Space(FormattingStageContext context, FormattingStageResearchBase stage)
    {
      return mySpace;
    }

    public bool Match(FormattingStageContext context)
    {
      if (myParentType != null)
      {
        if (context.Parent.NodeType != myParentType)
        {
          return false;
        }
      }
      TreeElement rightChild = context.RightChild as TreeElement;
      return (rightChild.NodeType == myType);
    }

    public int GetPriority()
    {
      return 2;
    }

    #endregion
  }

  public class FormattingRuleAfterNode : IFormattingRule
  {
    private readonly NodeType myType;
    private readonly NodeType myParentType;
    private readonly IEnumerable<string> mySpace;

    public FormattingRuleAfterNode([NotNull]NodeType type, string space)
    {
      myType = type;
      myParentType = null;
      mySpace = new[] {space};
    }

    public FormattingRuleAfterNode([NotNull] NodeType parentType, [NotNull] NodeType type, string space)
    {
      myType = type;
      myParentType = parentType;
      mySpace = new[] { space };
    }

    #region Implementation of IFormattingRule

    public IEnumerable<string> Space(FormattingStageContext context, FormattingStageResearchBase stage)
    {
      return mySpace;
    }

    public bool Match(FormattingStageContext context)
    {
      var leftChild = context.LeftChild as TreeElement;
      if(myParentType != null)
      {
        if(myParentType != context.Parent.NodeType)
        {
          return false;
        }
      }
      return (leftChild.NodeType == myType);
    }

    public int GetPriority()
    {
      return 2;
    }

    #endregion
  }

  public class CustomNewLineFormattingRule : IFormattingRule
  {
    private NodeType myParentType;
    private NodeType myLeftChildType;
    private NodeType myRightChildType;
    private NodeType mySingleLineParentType;
    private readonly string myLeftText;
    private readonly string myRightText;

    private IEnumerable<string> mySpace;

    public CustomNewLineFormattingRule([NotNull] NodeType singleLineNode, string leftText, string rightText,[NotNull] NodeType parent, NodeType leftChild, [NotNull]  NodeType rightChild, string space)
    {
      mySingleLineParentType = singleLineNode;
      myLeftText = leftText;
      myRightText = rightText;
      myParentType = parent;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = new string[]{space};
    }

    public CustomNewLineFormattingRule([NotNull] NodeType singleLineNode, [NotNull] NodeType parent, string space)
    {
      mySingleLineParentType = singleLineNode;
      myParentType = parent;
      myLeftChildType = null;
      myRightChildType = null;
      mySpace = new string[]{space};
    }

    public CustomNewLineFormattingRule([NotNull] NodeType singleLineNode, string leftText, string rightText, [NotNull] NodeType leftChild, [NotNull]  NodeType rightChild, string space)
    {
      mySingleLineParentType = singleLineNode;
      myLeftText = leftText;
      myRightText = rightText;
      myParentType = null;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = new[] {space};
    }

    public CustomNewLineFormattingRule([NotNull] NodeType singleLineNode, [NotNull] NodeType parent, NodeType leftChild, [NotNull]  NodeType rightChild, IEnumerable<string> space)
    {
      mySingleLineParentType = singleLineNode;
      myParentType = parent;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = space;
    }

    public CustomNewLineFormattingRule([NotNull] NodeType singleLineNode, [NotNull] NodeType parent, IEnumerable<string> space)
    {
      mySingleLineParentType = singleLineNode;
      myParentType = parent;
      myLeftChildType = null;
      myRightChildType = null;
      mySpace = space;
    }

    public CustomNewLineFormattingRule([NotNull] NodeType singleLineNode, [NotNull] NodeType leftChild, [NotNull]  NodeType rightChild, IEnumerable<string> space)
    {
      mySingleLineParentType = singleLineNode;
      myParentType = null;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = space;
    }

    public IEnumerable<string> Space(FormattingStageContext context, FormattingStageResearchBase stage)
    {
      ITreeNode parent = context.Parent;
      while((parent as TreeElement) != null && !(mySingleLineParentType == (parent as TreeElement).NodeType))
      {
        parent = parent.Parent;
      }
      if(parent == null)
      {
        return new[]{"\n"};
      }

      ITreeNode firstToken;
      ITreeNode lastToken;
      FindFisrtAndLastTokenIn(parent, context, out firstToken, out lastToken);
      if (firstToken != null && lastToken != null)
      {
        if (stage.HasLineFeedsTo(firstToken,lastToken))
        {
          return new[] {"\n"};
        }
      }
      return mySpace;
    }

    private void FindFisrtAndLastTokenIn(ITreeNode parent, FormattingStageContext context, out ITreeNode firstToken, out ITreeNode lastToken)
    {
      var offset = context.RightChild.GetTreeStartOffset().Offset;
      var child = parent.FirstChild;
      firstToken = null;
      lastToken = null;
      while((child != null) && (child.GetTreeStartOffset().Offset <= offset))
      {
        child = child.NextSibling;
      }

      if(child != null)
      {
        child = child.PrevSibling;
      }

      var leftChild = child;
      var rightChild = child;
      while(leftChild != null)
      {
        if(leftChild.GetText() == myLeftText)
        {
          firstToken = leftChild;
          break;
        }
        leftChild = leftChild.PrevSibling;
      }

      while(rightChild != null)
      {
        if(rightChild.GetText() == myRightText)
        {
          lastToken = rightChild;
          break;
        }
        rightChild = rightChild.NextSibling;
      }

    }

    public bool Match(FormattingStageContext context)
    {

      if(myParentType != null)
      {
        if(myParentType != context.Parent.NodeType)
        {
          return false;
        }
      }
      if(myLeftChildType != null)
      {
        if(!(myLeftChildType == (context.LeftChild as TreeElement).NodeType))
        {
          return false;
        }
      }
      if(myRightChildType!= null)
      {
        if(!(myRightChildType == (context.RightChild as TreeElement).NodeType))
        {
          return false;
        }
      }


      return true;
    }

    public int GetPriority()
    {
      if((myParentType!= null) && (myLeftChildType == null) && (myRightChildType == null))
      {
        return 1;
      }

      if((myParentType != null) && (myRightChildType != null) && (myLeftChildType == null))
      {
        return 3;
      }

      if((myParentType == null) && (myLeftChildType != null) && (myRightChildType != null))
      {
        return 4;
      }

      if((myParentType != null) && (myLeftChildType != null) && (myRightChildType != null))
      {
        return 5;
      }
      return 0;
    }
  }

  public class CustomNewLineFormattingRuleAfterToken : IFormattingRule
  {
    private Type myParentType;
    private readonly string myTokenText;
    private IEnumerable<string> mySpace;
    private Type mySingleLineParentType;

    #region Implementation of IFormattingRule

    public CustomNewLineFormattingRuleAfterToken([NotNull] Type singleLineType, string tokenText, string space)
    {
      myParentType = null;
      myTokenText = tokenText;
      mySingleLineParentType = singleLineType;
      mySpace = new[] { space };
    }

    public CustomNewLineFormattingRuleAfterToken([NotNull] Type singleLineType, [NotNull] Type parent, string tokenText, string space)
    {
      myParentType = parent;
      myTokenText = tokenText;
      mySingleLineParentType = singleLineType;
      mySpace = new[] { space };
    }

    /*public FormattingRuleAfterToken(string tokenText, IEnumerable<string> space)
    {
      myParentType = null;
      myTokenText = tokenText;
      mySpace = space;
    }

    public FormattingRuleAfterToken([NotNull] Type parent, string tokenText, IEnumerable<string> space)
    {
      myParentType = parent;
      myTokenText = tokenText;
      mySpace = space;
    }*/


    public IEnumerable<string> Space(FormattingStageContext context, FormattingStageResearchBase stage)
    {
      ITreeNode parent = context.Parent;
      while (parent != null && !mySingleLineParentType.IsInstanceOfType(parent))
      {
        parent = parent.Parent;
      }
      if (parent == null)
      {
        return new[] { "\n" };
      }
      if (stage.HasLineFeedsTo(parent.FirstChild, parent.LastChild))
      {
        return new[] { "\n" };
      }
      return mySpace;
    }

    public bool Match(FormattingStageContext context)
    {
      if (myParentType != null)
      {
        if (!(myParentType.IsInstanceOfType(context.Parent)))
        {
          return false;
        }
      }
      if (!(context.LeftChild is ITokenNode))
      {
        return false;
      }
      return context.LeftChild.GetText() == myTokenText;
    }

    public int GetPriority()
    {
      if (myParentType != null)
      {
        return 2;
      }
      else
      {
        return 1;
      }
    }

    #endregion
  }

  public class CustomNewLineFormattingRuleBeforeToken : IFormattingRule
  {
    private Type myParentType;
    private readonly string myTokenText;
    private IEnumerable<string> mySpace;
    private Type mySingleLineParentType;

    #region Implementation of IFormattingRule

    public CustomNewLineFormattingRuleBeforeToken([NotNull] Type singleLineType, string tokenText, string space)
    {
      myParentType = null;
      myTokenText = tokenText;
      mySingleLineParentType = singleLineType;
      mySpace = new[] { space };
    }

    public CustomNewLineFormattingRuleBeforeToken([NotNull] Type singleLineType, [NotNull] Type parent, string tokenText, string space)
    {
      myParentType = parent;
      myTokenText = tokenText;
      mySingleLineParentType = singleLineType;
      mySpace = new[] { space };
    }

    public IEnumerable<string> Space(FormattingStageContext context, FormattingStageResearchBase stage)
    {
      ITreeNode parent = context.Parent;
      while (parent != null && !mySingleLineParentType.IsInstanceOfType(parent))
      {
        parent = parent.Parent;
      }
      if (parent == null)
      {
        return new[] { "\n" };
      }
      if (stage.HasLineFeedsTo(parent.FirstChild, parent.LastChild))
      {
        return new[] { "\n" };
      }
      return mySpace;
    }

    public bool Match(FormattingStageContext context)
    {
      if (myParentType != null)
      {
        if (!(myParentType.IsInstanceOfType(context.Parent)))
        {
          return false;
        }
      }
      if (!(context.RightChild is ITokenNode))
      {
        return false;
      }
      return context.RightChild.GetText() == myTokenText;
    }

    public int GetPriority()
    {
      if (myParentType != null)
      {
        return 2;
      }
      else
      {
        return 1;
      }
    }

    #endregion
  }
}