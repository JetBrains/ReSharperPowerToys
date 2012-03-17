using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Services
{
  class PsiDeclaredElementPresenter : IDeclaredElementPresenter
  {
    public string Format(DeclaredElementPresenterStyle style, IDeclaredElement element, ISubstitution substitution, out DeclaredElementPresenterMarking marking)
    {
      marking = new DeclaredElementPresenterMarking();
      IRuleDeclaration ruleDeclaration = element as IRuleDeclaration;
      if(ruleDeclaration != null)
      {
        return ruleDeclaration.RuleName.GetText();
      }
      IOptionDefinition optionDefinition = element as IOptionDefinition;
      if(optionDefinition != null)
      {
        return optionDefinition.OptionName.GetText();
      }
      IDeclaredElement declaredElement = element as IDeclaredElement;
      if(declaredElement != null)
      {
        return declaredElement.ShortName;
      }
      return "dummy string";
    }

    public string Format(ParameterKind parameterKind)
    {
      throw new NotImplementedException();
    }

    public string Format(AccessRights accessRights)
    {
      throw new NotImplementedException();
    }
  }
}
