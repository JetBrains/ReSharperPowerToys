using System;
using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Util;
using JetBrains.ReSharper.Refactorings.Conflicts;
using JetBrains.ReSharper.Refactorings.Rename;
using JetBrains.ReSharper.Refactorings.RenameModel;
using JetBrains.ReSharper.Refactorings.Workflow;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Refactoring
{
  public class PsiRename : RenameBase
  {
    public PsiRename(RenameWorkflow workflow, ISolution solution, IRefactoringDriver driver):base(workflow, solution, driver)
    {
    }

    public override string[] GetPossibleReferenceNames(IDeclaredElement element, string newName)
    {
      return new[] { newName };
    }

    public override IList<IConflictSearcher> AdditionalConflictsSearchers(IDeclaredElement element, string newName)
    {
      return EmptyList<IConflictSearcher>.InstanceList;
    }

    public override void AdditionalReferenceProcessing(IDeclaredElement newTarget, IReference reference, ICollection<IReference> newReferences)
    {
    }

    public override IReference TransformProjectedInitializer(IReference reference)
    {
      //IRuleName ruleName = (IRuleName) reference.GetTreeNode();
      //PsiTreeUtil.ReplaceRuleName(ruleName.Parent, ruleName,);
      return reference;
    }

    public override bool DoNotProcess(IDeclaredElement element)
    {
      return false;
    }
  }
}