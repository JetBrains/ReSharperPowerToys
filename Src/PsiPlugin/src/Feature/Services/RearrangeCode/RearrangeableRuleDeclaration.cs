using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.RearrangeCode;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Services;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
using JetBrains.TextControl;

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services.RearrangeCode
{
  public class RearrangeableRuleDeclaration : RearrangeableElement
  {
    private readonly IRuleDeclaration myRuleDeclaration;

    public RearrangeableRuleDeclaration([NotNull] IRuleDeclaration declaration)
    {
      myRuleDeclaration = declaration;
    }

    #region Overrides of RearrangeableElement

    public override bool CanMove(Direction direction)
    {
      bool hasNewLine = false;
      if (direction == Direction.Up)
      {
        var sibling = myRuleDeclaration.PrevSibling;
        while (sibling is IWhitespaceNode)
        {
          if(sibling is NewLine)
          {
            hasNewLine = true;
          }
          sibling = sibling.PrevSibling;
        }

        var ruleDeclaration = sibling as IRuleDeclaration;

        return (hasNewLine && (ruleDeclaration != null));
      }

      if (direction == Direction.Down)
      {
        var sibling = myRuleDeclaration.NextSibling;
        while (sibling is IWhitespaceNode)
        {
          if(sibling is NewLine)
          {
            hasNewLine = true;
          }
          sibling = sibling.NextSibling;
        }

        var ruleDeclaration = sibling as IRuleDeclaration;

        return (hasNewLine && (ruleDeclaration != null));
      }

      return false;
    }

    protected override ITreeNode Move(Direction direction)
    {

      using (PsiTransactionCookie.CreateAutoCommitCookieWithCachesUpdate(myRuleDeclaration.GetPsiServices(), "Rearrange code"))
      {
        if (direction == Direction.Up)
        {
          var sibling = myRuleDeclaration.PrevSibling;
          while(sibling is IWhitespaceNode)
          {
            sibling = sibling.PrevSibling;
          }

          var ruleDeclaration = sibling as IRuleDeclaration;
          if(ruleDeclaration != null)
          {
            using (WriteLockCookie.Create())
            {
              LowLevelModificationUtil.AddChildBefore(ruleDeclaration, myRuleDeclaration);
              LowLevelModificationUtil.AddChildBefore(ruleDeclaration, new NewLine("\r\n"));
            }
          }
        }

        if (direction == Direction.Down)
        {
          var sibling = myRuleDeclaration.NextSibling;
          while (sibling is IWhitespaceNode)
          {
            sibling = sibling.NextSibling;
          }

          var ruleDeclaration = sibling as IRuleDeclaration;
          if (ruleDeclaration != null)
          {
            using (WriteLockCookie.Create())
            {
              LowLevelModificationUtil.AddChildAfter(ruleDeclaration, myRuleDeclaration);
              LowLevelModificationUtil.AddChildAfter(ruleDeclaration, new NewLine("\r\n"));
            }
          }

        }
      }
      return myRuleDeclaration;
    }

    public override string Title
    {
      get { return "rule declaration"; }
    }

    protected override ITreeNode Element
    {
      get { return myRuleDeclaration; }
    }

    public override Direction SupportedDirections
    {
      get { return Direction.Up | Direction.Down; }
    }

    #endregion
  }

  [RearrangeableElementType]
  public class Type : IRearrangeableElementType
  {

    public IRearrangeable CreateElement(ISolution solution, ITextControl textControl)
    {
      foreach (IRuleDeclaration ruleDeclaration in TextControlToPsi.GetElements<IRuleDeclaration>(solution, textControl))
      {
        return new RearrangeableRuleDeclaration(ruleDeclaration);
      }

      return null;
    }

  }
}
