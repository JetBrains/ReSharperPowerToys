using JetBrains.ReSharper.Psi.Jam.Tree;

namespace JetBrains.ReSharper.Psi.Jam.Impl
{
  internal class JamGlobalVariableDeclaredElement : JamDeclaredElementBase<IGlobalVariableDeclaration>, IGlobalVariableDeclaredElement
  {
    public JamGlobalVariableDeclaredElement(IGlobalVariableDeclaration declaration) : base(declaration) {}

    public override DeclaredElementType GetElementType()
    {
      return JamDeclaredElementType.GlobalVariable;
    }

    protected override IGlobalVariableDeclaration GetDeclaration(IJamIdentifier identifier)
    {
      return GlobalVariableDeclarationNavigator.GetByName(identifier);
    }
  }
}