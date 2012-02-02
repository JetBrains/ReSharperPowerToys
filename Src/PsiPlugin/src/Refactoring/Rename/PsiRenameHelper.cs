using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.PsiPlugin.Cach;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.ReSharper.Refactorings.Conflicts;
using JetBrains.ReSharper.Refactorings.Rename;
using JetBrains.ReSharper.Refactorings.RenameModel;
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
          foreach(IDeclaredElement element in ruleDeclaration.DerivedVisitorMethods)
          {
            yield return new PsiDerivedElementRename(element, ruleDeclaration.VisitorMethodPrefix + NameToCamelCase(newName) + ruleDeclaration.VisitorMethodSuffix,
                                                                        doNotAddBindingConflicts);           
          }
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

  [RenamePart]
  internal class PsiPrimaryDeclaredElementForRenameProvider : IPrimaryDeclaredElementForRenameProvider
  {
    public IDeclaredElement GetPrimaryDeclaredElement(IDeclaredElement declaredElement, IReference reference)
    {
      if (declaredElement is IMethod)
      {
        IMethod method = declaredElement as IMethod;
        string methodName = method.ShortName;
        if ("parse".Equals(methodName.Substring(0, "parse".Length)))
        {
          string ruleName = methodName.Substring("parse".Length, methodName.Length - "parse".Length);
          var cache = declaredElement.GetPsiServices().Solution.GetComponent<PsiCache>();
          ICollection<IPsiSymbol> symbols = cache.GetSymbol(PsiRenamesFactory.NameFromCamelCase(ruleName));
          if (symbols.Count > 0)
          {
            IPsiSymbol symbol = symbols.ToArray()[0];
            var element =
              symbol.SourceFile.GetPsiFile<PsiLanguage>().FindNodeAt(new TreeTextRange(new TreeOffset(symbol.Offset), 1));
            while (element != null)
            {
              if (element is IDeclaredElement)
              {
                return (IDeclaredElement) element;
              }
              element = element.Parent;
            }
            return declaredElement;
          }
          //todo check considence parser adn psi
          /*foreach (var psiSymbol in symbols)
          {
           ICollection<IPsiSymbol> files = cache.GetSymbol()           
          }*/
        }
      }

      if(declaredElement is IClass)
      {
        IClass classElement = declaredElement as IClass;
        string className = classElement.ShortName;
        var cache = declaredElement.GetPsiServices().Solution.GetComponent<PsiCache>();
        ICollection<IPsiSymbol> symbols = cache.GetSymbol(PsiRenamesFactory.NameFromCamelCase(className));
        if (symbols.Count > 0)
        {
          IPsiSymbol symbol = symbols.ToArray()[0];
          var element =
            symbol.SourceFile.GetPsiFile<PsiLanguage>().FindNodeAt(new TreeTextRange(new TreeOffset(symbol.Offset), 1));
          while (element != null)
          {
            if (element is IDeclaredElement)
            {
              return (IDeclaredElement)element;
            }
            element = element.Parent;
          }
          return declaredElement;
        }
      }

      if (declaredElement is IInterface)
      {
        IInterface interfaceElement = declaredElement as IInterface;
        string interfaceName = interfaceElement.ShortName;
        interfaceName = interfaceName.Substring(1, interfaceName.Length - 1);
        var cache = declaredElement.GetPsiServices().Solution.GetComponent<PsiCache>();
        ICollection<IPsiSymbol> symbols = cache.GetSymbol(PsiRenamesFactory.NameFromCamelCase(interfaceName));
        if (symbols.Count > 0)
        {
          IPsiSymbol symbol = symbols.ToArray()[0];
          var element =
            symbol.SourceFile.GetPsiFile<PsiLanguage>().FindNodeAt(new TreeTextRange(new TreeOffset(symbol.Offset), 1));
          while (element != null)
          {
            if (element is IDeclaredElement)
            {
              return (IDeclaredElement)element;
            }
            element = element.Parent;
          }
          return declaredElement;
        }
      }
      return declaredElement;
    }
  }
}