using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ActionManagement;
using JetBrains.ReSharper.Features.Finding.GoToDeclaration;
using JetBrains.ReSharper.Features.Finding.NavigateFromHere;

namespace JetBrains.ReSharper.PsiPlugin.Navigation
{
  [ActionHandler(new string[] { "GotoGeneratedClass" })]
  public class GotogeneratedClassAction : ContextNavigationActionBase<PsiNavigateFromHereProvider>
  {
  }
}
