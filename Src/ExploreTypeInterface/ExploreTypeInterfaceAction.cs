using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.IDE;
using JetBrains.IDE.TreeBrowser;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Features.Browsing;
using JetBrains.ReSharper.Features.Common.TreePsiBrowser;
using JetBrains.ReSharper.Psi;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ExploreTypeInterface
{
  /// <summary>
  /// Handles ExploreTypeInterface action, see Actions.xml
  /// </summary>
  [ActionHandler]
  public class PowerToys_ExploreTypeInterfaceAction : IActionHandler
  {
    #region IActionHandler Members

    public void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      // Get solution from context in which action is executed
      ISolution solution = context.GetData(DataConstants.SOLUTION);
      if(solution == null)
        return;

      // Get PsiManager for solution, from which we will obtain code elements
      PsiManager psiManager = PsiManager.GetInstance(solution);

      // Obtain read lock, so that no changes will occur while we are executing action
      using(ReadLockCookie.Create())
      {
        // Ensure changes in documents are processed by code analyser
        psiManager.CommitAllDocuments();
        // Wait until all caches are built, abort action if user cancelled wait
        if (!psiManager.WaitForCaches("ExploreTypeInterfaceAction", "Cancel"))
          return;

        bool instanceOnly;
        ITypeElement typeElement = TypeInterfaceUtil.GetTypeElement(context, out instanceOnly);
        if(typeElement == null)
        {
          MessageBox.ShowExclamation("Cannot get type from current location", "Explore Type Interface");
          return;
        }
        // Create descriptor and ask TreeModelBrowser to show it in the HierarchyResults view. 
        // Same view, where type hierarchy is shown
        var descriptor = new TypeInterfaceDescriptor(typeElement, instanceOnly);
        TreeModelBrowser.GetInstance(solution).Show(HierarchyWindowRegistrar.HierarchyWindowID, descriptor, new TreeModelBrowserPanelPsi(descriptor));
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