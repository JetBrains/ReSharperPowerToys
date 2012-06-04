using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Feature.Services.ContextNavigation;
using JetBrains.ReSharper.Feature.Services.ContextNavigation.ContextSearches;
using JetBrains.ReSharper.Features.Common.Occurences.ExecutionHosting;
using JetBrains.ReSharper.Features.Finding.ExecutionHosting;
using JetBrains.ReSharper.Features.Finding.NavigateFromHere;

namespace JetBrains.ReSharper.PsiPlugin.Navigation
{
  [ContextNavigationProvider]
  public class PsiNavigateFromHereProvider : INavigateFromHereProvider
  {
    private IFeaturePartsContainer myManager;

    public PsiNavigateFromHereProvider(IFeaturePartsContainer manager)
    {
      myManager = manager;
    }

    public IEnumerable<ContextNavigation> CreateWorkflow(IDataContext dataContext)
    {
      var execution = GetSearchesExecution(dataContext, DefaultNavigationExecutionHost.Instance);
      if (execution != null)
      {
        yield return new ContextNavigation(
          "Go to generated",
          null,
          NavigationActionGroup.Blessed,
          execution);
      }
    }

    public virtual Action GetSearchesExecution(IDataContext dataContext, INavigationExecutionHost host)
    {
      var searches = ContextNavigationLanguageUtil.GetAvailableContextSearches<GeneratedContextSearch>(dataContext, myManager);
      if (Enumerable.Any(searches))
        return () => Execute(dataContext, searches, host);
      return null;
    }

    private void Execute(IDataContext dataContext, ICollection<GeneratedContextSearch> searches, INavigationExecutionHost host)
    {
      throw new NotImplementedException();
    }
  }
}
