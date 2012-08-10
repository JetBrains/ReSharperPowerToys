using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ActionManagement;
using JetBrains.ReSharper.Features.Finding.NavigateFromHere;
using JetBrains.ReSharper.PsiPlugin.Navigation.GeneratedClass;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.CSharpToPsi
{
  [ActionHandler(new string[] { "GotoPsiRule" })]
  public class GotopsiRuleAction : ContextNavigationActionBase<CSharpToPsiNavigateProvider>
  {
  }
}
