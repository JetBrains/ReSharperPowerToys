using JetBrains.ReSharper.Psi.Jam.Tree;

namespace JetBrains.ReSharper.Psi.Jam.Impl
{
  internal class JamLocalVariableDeclaredElement : JamDeclaredElementBase<ILocalVariableDeclaration>, ILocalVariableDeclaredElement
  {
    public JamLocalVariableDeclaredElement(ILocalVariableDeclaration declaration) : base(declaration) {}

    public override DeclaredElementType GetElementType()
    {
      return JamDeclaredElementType.LocalVariable;
    }

    protected override ILocalVariableDeclaration GetDeclaration(IJamIdentifier identifier)
    {
      return LocalVariableDeclarationStatementNavigator.GetByName(identifier);
    }
  }
}