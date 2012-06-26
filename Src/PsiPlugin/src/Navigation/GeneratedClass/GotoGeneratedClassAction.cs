using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ActionManagement;
using JetBrains.ReSharper.Features.Finding.NavigateFromHere;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.GeneratedClass
{
  [ActionHandler(new string[] { "GotoGeneratedClass" })]
  public class GotoGeneratedClassAction : ContextNavigationActionBase<PsiNavigateGeneratedClassProvider>
  {
  }
}
