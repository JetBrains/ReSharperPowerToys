using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Util
{
  internal class PsiTreeUtil
  {
    public static ITreeNode GetFirstChild<T>(ITreeNode element) where T:ITreeNode
    {
      if( element == null)
      {
        return null;
      }
      var child = element.FirstChild;
      while (child != null)
      {
        if (child is T)
        {
          return child;
        }

        var result = GetFirstChild<T>(child);

        if (result != null)
        {
          return result;
        }
        child = child.NextSibling;
      }

      return null;
    }

    public static void ReplaceChild(ITreeNode parent, ITreeNode nameNode, string name)
    {
      if (name.IsEmpty())
        throw new ArgumentException("name shouldn't be empty", "name");

      using (WriteLockCookie.Create(parent.IsPhysical()))
      {
        IRuleName ruleName = PsiElementFactory.GetInstance(parent.GetPsiModule()).CreateIdentifierExpression(name);
        LowLevelModificationUtil.ReplaceChildRange(nameNode, nameNode, ruleName);
      }
    }

    public static ICollection<T> getAllChildren<T>(ITreeNode parent)
    {
      IList<T> list = new List<T>();
      getAllChildren<T>(parent, list);
      return list;
    } 

    public static void getAllChildren<T>(ITreeNode parent, ICollection<T> collection)
    {
      ITreeNode child = parent.FirstChild;
      while (child != null)
      {
        if(child is T)
        {
          collection.Add((T)child);
        }
        getAllChildren(child,collection);
        child = child.NextSibling;
      }
    }

    public static bool EqualsElements(ITreeNode treeNode1, ITreeNode treeNode2)
    {
      string s1 = GetTextWhithoutWhitespaces(treeNode1);
      string s2 = GetTextWhithoutWhitespaces(treeNode2);
      return s1.Equals(s2);
    }

    public static string GetTextWhithoutWhitespaces(ITreeNode treeNode)
    {
      string s = "";
      if(treeNode.FirstChild == null)
      {
        if(! (treeNode is Whitespace))
        {
          s = s + treeNode.GetText();
        }
        return s;
      }
      var child = treeNode.FirstChild;
      while ( child != null)
      {
        if ( !(child is Whitespace))
        {
          s = s + GetTextWhithoutWhitespaces(child);
        }
        child = child.NextSibling;
      }
      return s;
    }
  }
}
