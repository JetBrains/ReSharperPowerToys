using JetBrains.Application;
using JetBrains.ReSharper.Feature.Services.Intentions.DataProviders;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.Intentions
{
  internal class PsiRuleInserter
  {
    private readonly IRuleDeclaration myDeclarationToAdd;
    private readonly ICreationTarget myTarget;

    public PsiRuleInserter(IRuleDeclaration declarationToAdd, ICreationTarget target)
    {
      myDeclarationToAdd = declarationToAdd;
      myTarget = target;
    }

    public IRuleDeclaration InsertRule()
    {
      using (WriteLockCookie.Create())
      {
        var anchor = myTarget.Anchor;
        var parent = anchor.Parent;
        var whiteSpace = ModificationUtil.AddChildAfter(parent, anchor, new NewLine("\n"));
        whiteSpace = ModificationUtil.AddChildAfter(parent, whiteSpace, new NewLine("\n"));
        return ModificationUtil.AddChildAfter(parent, whiteSpace, myDeclarationToAdd);

      }
    }
  }
}