using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Intentions.DataProviders;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.ReSharper.PsiPlugin.Util;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Intentions.CreateFromUsage
{
  public class CreatePsiRuleTarget : ICreationTarget
  {
    private readonly PsiRuleReference myReference;
    private readonly ITreeNode myElement;
    private readonly IRuleDeclaration myDeclaration;

    public CreatePsiRuleTarget(PsiRuleReference reference)
    {
      myReference = reference;
      myElement = reference.GetTreeNode();
      string name = reference.GetName();

      myDeclaration = PsiElementFactory.GetInstance(myElement.GetPsiModule()).CreateRuleDeclaration(name);

      Anchor = myElement.GetContainingNode<IRuleDeclaration>();
    }

    public ITreeNode GetTargetDeclaration()
    {
      return myDeclaration.Parent;
    }

    public IRuleDeclaration Declaration
    {
      get { return myDeclaration; }
    }

    public IFile GetTargetDeclarationFile()
    {
      return myElement.GetContainingFile();
    }

    public IEnumerable<ITreeNode> GetPossibleTargetDeclarations()
    {
      yield return myDeclaration.Parent;
    }

    public ITreeNode Anchor { get; private set; }
  }
}
