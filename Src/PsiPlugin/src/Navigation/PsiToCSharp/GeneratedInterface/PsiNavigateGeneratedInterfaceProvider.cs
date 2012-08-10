using System.Collections.Generic;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Feature.Services.ContextNavigation;
using JetBrains.ReSharper.Feature.Services.Search;
using JetBrains.ReSharper.Features.Finding.ExecutionHosting;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.GeneratedInterface
{
  [ContextNavigationProvider]
  public class PsiNavigateGeneratedInterfaceProvider : PsiNavigateFromHereProvider<GeneratedInterfaceContextSearch, GotoGeneratedDescriptor , GeneratedInterfaceSearchRequest>, INavigateFromHereProvider
  {

    public PsiNavigateGeneratedInterfaceProvider(IFeaturePartsContainer manager) : base(manager)
    {
    }

    public IEnumerable<ContextNavigation> CreateWorkflow(IDataContext dataContext)
    {
      var execution = GetSearchesExecution(dataContext, DefaultNavigationExecutionHost.Instance);
      if (execution != null)
      {
        yield return new ContextNavigation(
          "Go to generated interface",
          null,
          NavigationActionGroup.Blessed,
          execution);
      }
    }

    protected override GotoGeneratedDescriptor CreateSearchDescriptor(GeneratedInterfaceSearchRequest searchRequest, ICollection<IOccurence> occurences)
    {
      return new GotoGeneratedDescriptor(searchRequest, occurences);
    }
  }
}
