using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiIndentVisitor : TreeNodeVisitor<FormattingStageContext, string>
  {
    private PsiCodeFormattingSettings myFormattingSettings;
    private PsiIndentCache myIndentCache;
    private string myContIndent;
    private const string StandartIndent = "  ";

    public PsiIndentVisitor(PsiCodeFormattingSettings formattingSettings, PsiIndentCache indentCache)
    {
      myFormattingSettings = formattingSettings;
      myIndentCache = indentCache;
      myContIndent = formattingSettings.GlobalSettings.InsertTabs ? new string('\t', 1) : new string(' ', 1 * formattingSettings.GlobalSettings.IndentSize);
    }

    public override string VisitExtrasDefinition(IExtrasDefinition extrasDefinitionParam, FormattingStageContext context)
    {
      string parentIndent = GetParentIndent(context.Parent);
      if(context.RightChild is IExtraDefinition)
      {
        return parentIndent + StandartIndent;
      }
      return myIndentCache.GetNodeIndent(extrasDefinitionParam);
    }

    public override string VisitOptionsDefinition(IOptionsDefinition optionsDefinitionParam, FormattingStageContext context)
    {
      string parentIndent = GetParentIndent(context.Parent);
      if(context.RightChild is IOptionDefinition)
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
      if(context.RightChild is IRuleBody)
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
      return myIndentCache.GetNodeIndent(psiExpressionParam);
    }

    public override string VisitSequence(ISequence sequenceParam, FormattingStageContext context)
    {
      return GetParentIndent(context.Parent);
    }

    private string GetParentIndent(CompositeElement parent)
    {
      string result = "";
      var node = GetPrevSibling(parent);
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
      return result;
    }

    private ITreeNode GetPrevSibling(ITreeNode node)
    {
      while((node != null) && ((node.PrevSibling == null) || (node.Parent is IParenExpression) || (node.Parent is IChoiceTail)))
      {
        node = node.Parent;
      }
      if(node != null)
      {
        return node.PrevSibling;
      }
      return node;
    }
  }
}