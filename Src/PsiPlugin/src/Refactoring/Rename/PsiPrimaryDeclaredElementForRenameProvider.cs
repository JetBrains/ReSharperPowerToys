using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.PsiPlugin.src.Refactoring;
using JetBrains.ReSharper.Refactorings.Rename;

namespace JetBrains.ReSharper.PsiPlugin.Refactoring.Rename
{
  [RenamePart]
  internal class PsiPrimaryDeclaredElementForRenameProvider : IPrimaryDeclaredElementForRenameProvider
  {
    public IDeclaredElement GetPrimaryDeclaredElement(IDeclaredElement declaredElement, IReference reference)
    {
      var method = declaredElement as IMethod;
      if (method != null)
        return DerivedDeclaredElementUtil.GetPrimaryDeclaredElementForMethod(method);

      var @class = declaredElement as IClass;
      if (@class != null)
        return DerivedDeclaredElementUtil.GetPrimaryDeclaredElementForClass(@class);

      var @interface = declaredElement as IInterface;
      if (@interface != null)
        return DerivedDeclaredElementUtil.GetPrimaryDeclaredElementForInterface(@interface);

      return declaredElement;
    }
  }
}
