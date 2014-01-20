using System.Collections.Generic;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Feature.Services.ContextNavigation;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;
using JetBrains.ReSharper.Feature.Services.Navigation.Search;
using JetBrains.ReSharper.Features.Finding.ExecutionHosting;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.PsiToCSharp.GeneratedInterface
{
  [ContextNavigationProvider]
  public class PsiNavigateGeneratedInterfaceProvider : PsiNavigateFromHereProvider<GeneratedInterfaceContextSearch, GotoGeneratedDescriptor , GeneratedInterfaceSearchRequest>, INavigateFromHereProvider
  {

    public PsiNavigateGeneratedInterfaceProvider(IFeaturePartsContainer manager) : base(manager)
    {
    }

    public IEnumerable<ContextNavigation> CreateWorkflow(IDataContext dataContext)
    {
      var solution = dataContext.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      if (solution == null) yield break;
      var execution = GetSearchesExecution(dataContext, DefaultNavigationExecutionHost.GetInstance(solution));
      if (execution != null)
      {
        yield return new ContextNavigation(
          "Go to generated interface",
          null,
          NavigationActionGroup.Blessed,
          execution);
      }
    }

    protected override GotoGeneratedDescriptor CreateSearchDescriptor(GeneratedInterfaceSearchRequest searchRequest, ICollection<IOccurence> occurrences)
    {
      return new GotoGeneratedDescriptor(searchRequest, occurrences);
    }
  }
}
