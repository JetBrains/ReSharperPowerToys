using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.ResearchFormatter
{
  public interface IFormattingRule
  {
    IEnumerable<string> Space { get;}
    bool Match(FormattingStageContext context);
    int GetPriority();
  }

  public class FormattingRule : IFormattingRule
  {
    private Type myParentType;
    private Type myLeftChildType;
    private Type myRightChildType;

    private IEnumerable<string> mySpace;

    public FormattingRule([NotNull] Type parent, Type leftChild, [NotNull]  Type rightChild, string space)
    {
      myParentType = parent;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = new string[]{space};
    }

    public FormattingRule([NotNull] Type parent, string space)
    {
      myParentType = parent;
      myLeftChildType = null;
      myRightChildType = null;
      mySpace = new string[]{space};
    }

    public FormattingRule([NotNull] Type leftChild, [NotNull]  Type rightChild, string space)
    {
      myParentType = null;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = new[] {space};
    }

    public FormattingRule([NotNull] Type parent, Type leftChild, [NotNull]  Type rightChild, IEnumerable<string> space)
    {
      myParentType = parent;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = space;
    }

    public FormattingRule([NotNull] Type parent, IEnumerable<string> space)
    {
      myParentType = parent;
      myLeftChildType = null;
      myRightChildType = null;
      mySpace = space;
    }

    public FormattingRule([NotNull] Type leftChild, [NotNull]  Type rightChild, IEnumerable<string> space)
    {
      myParentType = null;
      myLeftChildType = leftChild;
      myRightChildType = rightChild;
      mySpace = space;
    }

    public IEnumerable<string> Space
    {
      get { return mySpace; }
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

  public class FormattingRuleAfterToken : IFormattingRule
  {
    private Type myParentType;
    private readonly string myTokenText;
    private IEnumerable<string> mySpace;

    #region Implementation of IFormattingRule

    public FormattingRuleAfterToken(string tokenText, string space)
    {
      myParentType = null;
      myTokenText = tokenText;
      mySpace = new[] {space};      
    }

    public FormattingRuleAfterToken([NotNull] Type parent, string tokenText, string space)
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

    public IEnumerable<string> Space
    {
      get { return mySpace; }
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
    private Type myParentType;
    private readonly string myTokenText;
    private IEnumerable<string> mySpace;

    #region Implementation of IFormattingRule

    public FormattingRuleBeforeToken(string tokenText, string space)
    {
      myParentType = null;
      myTokenText = tokenText;
      mySpace = new[] {space};      
    }

    public FormattingRuleBeforeToken([NotNull] Type parent, string tokenText, string space)
    {
      myParentType = parent;
      myTokenText = tokenText;
      mySpace = new[] {space};
    }

    public IEnumerable<string> Space
    {
      get { return mySpace; }
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

  public class FormattingRuleBeforeNode : IFormattingRule
  {
    private readonly Type myType;
    private readonly IEnumerable<string> mySpace;

    public FormattingRuleBeforeNode([NotNull] Type type, string space)
    {
      myType = type;
      mySpace = new[] {space};
    }

    #region Implementation of IFormattingRule

    public IEnumerable<string> Space
    {
      get { return mySpace; }
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

    public IEnumerable<string> Space
    {
      get { return mySpace; }
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
  }
}