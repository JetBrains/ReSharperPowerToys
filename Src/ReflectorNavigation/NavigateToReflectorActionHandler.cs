using System.Collections.Generic;
using JetBrains.ActionManagement;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.ExternalSources.ReSharperIntegration;
using JetBrains.ReSharper.Feature.Services.Navigation;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Services;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ReflectorNavigation
{
  [ActionHandler]
  public class NavigateToReflectorActionHandler : IActionHandler
  {
    #region IActionHandler Members

    public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      var compiledElement = context.GetData(DataConstants.DECLARED_ELEMENT) as ICompiledElement;
      return compiledElement != null;
    }

    public void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      var compiledElement = context.GetData(DataConstants.DECLARED_ELEMENT) as ICompiledElement;
      if (compiledElement == null)
        return;

      try
      {
        IEnumerable<INavigationPoint> points =
          new ExternalSourcesNavigationProvider().CreateNavigationPoints(
            compiledElement, EmptyArray<INavigationPoint>.Instance,
            new[] {new ReflectorExternalSourcesProvider()});

        if (points != null && !points.IsEmpty())
          NavigationManager.GetInstance(compiledElement.GetSolution()).Navigate(
            points, NavigationOptions.FromDataContext(context, "Declarations"));
      } catch (ProcessCancelledException)
      {
      }
    }

    #endregion
  }
}