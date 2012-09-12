using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  internal partial class GlobalVariableDeclaration
  {
    IGlobalVariableDeclaredElement IGlobalVariableDeclaration.DeclaredElement
    {
      get { return (IGlobalVariableDeclaredElement) base.DeclaredElement; }
    }

    protected override IDeclaredElement CreateDeclaredElement(IDeclaration declaration)
    {
      return new JamGlobalVariableDeclaredElement((IGlobalVariableDeclaration) declaration);
    }

    protected override IJamIdentifier Identifier
    {
      get { return Name; }
    }

    protected override void SetNameImpl(IJamIdentifier identifier)
    {
      SetName(identifier);
    }
  }
}