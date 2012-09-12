using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
using JetBrains.ReSharper.PsiPlugin.Util;
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

      var constructor = element as IConstructor;
      if (constructor != null)
      {
        ITypeElement typeElement = constructor.GetContainingType();
        if (typeElement != null)
        {
          names.Add(typeElement.ShortName);
        }

        names.Add(DeclaredElementConstants.CONSTRUCTOR_NAME);
        return names;
      }

      names.Add(element.ShortName);

      if (PsiDeclaredElementUtil.IsCollectionInitializerAddMethod(element))
      {
        names.Add("new");
      }

      if (PsiDeclaredElementUtil.IsForeachEnumeratorPatternMember(element))
      {
        names.Add("foreach");
      }

      if (DeclaredElementUtil.IsAsyncAwaitablePatternMember(element))
      {
        names.Add("async");
        names.Add("await");
      }

      // LINQ
      var method = element as IMethod;
      if (method != null)
      {
        if (method.ShortName == "Cast" && method.TypeParameters.Count == 1 && IsCorrectParametersNumberForQueryPatternMethod(method, 0))
        {
          names.Add("from");
        }
        if (method.ShortName == "Select" && IsCorrectParametersNumberForQueryPatternMethod(method, 1))
        {
          names.Add("from");
        }
        if (method.ShortName == "SelectMany" && IsCorrectParametersNumberForQueryPatternMethod(method, 2))
        {
          names.Add("from");
        }
        if (method.ShortName == "Where" && IsCorrectParametersNumberForQueryPatternMethod(method, 1))
        {
          names.Add("where");
        }
        if ((method.ShortName == "Join" || method.ShortName == "GroupJoin") && IsCorrectParametersNumberForQueryPatternMethod(method, 4))
        {
          names.Add("join");
        }
        if ((method.ShortName == "OrderBy" || method.ShortName == "OrderByDescending" || method.ShortName == "ThenBy" || method.ShortName == "ThenByDescending") && IsCorrectParametersNumberForQueryPatternMethod(method, 1))
        {
          names.Add("orderby");
        }
        if (method.ShortName == "GroupBy" && (IsCorrectParametersNumberForQueryPatternMethod(method, 1) || IsCorrectParametersNumberForQueryPatternMethod(method, 2)))
        {
          names.Add("group");
        }
      }

      return names;
    }

    public bool IsCompatibleWithLanguage(PsiLanguageType languageType)
    {
      return languageType.Is<PsiLanguage>();
    }

    public IDomainSpecificSearcher CreateConstructorSpecialReferenceSearcher(ICollection<IConstructor> constructors)
    {
      //return new PsiConstructorSpecialReferenceSearcher(constructors);
      return null;
    }

    public IDomainSpecificSearcher CreateMethodsReferencedByDelegateSearcher(IDelegate @delegate)
    {
      //return new PsiMethodsReferencedByDelegateSearcher(@delegate);
      return null;
    }

    public IDomainSpecificSearcher CreateReferenceSearcher(ICollection<IDeclaredElement> elements, bool findCandidates)
    {
      return new PsiReferenceSearcher(this, elements, false);
    }

    public IDomainSpecificSearcher CreateTextOccurenceSearcher(ICollection<IDeclaredElement> elements)
    {
      //return new PsiTextOccurenceSearcher(elements);
      return null;
    }

    public IDomainSpecificSearcher CreateTextOccurenceSearcher(string subject)
    {
      // return new PsiTextOccurenceSearcher(subject);
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

    private static bool IsCorrectParametersNumberForQueryPatternMethod(IMethod method, int expectedParameters)
    {
      if (!method.IsStatic && method.Parameters.Count == expectedParameters)
      {
        return true;
      }
      if (method.IsExtensionMethod && method.Parameters.Count == expectedParameters + 1)
      {
        return true;
      }

      return false;
    }
  }
}
