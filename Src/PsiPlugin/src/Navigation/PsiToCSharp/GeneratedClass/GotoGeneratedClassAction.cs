using JetBrains.ActionManagement;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.PsiToCSharp.GeneratedClass
{
  [ActionHandler(new [] { "GotoGeneratedClass" })]
  public class GotoGeneratedClassAction : ContextNavigationActionBase<PsiNavigateGeneratedClassProvider>
  {
  }
}
