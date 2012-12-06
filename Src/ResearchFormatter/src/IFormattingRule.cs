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
    private CompositeNodeType myParentType;
    private CompositeNodeType myLeftChildType;
    private CompositeNodeType myRightChildType;

    private IEnumerable<string> mySpace;

    public FormattingRule([NotNull] CompositeNodeType parent, CompositeNodeType leftChild, [NotNull]  CompositeNodeType rightChild, string space)
    {
      myParentType = parent;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = new string[]{space};
    }

    public FormattingRule([NotNull] CompositeNodeType parent, string space)
    {
      myParentType = parent;
      myLeftChildType = null;
      myRightChildType = null;
      mySpace = new string[]{space};
    }

    public FormattingRule([NotNull] CompositeNodeType leftChild, [NotNull]  CompositeNodeType rightChild, string space)
    {
      myParentType = null;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = new[] {space};
    }

    public FormattingRule([NotNull] CompositeNodeType parent, CompositeNodeType leftChild, [NotNull]  CompositeNodeType rightChild, IEnumerable<string> space)
    {
      myParentType = parent;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = space;
    }

    public FormattingRule([NotNull] CompositeNodeType parent, IEnumerable<string> space)
    {
      myParentType = parent;
      myLeftChildType = null;
      myRightChildType = null;
      mySpace = space;
    }

    public FormattingRule([NotNull] CompositeNodeType leftChild, [NotNull]  CompositeNodeType rightChild, IEnumerable<string> space)
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
        var leftChild = context.LeftChild as CompositeElement;
        if(!((leftChild != null) && ( leftChild.NodeType == myLeftChildType)))
        {
          return false;
        }
      }
      if(myRightChildType!= null)
      {
        var rightChild = context.RightChild as CompositeElement;
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
    private CompositeNodeType myParentType;
    private readonly string myTokenText;
    private IEnumerable<string> mySpace;

    #region Implementation of IFormattingRule

    public FormattingRuleAfterToken(string tokenText, string space)
    {
      myParentType = null;
      myTokenText = tokenText;
      mySpace = new[] {space};      
    }

    public FormattingRuleAfterToken([NotNull] CompositeNodeType parent, string tokenText, string space)
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
    private CompositeNodeType myParentType;
    private readonly string myTokenText;
    private IEnumerable<string> mySpace;

    #region Implementation of IFormattingRule

    public FormattingRuleBeforeToken(string tokenText, string space)
    {
      myParentType = null;
      myTokenText = tokenText;
      mySpace = new[] {space};      
    }

    public FormattingRuleBeforeToken([NotNull] CompositeNodeType parent, string tokenText, string space)
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

  /*public class FormattingRuleBeforeNode : IFormattingRule
  {
    private readonly Type myType;
    private readonly IEnumerable<string> mySpace;

    public FormattingRuleBeforeNode([NotNull] Type type, string space)
    {
      myType = type;
      mySpace = new[] {space};
    }

    #region Implementation of IFormattingRule

    public IEnumerable<string> Space(FormattingStageContext context, FormattingStageResearchBase stage)
    {
      return mySpace;
    }

    public bool Match(FormattingStageContext context)
    {
      return myType.IsInstanceOfType(context.RightChild);
    }

    public int GetPriority()
    {
      return 2;
    }

    #endregion
  }

  public class FormattingRuleAfterNode : IFormattingRule
  {
    private readonly Type myType;
    private readonly IEnumerable<string> mySpace;

    public FormattingRuleAfterNode([NotNull] Type type, string space)
    {
      myType = type;
      mySpace = new[] {space};
    }

    #region Implementation of IFormattingRule

    public IEnumerable<string> Space(FormattingStageContext context, FormattingStageResearchBase stage)
    {
      return mySpace;
    }

    public bool Match(FormattingStageContext context)
    {
      return myType.IsInstanceOfType(context.LeftChild);
    }

    public int GetPriority()
    {
      return 2;
    }

    #endregion
  }*/

  public class CustomNewLineFormattingRule : IFormattingRule
  {
    private Type myParentType;
    private Type myLeftChildType;
    private Type myRightChildType;
    private Type mySingleLineParentType;

    private IEnumerable<string> mySpace;

    public CustomNewLineFormattingRule([NotNull] Type singleLineNode,[NotNull] Type parent, Type leftChild, [NotNull]  Type rightChild, string space)
    {
      mySingleLineParentType = singleLineNode;
      myParentType = parent;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = new string[]{space};
    }

    public CustomNewLineFormattingRule([NotNull] Type singleLineNode, [NotNull] Type parent, string space)
    {
      mySingleLineParentType = singleLineNode;
      myParentType = parent;
      myLeftChildType = null;
      myRightChildType = null;
      mySpace = new string[]{space};
    }

    public CustomNewLineFormattingRule([NotNull] Type singleLineNode, [NotNull] Type leftChild, [NotNull]  Type rightChild, string space)
    {
      mySingleLineParentType = singleLineNode;
      myParentType = null;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = new[] {space};
    }

    public CustomNewLineFormattingRule([NotNull] Type singleLineNode, [NotNull] Type parent, Type leftChild, [NotNull]  Type rightChild, IEnumerable<string> space)
    {
      mySingleLineParentType = singleLineNode;
      myParentType = parent;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = space;
    }

    public CustomNewLineFormattingRule([NotNull] Type singleLineNode, [NotNull] Type parent, IEnumerable<string> space)
    {
      mySingleLineParentType = singleLineNode;
      myParentType = parent;
      myLeftChildType = null;
      myRightChildType = null;
      mySpace = space;
    }

    public CustomNewLineFormattingRule([NotNull] Type singleLineNode, [NotNull] Type leftChild, [NotNull]  Type rightChild, IEnumerable<string> space)
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
      while(parent != null && !mySingleLineParentType.IsInstanceOfType(parent))
      {
        parent = parent.Parent;
      }
      if(parent == null)
      {
        return new[]{"\n"};
      }
      if(stage.HasLineFeedsTo(parent.FirstChild, parent.LastChild))
      {
        return new[] {"\n"};
      }
      return mySpace;
    }

    public bool Match(FormattingStageContext context)
    {

      if(myParentType != null)
      {
        if(!(myParentType.IsInstanceOfType(context.Parent)))
        {
          return false;
        }
      }
      if(myLeftChildType != null)
      {
        if(!(myLeftChildType.IsInstanceOfType(context.LeftChild)))
        {
          return false;
        }
      }
      if(myRightChildType!= null)
      {
        if(!(myRightChildType.IsInstanceOfType(context.RightChild)))
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