using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Naming.Extentions;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Refactorings.Rename;
using JetBrains.ReSharper.Refactorings.RenameModel;
using JetBrains.ReSharper.Refactorings.Workflow;

namespace JetBrains.ReSharper.PsiPlugin.Refactoring
{
  public class PsiRenameHelper : RenameHelperBase
  {
    public override IRefactoringPage GetPageBeforeInitial(RenameWorkflow renameWorkflow)
    {
      return base.GetPageBeforeInitial(renameWorkflow);
    }

    public override bool IsLanguageSupported
    {
      get { return true; }
    }

    public override IDeclaredElement GetPrimevalDeclaredElement(IDeclaredElement element, IReference reference)
    {
      return base.GetPrimevalDeclaredElement(element, reference);
    }

    public override IEnumerable<IReference> GetCustomUsages(IDeclaredElement declaredElement)
    {
      return base.GetCustomUsages(declaredElement);
    }

    public override bool IsLocalRename(IDeclaredElement primevalDeclaredElement)
    {
      return base.IsLocalRename(primevalDeclaredElement);
    }

    public override void AddExtraNames(INamesCollection suggestion, IDeclaredElement declaredElement)
    {
      base.AddExtraNames(suggestion, declaredElement);
    }

    public override IEnumerable<AtomicRenameBase> CreateAtomicRenames(IDeclaredElement declaredElement, string newName, bool doNotAddBindingConflicts)
    {
      return base.CreateAtomicRenames(declaredElement, newName, doNotAddBindingConflicts);
    }

    public override RenameAvailabilityCheckResult CheckRenameAvailability(IDeclaredElement declaredElement)
    {
      return RenameAvailabilityCheckResult.CanBeRenamed;
    }

    public override IReference BindReferenceToNamespace(IReference reference, INamespace ns)
    {
      return base.BindReferenceToNamespace(reference, ns);
    }
  }
}