using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Feature.Services.ContextNavigation.ContextSearches;
using JetBrains.ReSharper.Feature.Services.Navigation;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;
using JetBrains.ReSharper.Feature.Services.Navigation.Search;
using JetBrains.ReSharper.Feature.Services.Navigation.Search.SearchRequests;
using JetBrains.ReSharper.Feature.Services.Occurences;
using JetBrains.ReSharper.Feature.Services.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Navigation
{
  public abstract class PsiNavigateFromHereProvider<TContextSearch, TDescriptor, TSearchRequest> : RequestContextSearchProvider<TContextSearch, TDescriptor, TSearchRequest>
    where TContextSearch : class, IRequestContextSearch<TSearchRequest>
    where TDescriptor : OccurenceBrowserDescriptor
    where TSearchRequest : SearchRequest
  {
    protected PsiNavigateFromHereProvider(IFeaturePartsContainer manager) : base(manager)
    {
    }

    protected override void ShowResults(IDataContext context, INavigationExecutionHost host, TSearchRequest searchRequest, ICollection<IOccurence> occurrences, Func<TDescriptor> descriptorBuilder)
    {
      var occurrencesList = occurrences.ToList();
      occurrencesList.Sort((occurrence, occurrence1) =>
      {
        var result = searchRequest.CompareOccurences(occurrence, occurrence1);
        if (result != 0)
          return result;
        return OccurenceUtil.CompareOccurences(occurrence, occurrence1, OccurencePresentationOptions.DefaultOptions);
      });

      host.ShowContextPopupMenu(context, occurrencesList, descriptorBuilder, ProvideFeatureSpecificPresentationOptions(searchRequest), true, searchRequest.Title);
    }

    protected override OccurencePresentationOptions? ProvideFeatureSpecificPresentationOptions(TSearchRequest searchRequest)
    {
      return new OccurencePresentationOptions
      {
        IconDisplayStyle = IconDisplayStyle.File,
        TextDisplayStyle = TextDisplayStyle.ContainingFile,
        LocationStyle = GlobalLocationStyle.File
      };
    }
  }
}
