using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Services
{
  internal class PsiDeclaredElementPresenter : IDeclaredElementPresenter
  {
    public string Format(DeclaredElementPresenterStyle style, IDeclaredElement element, ISubstitution substitution, out DeclaredElementPresenterMarking marking)
    {
      marking = new DeclaredElementPresenterMarking();
      var ruleDeclaration = element as IRuleDeclaration;
      if (ruleDeclaration != null)
      {
        return ruleDeclaration.RuleName.GetText();
      }
      return element.ShortName;
    }

    public string Format(ParameterKind parameterKind)
    {
      return string.Empty;
    }

    public string Format(AccessRights accessRights)
    {
      return string.Empty;
    }
  }
}
