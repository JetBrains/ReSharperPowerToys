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
      return string.Empty;
    }

    public string Format(AccessRights accessRights)
    {
      return string.Empty;
    }
  }
}
