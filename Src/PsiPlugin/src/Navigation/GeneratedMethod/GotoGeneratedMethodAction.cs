using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ActionManagement;
using JetBrains.ReSharper.Features.Finding.NavigateFromHere;


namespace JetBrains.ReSharper.PsiPlugin.Navigation.GeneratedMethod
{
  [ActionHandler(new string[] { "GotoGeneratedMethod" })]
  public class GotoGeneratedMethodAction : ContextNavigationActionBase<PsiNavigateGeneratedMethodProvider>
  {
  }
}
