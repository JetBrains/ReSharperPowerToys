using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Feature.Services.ContextNavigation;
using JetBrains.ReSharper.Feature.Services.ContextNavigation.ContextSearches;
using JetBrains.ReSharper.Feature.Services.Search.SearchRequests;
using JetBrains.ReSharper.Psi;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Navigation
{
  [ContextNavigationProvider]
  class PsiNavigeFromHereProvider : INavigateFromHereProvider
  {
    private IFeaturePartsContainer myManager;

    public PsiNavigeFromHereProvider(IFeaturePartsContainer manager)
    {
      myManager = manager;
    }

    public IEnumerable<ContextNavigation> CreateWorkflow(IDataContext dataContext)
    {
      //var execution = GetSearchesExecution(dataContext, DefaultNavigationExecutionHost.Instance);
      /*if (execution != null)
      {
        yield return new ContextNavigation(
          "Go to generated",
          null,
          NavigationActionGroup.Blessed,
          execution);
      }*/
      return null;
    }

    /*public virtual Action GetSearchesExecution(IDataContext dataContext, INavigationExecutionHost host)
    {
      var searches = ContextNavigationLanguageUtil.GetAvailableContextSearches<IDeclarationContextSearch>(dataContext, myManager);
      if (Enumerable.Any(searches))
        return () => Execute(dataContext, searches, host);
      return null;
    }*/

    /*protected void Execute(IDataContext dataContext, IEnumerable<IDeclarationContextSearch> searches,
                                    INavigationExecutionHost host)
    {
      var elementsSearchPairs = searches.Select(x => x.GetElementsAndSearchRequest(dataContext));

      //we assume that if in elements-request pair there are no elements then it's a special search request
      var specialSearchRequest = TryFindSingleSpecialSearch(elementsSearchPairs);
      if (specialSearchRequest != null)
      {
        //ExecuteSearchRequest(dataContext, specialSearchRequest(dataContext, null), host);
        return;
      }

      var declaredElementsSearches = elementsSearchPairs.SelectMany(x => x.A.Select(declaredElement => JetTuple.Of(declaredElement, x.B)));
      //ExecuteElementSearchPairs(dataContext, declaredElementsSearches, host);
    }*/

    /*protected void ExecuteSearchRequest(IDataContext context, [CanBeNull] SearchRequest searchRequest, INavigationExecutionHost host)
    {
      //search request can be null means the process of creating a searchRequest was interrupted by user.
      if (searchRequest == null)
        return;

      string featureID = GetFeatureID();
      if (!featureID.IsEmpty())
      {
        TipsManager.Instance.FeatureIsUsed(featureID, context);
      }

      var results = searchRequest.Search();
      if (results == null)
        return;
      if (results.IsEmpty())
      {
        ShowResultMessage(context, GetNotFoundMessage(searchRequest));
        return;
      }

      Func<TDescriptor> descriptorBuilder = () => CreateSearchDescriptor(searchRequest, results);

      if (ProcessImmediateResult(context, results, host))
        return;

      ShowResults(context, host, searchRequest.Title, results, descriptorBuilder, Comparer.Create<IOccurence>(searchRequest.CompareOccurences));
    }*/

    protected GetSearchRequest<SearchDeclarationsRequest> TryFindSingleSpecialSearch(IEnumerable<JetTuple<IEnumerable<IDeclaredElement>, GetSearchRequest<SearchDeclarationsRequest>>> elementsSearchPairs)
    {
      foreach (JetTuple<IEnumerable<IDeclaredElement>, GetSearchRequest<SearchDeclarationsRequest>> elementSearchPair in elementsSearchPairs)
      {
        if (elementSearchPair.A.IsEmpty())
        {
          var request = elementSearchPair.B;
          return request;
        }
      }

      return null;
    }

    private void Execute(IDataContext dataContext)
    {
      throw new NotImplementedException();
    }

    private static bool IsAvailable(IDataContext dataContext)
    {
      throw new NotImplementedException();
    }
  }
}
