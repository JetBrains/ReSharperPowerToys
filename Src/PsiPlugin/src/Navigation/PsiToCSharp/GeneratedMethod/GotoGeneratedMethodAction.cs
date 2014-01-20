using JetBrains.ActionManagement;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.PsiToCSharp.GeneratedMethod
{
  [ActionHandler(new [] { "GotoGeneratedMethod" })]
  public class GotoGeneratedMethodAction : ContextNavigationActionBase<PsiNavigateGeneratedMethodProvider>
  {
  }
}
