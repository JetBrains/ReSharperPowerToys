using JetBrains.ActionManagement;
using JetBrains.ReSharper.Features.Finding.NavigateFromHere;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.PsiToCSharp.GeneratedMethod
{
  [ActionHandler(new string[] { "GotoGeneratedMethod" })]
  public class GotoGeneratedMethodAction : ContextNavigationActionBase<PsiNavigateGeneratedMethodProvider>
  {
  }
}
