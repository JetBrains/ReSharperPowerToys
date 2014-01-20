using JetBrains.ActionManagement;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.CSharpToPsi
{
  [ActionHandler(new[] { "GotoPsiRule" })]
  public class GotoPsiRuleAction : ContextNavigationActionBase<CSharpToPsiNavigateProvider>
  {
  }
}
