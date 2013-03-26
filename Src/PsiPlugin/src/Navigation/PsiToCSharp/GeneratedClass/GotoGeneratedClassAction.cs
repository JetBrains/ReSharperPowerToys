using JetBrains.ActionManagement;
using JetBrains.ReSharper.Features.Finding.NavigateFromHere;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.PsiToCSharp.GeneratedClass
{
  [ActionHandler(new string[] { "GotoGeneratedClass" })]
  public class GotoGeneratedClassAction : ContextNavigationActionBase<PsiNavigateGeneratedClassProvider>
  {
  }
}
