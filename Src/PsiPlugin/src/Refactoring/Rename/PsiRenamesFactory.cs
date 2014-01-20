using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.Refactorings.Specific.Rename;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.Refactoring.Rename
{
  [ShellFeaturePart]
  public class PsiRenamesFactory : AtomicRenamesFactory
  {
    public override bool IsApplicable(IDeclaredElement declaredElement)
    {
      if (declaredElement.PresentationLanguage.Is<PsiLanguage>())
      {
        return true;
      }
      return false;
    }

    public override IEnumerable<AtomicRenameBase> CreateAtomicRenames(IDeclaredElement declaredElement, string newName, bool doNotAddBindingConflicts)
    {
      yield return new PsiAtomicRename(declaredElement, newName, doNotAddBindingConflicts);
      if (declaredElement is RuleDeclaration)
      {
        var ruleDeclaration = declaredElement as RuleDeclaration;
        ruleDeclaration.UpdateDerivedDeclaredElements();
        foreach (IDeclaredElement element in ruleDeclaration.DerivedParserMethods)
        {
          yield return new PsiDerivedElementRename(element, "parse" + NameToCamelCase(newName),
            doNotAddBindingConflicts);
        }
        foreach (IDeclaredElement element in ruleDeclaration.DerivedClasses)
        {
          yield return new PsiDerivedElementRename(element, NameToCamelCase(newName),
            doNotAddBindingConflicts);
        }
        foreach (IDeclaredElement element in ruleDeclaration.DerivedInterfaces)
        {
          yield return new PsiDerivedElementRename(element, ruleDeclaration.InterfacePrefix + NameToCamelCase(newName),
            doNotAddBindingConflicts);
        }
        foreach (IDeclaredElement element in ruleDeclaration.DerivedVisitorMethods)
        {
          yield return new PsiDerivedElementRename(element, ruleDeclaration.VisitorMethodPrefix + NameToCamelCase(newName) + ruleDeclaration.VisitorMethodSuffix,
            doNotAddBindingConflicts);
        }
      }
    }

    public override RenameAvailabilityCheckResult CheckRenameAvailability(IDeclaredElement element)
    {
      return RenameAvailabilityCheckResult.CanBeRenamed;
    }

    public static string NameToCamelCase(string s)
    {
      string firstLetter = s.Substring(0, 1);
      firstLetter = firstLetter.ToUpper();
      s = firstLetter + s.Substring(1, s.Length - 1);
      return s;
    }

    public static string NameFromCamelCase(string s)
    {
      string firstLetter = s.Substring(0, 1);
      firstLetter = firstLetter.ToLower();
      s = firstLetter + s.Substring(1, s.Length - 1);
      return s;
    }
  }
}
