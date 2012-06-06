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
    //private IFeaturePartsContainer myManager;


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

    protected override GotoGeneratedDescriptor CreateSearchDescriptor(GeneratedSearchRequest searchRequest, ICollection<IOccurence> occurences)
    {
      return new GotoGeneratedDescriptor(searchRequest, occurences);
    }
  }
}
