using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  internal partial class Parameter
  {
    public new IParameterDeclaredElement DeclaredElement
    {
      get { return (IParameterDeclaredElement) base.DeclaredElement; }
    }

    protected override IDeclaredElement CreateDeclaredElement(IDeclaration declaration)
    {
      return new JamParameterDeclaredElement((Jam.Tree.IParameter) declaration);
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