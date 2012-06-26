using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ActionManagement;

namespace JetBrains.ReSharper.PsiPlugin.Navigation
{
  [ActionHandler(new string[] { "GotoGeneratedClass" })]
  public class GotogeneratedClassAction : ContextNavigationActionBase<PsiNavigateFromHereProvider>
  {
  }
}
