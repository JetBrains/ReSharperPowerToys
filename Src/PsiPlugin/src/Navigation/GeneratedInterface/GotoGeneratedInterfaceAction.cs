using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ActionManagement;
using JetBrains.ReSharper.Features.Finding.NavigateFromHere;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.GeneratedInterface
{
  [ActionHandler(new string[] { "GotoGeneratedInterface" })]
  public class GotoGeneratedInterfaceAction : ContextNavigationActionBase<PsiNavigateGeneratedInterfaceProvider>
  {
  }
}
