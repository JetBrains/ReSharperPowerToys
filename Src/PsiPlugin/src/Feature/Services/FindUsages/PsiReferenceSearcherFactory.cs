using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
using JetBrains.Util;
using JetBrains.Util.DataStructures;

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services.FindUsages
{
  [PsiSharedComponent]
  internal class PsiSearcherFactory : IDomainSpecificSearcherFactory
  {
    private readonly SearchDomainFactory mySearchDomainFactory;

    public PsiSearcherFactory(SearchDomainFactory searchDomainFactory)
    {
      mySearchDomainFactory = searchDomainFactory;
    }

    #region IDomainSpecificSearcherFactory Members

    public IEnumerable<string> GetAllPossibleWordsInFile(IDeclaredElement element)
    {
      var names = new JetHashSet<string>();

      names.Add(element.ShortName);

      return names;
    }

    public bool IsCompatibleWithLanguage(PsiLanguageType languageType)
    {
      return languageType.Is<PsiLanguage>();
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
      return new ConstantExpressionDomainSpecificSearcher<PsiLanguage>(constantValue, onlyLiteralExpression);
    }

    public IEnumerable<Pair<IDeclaredElement, Predicate<FindResult>>> GetRelatedDeclaredElements(IDeclaredElement element)
    {
      //todo
      yield return new Pair<IDeclaredElement, Predicate<FindResult>>(element, JetPredicate<FindResult>.True);
    }

    public JetTuple<ICollection<IDeclaredElement>, Predicate<IFindResultReference>, bool> GetDerivedFindRequest(IFindResultReference result)
    {
      //todo
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
      HybridCollection<IPsiSourceFile> files = declaredElement.GetSourceFiles();
      if (!(declaredElement is RuleDeclaration))
      {
        if (files.Count > 0)
        {
          return mySearchDomainFactory.CreateSearchDomain(files[0]);
        }
      }
      return mySearchDomainFactory.CreateSearchDomain(declaredElement.GetSolution(), false);
    }

    #endregion
  }
}
