using System.Collections.Generic;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Feature.Services.ContextNavigation;
using JetBrains.ReSharper.Feature.Services.Navigation.Search;
using JetBrains.ReSharper.Features.Finding.ExecutionHosting;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.PsiToCSharp.GeneratedMethod
{
  [ContextNavigationProvider]
  public class PsiNavigateGeneratedMethodProvider : PsiNavigateFromHereProvider<GeneratedMethodContextSearch, GotoGeneratedDescriptor, GeneratedMethodSearchRequest>, INavigateFromHereProvider
  {
    public PsiNavigateGeneratedMethodProvider(IFeaturePartsContainer manager) : base(manager)
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
          "Go to generated method",
          null,
          NavigationActionGroup.Blessed,
          execution);
      }
    }

    protected override GotoGeneratedDescriptor CreateSearchDescriptor(GeneratedMethodSearchRequest searchRequest, ICollection<IOccurence> occurences)
    {
      return new GotoGeneratedDescriptor(searchRequest, occurences);
    }
  }
}
