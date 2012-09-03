using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;

namespace JetBrains.ReSharper.LexPlugin.Services
{
  public class LexDeclaredElementPresenter : IDeclaredElementPresenter
  {
    public string Format(DeclaredElementPresenterStyle style, IDeclaredElement element, ISubstitution substitution, out DeclaredElementPresenterMarking marking)
    {
      marking = new DeclaredElementPresenterMarking();
      var tokenDeclaration = element as ITokenDeclaration;
      if (tokenDeclaration != null)
      {
        return tokenDeclaration.TokenName.GetText();
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
  }
}
