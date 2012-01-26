using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.ReSharper.Refactorings.Conflicts;
using JetBrains.ReSharper.Refactorings.Rename;
using JetBrains.ReSharper.Refactorings.RenameModel;
using JetBrains.ReSharper.Psi.CSharp.Impl.DeclaredElement;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Refactoring.Rename
{
  [FeaturePart]
  public class PsiRenamesFactory : AtomicRenamesFactory
  {
    public override bool IsApplicable(IDeclaredElement declaredElement)
    {
      if(declaredElement.PresentationLanguage.Is<PsiLanguage>())
      {
        return true;
      }
      return false;
    }

    /*internal bool isCSharpPsiMember(IDeclaredElement declaredElement)
    {
      if(declaredElement.PresentationLanguage.Is<PsiLanguage>()){
        if(declaredElement is CSharpMethod)
        {
          var method = declaredElement as CSharpMethod;
          string name = method.ShortName;
          return (name.Length > "parse".Length) && ("parse".Equals(name.Substring(0, "parse".Length)));
        }

        return ((declaredElement is IClass) || (declaredElement is IInterface));
     }
     return false;
    }*/

    public override IEnumerable<AtomicRenameBase> CreateAtomicRenames(IDeclaredElement declaredElement, string newName, bool doNotAddBindingConflicts)
    {
      yield return new PsiAtomicRename(declaredElement, newName, doNotAddBindingConflicts);
      if (declaredElement is RuleDeclaration)
      {
        var ruleDeclaration = declaredElement as RuleDeclaration;
        if (ruleDeclaration != null)
        {
          ruleDeclaration.CollectDerivedDeclaredElements();
          foreach (IDeclaredElement element in ruleDeclaration.DerivedDeclaredElements)
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
            yield return new PsiDerivedElementRename(element, "I" + NameToCamelCase(newName),
                                                                         doNotAddBindingConflicts);
          }
        }
      }
    }

    public override RenameAvailabilityCheckResult CheckRenameAvailability(IDeclaredElement element)
    {
      return RenameAvailabilityCheckResult.CanBeRenamed;
    }

    private string NameToCamelCase(string s)
    {
      string firstLetter = s.Substring(0, 1);
      firstLetter = firstLetter.ToUpper();
      s = firstLetter + s.Substring(1, s.Length - 1);
      return s;
    }

  }



  /*[Language(typeof(PsiLanguage))]
  public class PsiRenameHelper : RenameHelperBase
  {
    public override bool IsLanguageSupported
    {
      get { return true; }
    }

    public override IList<IConflictSearcher> AdditionalConflictsSearchers(IDeclaredElement element, string newName)
    {
      return EmptyList<IConflictSearcher>.InstanceList;
    }

    public override IReference TransformProjectedInitializer(IReference reference)
    {
      return reference;
    }

    public override bool DoNotProcess(IDeclaredElement element)
    {
      return false;
    }

  }*/
}