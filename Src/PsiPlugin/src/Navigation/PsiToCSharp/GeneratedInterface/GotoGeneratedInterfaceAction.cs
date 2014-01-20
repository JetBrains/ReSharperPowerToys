using JetBrains.ActionManagement;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.PsiToCSharp.GeneratedInterface
{
  [ActionHandler(new [] { "GotoGeneratedInterface" })]
  public class GotoGeneratedInterfaceAction : ContextNavigationActionBase<PsiNavigateGeneratedInterfaceProvider>
  {
  }
}
