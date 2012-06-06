using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Feature.Services.ContextNavigation;
using JetBrains.ReSharper.Feature.Services.ContextNavigation.ContextSearches;
using JetBrains.ReSharper.Feature.Services.Occurences;
using JetBrains.ReSharper.Feature.Services.Search;
using JetBrains.ReSharper.Features.Common.Occurences;
using JetBrains.ReSharper.Features.Common.Occurences.ExecutionHosting;
using JetBrains.ReSharper.Features.Finding.ExecutionHosting;
using JetBrains.ReSharper.Features.Finding.NavigateFromHere;

namespace JetBrains.ReSharper.PsiPlugin.Navigation
{
  [ContextNavigationProvider]
  public class PsiNavigateFromHereProvider : RequestContextSearchProvider<GeneratedContextSearch, GotoGeneratedDescriptor, GeneratedSearchRequest>, INavigateFromHereProvider
  {

    public PsiNavigateFromHereProvider(IFeaturePartsContainer manager) : base(manager)
    {
    }

    public IEnumerable<ContextNavigation> CreateWorkflow(IDataContext dataContext)
    {
      var execution = GetSearchesExecution(dataContext, DefaultNavigationExecutionHost.Instance);
      if (execution != null)
      {
        yield return new ContextNavigation(
          "Go to generated class",
          null,
          NavigationActionGroup.Blessed,
          execution);
      }
    }

    protected override void ShowResults(IDataContext context, INavigationExecutionHost host, string title, ICollection<IOccurence> occurences, Func<GotoGeneratedDescriptor> descriptorBuilder, IComparer<IOccurence> customSearchRequestComparer)
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

    protected override GotoGeneratedDescriptor CreateSearchDescriptor(GeneratedSearchRequest searchRequest, ICollection<IOccurence> occurences)
    {
      return new GotoGeneratedDescriptor(searchRequest, occurences);
    }
  }
}
