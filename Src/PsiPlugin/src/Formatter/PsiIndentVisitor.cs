using System.Collections.Generic;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using IWhitespaceNode = JetBrains.ReSharper.PsiPlugin.Tree.IWhitespaceNode;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiIndentVisitor : TreeNodeVisitor<FormattingStageContext, string>
  {
    private const string StandartIndent = "  ";
    private readonly IDictionary<ITreeNode, string> myCache;
    private readonly bool myInTypingAssist;
    private readonly PsiIndentCache myIndentCache;

    public PsiIndentVisitor(PsiIndentCache indentCache, bool inTypingAssist)
    {
      myIndentCache = indentCache;
      myInTypingAssist = inTypingAssist;
      myCache = new Dictionary<ITreeNode, string>();
    }

    public override string VisitExtrasDefinition(IExtrasDefinition extrasDefinitionParam, FormattingStageContext context)
    {
      string parentIndent = GetParentIndent(context.Parent);
      if (context.RightChild is IExtraDefinition)
      {
        return parentIndent + StandartIndent;
      }
      return myIndentCache.GetNodeIndent(extrasDefinitionParam);
    }

    public override string VisitOptionsDefinition(IOptionsDefinition optionsDefinitionParam, FormattingStageContext context)
    {
      string parentIndent = GetParentIndent(context.Parent);
      if (context.RightChild is IOptionDefinition)
      {
        return parentIndent + StandartIndent;
      }
      return myIndentCache.GetNodeIndent(optionsDefinitionParam);
    }

    public override string VisitRuleBody(IRuleBody ruleBodyParam, FormattingStageContext context)
    {
      string parentIndent = GetParentIndent(context.Parent);
      return parentIndent + StandartIndent;
    }

    public override string VisitRuleDeclaration(IRuleDeclaration ruleDeclarationParam, FormattingStageContext context)
    {
      string parentIndent = GetParentIndent(context.Parent);
      if (context.RightChild is IRuleBody)
      {
        return parentIndent + StandartIndent + StandartIndent;
      }
      var token = context.RightChild as ITokenNode;
      if ((token != null) && ((token.GetTokenType() == PsiTokenType.COLON) || (token.GetTokenType() == PsiTokenType.SEMICOLON)))
      {
        return parentIndent + StandartIndent;
      }
      return myIndentCache.GetNodeIndent(ruleDeclarationParam);
    }

    public override string VisitRuleDeclaredName(IRuleDeclaredName ruleDeclaredNameParam, FormattingStageContext context)
    {
      return myIndentCache.GetNodeIndent(ruleDeclaredNameParam);
    }

    public override string VisitNotChoiceExpression(INotChoiceExpression notChoiceExpressionParam, FormattingStageContext context)
    {
      return myIndentCache.GetNodeIndent(notChoiceExpressionParam);
    }

    public override string VisitPsiExpression(IPsiExpression psiExpressionParam, FormattingStageContext context)
    {
      string parentIndent = GetParentIndent(context.Parent);
      if (context.RightChild is IChoiceTail)
      {
        return parentIndent;
      }
      if (context.Parent is ParenExpression)
      {
        return parentIndent + StandartIndent;
      }
      return myIndentCache.GetNodeIndent(psiExpressionParam);
    }

    public override string VisitParenExpression(IParenExpression parenExpressionParam, FormattingStageContext context)
    {
      string parentIndent = GetParentIndent(context.Parent);
      if (context.RightChild is IPsiExpression)
      {
        return parentIndent + StandartIndent;
      }
      return parentIndent;
    }

    public override string VisitSequence(ISequence sequenceParam, FormattingStageContext context)
    {
      if (myInTypingAssist)
      {
        ITreeNode node = GetParent(context.Parent);
        string indent = GetIndentByOldParent(node);
        if (node.Parent is IParenExpression)
        {
          return VisitParenExpression(node.Parent as IParenExpression, new FormattingStageContext(new FormattingRange(node.PrevSibling, node)));
        }
        return indent;
      }
      else
      {
        return GetParentIndent(context.Parent);
      }
    }

    public override string VisitChoiceTail(IChoiceTail choiceTailParam, FormattingStageContext context)
    {
      if (context.LeftChild is ICommentNode)
      {
        return GetParentIndent(choiceTailParam) + StandartIndent;
      }
      return base.VisitChoiceTail(choiceTailParam, context);
    }

    private string GetParentIndent(ITreeNode parent)
    {
      string result = "";
      ITreeNode node = GetParent(parent);
      if (node == null)
      {
        return "";
      }
      if (myCache.ContainsKey(node))
      {
        return myCache[node];
      }
      ITreeNode oldParent = node;
      node = node.PrevSibling;
      while (node != null)
      {
        if (!(node is IWhitespaceNode))
        {
          result = "";
          break;
        }
        if (node is NewLine)
        {
          break;
        }
        result += node.GetText();
        node = node.PrevSibling;
      }
      myCache.Add(oldParent, result);
      return result;
    }

    private string GetIndentByOldParent(ITreeNode parent)
    {
      string result = "";
      ITreeNode node = parent;
      if (node == null)
      {
        return "";
      }
      if (myCache.ContainsKey(node))
      {
        return myCache[node];
      }
      ITreeNode oldParent = node;
      node = node.PrevSibling;
      while (node != null)
      {
        if (!(node is IWhitespaceNode))
        {
          result = "";
          break;
        }
        if (node is NewLine)
        {
          break;
        }
        result += node.GetText();
        node = node.PrevSibling;
      }
      myCache.Add(oldParent, result);
      return result;
    }

    private ITreeNode GetParent(ITreeNode node)
    {
      //while((node != null) && ((node.PrevSibling == null) || (node.Parent is IParenExpression) || (node.Parent is IChoiceTail)))
      //while((node != null) && ((node.PrevSibling == null)))
      while ((node != null) && ((node.PrevSibling == null) || (node.Parent is IChoiceTail)))
      {
        node = node.Parent;
      }
      return node;
    }
  }
}
