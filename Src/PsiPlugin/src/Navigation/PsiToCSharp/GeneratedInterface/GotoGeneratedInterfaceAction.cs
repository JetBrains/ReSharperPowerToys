using JetBrains.ActionManagement;
using JetBrains.ReSharper.Features.Finding.NavigateFromHere;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.PsiToCSharp.GeneratedInterface
{
  [ActionHandler(new string[] { "GotoGeneratedInterface" })]
  public class GotoGeneratedInterfaceAction : ContextNavigationActionBase<PsiNavigateGeneratedInterfaceProvider>
  {
  }
}
