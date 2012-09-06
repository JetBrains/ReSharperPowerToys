using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.ReSharper.LexPlugin.Grammar;
using JetBrains.ReSharper.LexPlugin.Resolve;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Refactorings.Rename;
using JetBrains.ReSharper.Refactorings.RenameModel;
using JetBrains.Util;

namespace JetBrains.ReSharper.LexPlugin.Refactoring.Rename
{
  [FeaturePart]
  public class LexRenamesFactory : AtomicRenamesFactory
  {
    public override bool IsApplicable(IDeclaredElement declaredElement)
    {
      if (declaredElement.PresentationLanguage.Is<LexLanguage>())
      {
        if (declaredElement is InitialStateDeclaredElement)
        {
          return false;
        }
        return true;
      }
      return false;
    }

    public override IEnumerable<AtomicRenameBase> CreateAtomicRenames(IDeclaredElement declaredElement, string newName, bool doNotAddBindingConflicts)
    {
       yield return new LexAtomicRename(declaredElement, newName, doNotAddBindingConflicts);
    }

    public override RenameAvailabilityCheckResult CheckRenameAvailability(IDeclaredElement element)
    {
      return RenameAvailabilityCheckResult.CanBeRenamed;
    }
  }
}
