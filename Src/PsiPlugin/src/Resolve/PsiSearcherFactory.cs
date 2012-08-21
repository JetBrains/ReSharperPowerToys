using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Impl.Search.SearchDomain;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.PsiPlugin.Feature.Services.FindUsages;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
using JetBrains.ReSharper.PsiPlugin.PsiGrammar;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  [PsiSharedComponent]
  class PsiSearcherFactory : IDomainSpecificSearcherFactory
  {
    public bool IsCompatibleWithLanguage(PsiLanguageType languageType)
    {
      return ((languageType == PsiLanguage.Instance) || (languageType == CSharpLanguage.Instance));
    }

    public IDomainSpecificSearcher CreateConstructorSpecialReferenceSearcher(ICollection<IConstructor> constructors)
    {
      return null;
    }

    public IDomainSpecificSearcher CreateMethodsReferencedByDelegateSearcher(IDelegate @delegate)
    {
      return null;
    }

    public IDomainSpecificSearcher CreateReferenceSearcher(ICollection<IDeclaredElement> elements, bool findCandidates)
    {
      return new PsiReferenceSearcher(this, elements, false);
    }

    public IDomainSpecificSearcher CreateTextOccurenceSearcher(ICollection<IDeclaredElement> elements)
    {
      return null;
    }

    public IDomainSpecificSearcher CreateTextOccurenceSearcher(string subject)
    {
      return null;
    }

    public IDomainSpecificSearcher CreateLateBoundReferenceSearcher(ICollection<IDeclaredElement> elements)
    {
      return new PsiReferenceSearcher(this, elements, true);
    }

    public IDomainSpecificSearcher CreateAnonymousTypeSearcher(IList<AnonymousTypeDescriptor> typeDescription, bool caseSensitive)
    {
      return null;
    }

    public IDomainSpecificSearcher CreateConstantExpressionSearcher(ConstantValue constantValue, bool onlyLiteralExpression)
    {
      return null;
    }

    public IEnumerable<string> GetAllPossibleWordsInFile(IDeclaredElement element)
    {
      var shortName = element.ShortName;

      /*var psiLanguageService = element.PresentationLanguage.LanguageService() as PsiLanguageService;
      if (psiLanguageService != null)
      {
        if (!psiLanguageService.IsValidJavaScriptReferenceExpressionName(shortName))
          return new[] { string.Empty };
      }*/

      return new[] { shortName };
    }

    public IEnumerable<Pair<IDeclaredElement, Predicate<FindResult>>> GetRelatedDeclaredElements(IDeclaredElement element)
    {
      var ruleDeclaration = element as RuleDeclaration;
      if (ruleDeclaration != null)
      {
        return ruleDeclaration.GetRelatedDeclaredElements();
      }
      return EmptyList<Pair<IDeclaredElement, Predicate<FindResult>>>.InstanceList;
    }

    public JetTuple<ICollection<IDeclaredElement>, Predicate<IFindResultReference>, bool> GetDerivedFindRequest(IFindResultReference result)
    {
      return null;
    }

    public JetTuple<ICollection<IDeclaredElement>, bool> GetNavigateToTargets(IDeclaredElement element)
    {
      return null;
    }

    public ICollection<FindResult> TransformNavigationTargets(ICollection<FindResult> targets)
    {
      return targets;
    }

    public ISearchDomain GetDeclaredElementSearchDomain(IDeclaredElement declaredElement)
    {
      /*if (declaredElement is IPsiDeclaredElement)
      {
        var searchDomainOwner = declaredElement as IPsiSearchDomainOwner;

        Assertion.Assert(searchDomainOwner != null, "JavaScript declared elements ({0}) must implement IJavaScriptSearchDomainOwner",
          declaredElement.GetType().FullName);

        return searchDomainOwner.GetSearchDomain();
      }*/
      return EmptySearchDomain.Instance;
    }
  }
}
