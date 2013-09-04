using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Refactorings.Rename;

namespace JetBrains.ReSharper.PsiPlugin.Refactoring.Rename
{
  [RenamePart]
  internal class PsiPrimaryDeclaredElementForRenameProvider : IPrimaryDeclaredElementForRenameProvider
  {
    public IDeclaredElement GetPrimaryDeclaredElement(IDeclaredElement declaredElement, IReference reference)
    {
      IDeclaredElement derivedElement = null;

      var method = declaredElement as IMethod;
      if (method != null)
      {
        derivedElement = DerivedDeclaredElementUtil.GetPrimaryDeclaredElementForMethod(method);
      }


      var @class = declaredElement as IClass;
      if (@class != null)
        derivedElement = DerivedDeclaredElementUtil.GetPrimaryDeclaredElementForClass(@class);

      var @interface = declaredElement as IInterface;
      if (@interface != null)
        derivedElement = DerivedDeclaredElementUtil.GetPrimaryDeclaredElementForInterface(@interface);

      if(derivedElement != null)
      {
        return derivedElement;
      }
      return declaredElement;
    }
  }
}
