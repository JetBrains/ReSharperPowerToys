using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Feature.Services.ContextNavigation.ContextSearches;
using JetBrains.ReSharper.Feature.Services.Occurences;
using JetBrains.ReSharper.Feature.Services.Search;
using JetBrains.ReSharper.Feature.Services.Search.SearchRequests;
using JetBrains.ReSharper.Features.Common.Occurences;
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

    protected override void ShowResults(IDataContext context, INavigationExecutionHost host, string title, ICollection<IOccurence> occurences, Func<TDescriptor> descriptorBuilder, IComparer<IOccurence> customSearchRequestComparer)
    {
      var occurencesList = occurences.ToList();
      occurencesList.Sort((occurence, occurence1) =>
      {
        var result = customSearchRequestComparer.Compare(occurence, occurence1);
        if (result != 0)
          return result;
        return OccurenceUtil.CompareOccurences(occurence, occurence1, OccurencePresentationOptions.DefaultOptions);
      });

      host.ShowResultsPopupMenu(context, occurencesList, descriptorBuilder, ProvideFeatureSpecificPresentationOptions(), true, title);
    }

    protected override OccurencePresentationOptions? ProvideFeatureSpecificPresentationOptions()
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
