using System.Collections.Generic;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiFormattingVisitor: TreeNodeVisitor<PsiFmtStageContext, IEnumerable<string>>{
    private readonly bool myIsGenerated;

    public PsiFormattingVisitor(FormattingStageData data)
    {
      var node = data.Context.FirstNode;
      var projectFile = node.GetSourceFile();
      if (projectFile != null)
        myIsGenerated = !Equals(projectFile.PrimaryPsiLanguage, node.Language);
    }

    public override IEnumerable<string> VisitPsiFile(IPsiFile psiFile, PsiFmtStageContext context)
    {
      if (!myIsGenerated)
        return base.VisitPsiFile(psiFile, context);

      return base.VisitPsiFile(psiFile, context);
    }

    public override IEnumerable<string> VisitRuleDeclaration(IRuleDeclaration ruleDeclarationParam, PsiFmtStageContext context)
    {
      if(context.LeftChild is IModifier)
      {
        return new[]{" "};
      }
      if (context.RightChild is IRoleGetterParameter)
      {
        return new[] { " " };
      }
      if (context.RightChild is IRuleBracketTypedParameters)
      {
        return new[] { " " };
      }

      return new[]{"\r\n"};
    }

    public override IEnumerable<string> VisitRuleBody(IRuleBody ruleBodyParam, PsiFmtStageContext context)
    {
      return new[] { " " };
    }
 
    public override IEnumerable<string> VisitExtrasDefinition(IExtrasDefinition extrasDefinitionParam, PsiFmtStageContext context)
    {
      return new[] { "\r\n" };
    }

    public override IEnumerable<string> VisitSequence(ISequence sequenceParam, PsiFmtStageContext context)
    {
      /*var node = context.RightChild;
      var child = sequenceParam.FirstChild;
      int length = 0;
      while(child != node)
      {
        if(! ( child is IWhitespaceNode))
        {
          if(length > MaxLineLength)
          {
            length = 0;
          } 
          length += child.GetTextLength();
        }
        child = child.NextSibling;
      }
      if (length < MaxLineLength)
      {
        return new string[] { " " };
      }
      else
      {*/
      return new[] { "\r\n" };
      //}
    }

    public override IEnumerable<string> VisitExtraDefinition(IExtraDefinition extraDefinitionParam, PsiFmtStageContext context)
    {
      return new[] { " " };
    }

    public override IEnumerable<string> VisitOptionsDefinition(IOptionsDefinition optionsDefinitionParam, PsiFmtStageContext context)
    {
      return new[] { "\r\n" };
    }

    public override IEnumerable<string> VisitPsiExpression(IPsiExpression psiExpressionParam, PsiFmtStageContext context)
    {
      if(context.RightChild is IChoiceTail)
      {
        return new[] { "\r\n" };
      }
      return new[] { " " };
    }

    public override IEnumerable<string> VisitParenExpression(IParenExpression parenExpressionParam, PsiFmtStageContext context)
    {
      if ((context.LeftChild is IPsiExpression) || (context.RightChild is IPsiExpression))
      {
        return new[] { "\r\n" };
      }
      return new[] { " " };
    }

    public override IEnumerable<string> VisitChoiceTail(IChoiceTail choiceTailParam, PsiFmtStageContext context)
    { 
      if(context.LeftChild is ICommentNode)
      {
        return new[] {"\r\n"};
      }
      return new[]{" "};
    }
  }
}