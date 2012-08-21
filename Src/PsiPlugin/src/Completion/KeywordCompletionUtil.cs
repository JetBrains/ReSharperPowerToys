using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Completion
{
  internal static class KeywordCompletionUtil
  {
    public static IEnumerable<string> GetAplicableKeywords(IPsiFile file, TreeTextRange referenceRange)
    {
      // statements part ...
      //var ret = new HashSet<string>();
      IList<string> list = new List<string>();
      var token = file.FindNodeAt(referenceRange) as ITokenNode;
      if (token == null)
      {
        return list;
      }

      ITreeNode currentNode;
      ITreeNode child;

      if (token.GetTokenType() == PsiTokenType.IDENTIFIER && token.Parent != null)
      {
        currentNode = token.Parent.Parent;
        child = token.Parent;
      }
      else
      {
        currentNode = token.Parent;
        child = token;
      }

      if (currentNode is IRuleDeclaration)
      {
        if (!IsInRuleBody(child))
        {
          if (IsAfterName(child))
          {
            list.Add("extras");
            list.Add("options");
          }
          if (IsBeforeName(child))
          {
            list.Add("abstract");
            list.Add("errorhandling");
            list.Add("interface");
            list.Add("private");
          }
          if (HasNoName(child))
          {
            list.Add("interfaces");
            list.Add("paths");
          }
        }
        else
        {
          list.Add("null");
          list.Add("LIST");
          list.Add("SEP");
        }
      }
      else if (currentNode is IExtrasDefinition)
      {
        list.Add("get");
        list.Add("returnType");
        list.Add("isCashed");
      }
      else if (currentNode is ISimpleExpression)
      {
        list.Add("null");
        list.Add("LIST");
        list.Add("SEP");
      }
      else if (currentNode is IListExpression)
      {
        list.Add("SEP");
      }
      else if (currentNode is IRuleParameters)
      {
        list.Add("cashed");
        list.Add("isCashed");
        list.Add("ROLE");
        list.Add("getter");
      }
      return list;
    }

    private static bool HasNoName(ITreeNode child)
    {
      return (!IsAfterName(child) && !IsBeforeName(child));
    }

    private static bool IsBeforeName(ITreeNode child)
    {
      ITreeNode sibling = child;
      while (sibling != null)
      {
        if (sibling is IRuleDeclaredName)
        {
          return true;
        }
        sibling = sibling.NextSibling;
      }

      return false;
    }

    private static bool IsAfterName(ITreeNode child)
    {
      ITreeNode sibling = child;
      while (sibling != null)
      {
        if (sibling is IRuleDeclaredName)
        {
          return true;
        }
        sibling = sibling.PrevSibling;
      }

      return false;
    }

    private static bool IsInRuleBody(ITreeNode child)
    {
      bool hasColon = false;
      bool hasSemicolon = false;

      ITreeNode sibling = child;
      while (sibling != null)
      {
        if (sibling is ITokenNode)
        {
          if ((sibling as ITokenNode).GetTokenType() == PsiTokenType.SEMICOLON)
          {
            hasSemicolon = true;
          }
        }

        sibling = sibling.NextSibling;
      }

      sibling = child;
      while (sibling != null)
      {
        if (sibling is ITokenNode)
        {
          if ((sibling as ITokenNode).GetTokenType() == PsiTokenType.COLON)
          {
            hasColon = true;
          }
        }

        sibling = sibling.PrevSibling;
      }

      return hasColon && hasSemicolon;
    }
  }
}
