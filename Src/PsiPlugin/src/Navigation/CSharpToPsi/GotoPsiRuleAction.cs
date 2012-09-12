using JetBrains.ActionManagement;
using JetBrains.ReSharper.Features.Finding.NavigateFromHere;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.CSharpToPsi
{
  [ActionHandler(new string[] { "GotoPsiRule" })]
  public class GotoPsiRuleAction : ContextNavigationActionBase<CSharpToPsiNavigateProvider>
  {
  }
}
