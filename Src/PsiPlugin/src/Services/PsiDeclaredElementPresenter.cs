using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Services
{
  internal class PsiDeclaredElementPresenter : IDeclaredElementPresenter
  {
    #region IDeclaredElementPresenter Members

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
      throw new NotImplementedException();
    }

    public string Format(AccessRights accessRights)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
