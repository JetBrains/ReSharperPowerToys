using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Feature.Services.ContextNavigation.ContextSearches;
using JetBrains.ReSharper.Feature.Services.Navigation.Search;
using JetBrains.ReSharper.Feature.Services.Navigation.Search.SearchRequests;
using JetBrains.ReSharper.Feature.Services.Occurences;
using JetBrains.ReSharper.Feature.Services.Tree;
using JetBrains.ReSharper.Features.Common.Occurences.ExecutionHosting;
using JetBrains.ReSharper.Features.Finding.NavigateFromHere;

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

    protected override void ShowResults(IDataContext context, INavigationExecutionHost host, TSearchRequest searchRequest, ICollection<IOccurence> occurences, Func<TDescriptor> descriptorBuilder)
    {
      var occurencesList = occurences.ToList();
      occurencesList.Sort((occurence, occurence1) =>
      {
        var result = searchRequest.CompareOccurences(occurence, occurence1);
        if (result != 0)
          return result;
        return OccurenceUtil.CompareOccurences(occurence, occurence1, OccurencePresentationOptions.DefaultOptions);
      });

      host.ShowContextPopupMenu(context, occurencesList, descriptorBuilder, ProvideFeatureSpecificPresentationOptions(searchRequest), true, searchRequest.Title);
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
