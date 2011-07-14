using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.Application.DataContext;
using JetBrains.DataFlow;
using JetBrains.IDE.TreeBrowser;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Features.Browsing;
using JetBrains.ReSharper.Features.Common.TreePsiBrowser;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ExploreTypeInterface
{
  /// <summary>
  /// Handles ExploreTypeInterface action, see Actions.xml
  /// </summary>
  [ActionHandler]
  public class PowerToys_ExploreTypeInterfaceAction : IActionHandler
  {
    private readonly Lifetime lifetime;

    public PowerToys_ExploreTypeInterfaceAction(Lifetime lifetime)
    {
      this.lifetime = lifetime;
    }

    #region IActionHandler Members

    public void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      // Get solution from context in which action is executed
      ISolution solution = context.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      if (solution == null)
        return;

      // Get PsiManager for solution, from which we will obtain code elements
      PsiManager psiManager = PsiManager.GetInstance(solution);
      CacheManager cacheManager = CacheManager.GetInstance(solution);

      // Obtain read lock, so that no changes will occur while we are executing action
      using (ReadLockCookie.Create())
      {
        // Ensure changes in documents are processed by code analyser
        psiManager.CommitAllDocuments();

        // Wait until all caches are built, abort action if user cancelled wait
        if (!cacheManager.WaitForCaches("ExploreTypeInterfaceAction", "Cancel"))
          return;

        bool instanceOnly;
        ITypeElement typeElement = TypeInterfaceUtil.GetTypeElement(context, out instanceOnly);
        if (typeElement == null)
        {
          MessageBox.ShowExclamation("Cannot get type from current location", "Explore Type Interface");
          return;
        }
        // Create descriptor and ask TreeModelBrowser to show it in the HierarchyResults view. 
        // Same view, where type hierarchy is shown
        var descriptor = new TypeInterfaceDescriptor(typeElement, instanceOnly);

        // todo: fix by using ActivateToolWindowActionHandler<T> as example
        //TreeModelBrowser.GetInstance(solution).Show(lifetime, HierarchyWindowRegistrar.HierarchyWindowID, descriptor,
        //                                            new TreeModelBrowserPanelPsi(descriptor));
      }
    }

    public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      // Check that we have a solution
      return context.CheckAllNotNull(Psi.Services.DataConstants.DECLARED_ELEMENT);
    }

    #endregion
  }
}