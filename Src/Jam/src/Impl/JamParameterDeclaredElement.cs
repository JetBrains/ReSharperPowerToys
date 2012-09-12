using JetBrains.ReSharper.Psi.Jam.Tree;

namespace JetBrains.ReSharper.Psi.Jam.Impl
{
  internal class JamParameterDeclaredElement : JamDeclaredElementBase<Jam.Tree.IParameter>, IParameterDeclaredElement
  {
    public JamParameterDeclaredElement(Jam.Tree.IParameter declaration) : base(declaration) {}

    public override DeclaredElementType GetElementType()
    {
      return JamDeclaredElementType.Parameter;
    }

    public IProcedureDeclaredElement ContainingProcedure
    {
      get
      {
        var procedureDeclaration = ProcedureDeclarationNavigator.GetByParameterList(ParameterListNavigator.GetByParameter(GetDeclaration()));
        return procedureDeclaration == null ? null : procedureDeclaration.DeclaredElement;
      }
    }

    protected override Jam.Tree.IParameter GetDeclaration(IJamIdentifier identifier)
    {
      return ParameterNavigator.GetByName(identifier);
    }
  }
}