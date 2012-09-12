using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Jam.Impl
{
  internal class JamProcedureDeclaredElement : JamDeclaredElementBase<IProcedureDeclaration>, IProcedureDeclaredElement
  {
    public JamProcedureDeclaredElement(IProcedureDeclaration declaration) : base(declaration) {}

    public override DeclaredElementType GetElementType()
    {
      return JamDeclaredElementType.Procedure;
    }

    public IList<IParameterDeclaredElement> Parameters
    {
      get
      {
        var declaration = GetDeclaration();
        if (declaration == null)
          return EmptyList<IParameterDeclaredElement>.InstanceList;

        var parameterList = declaration.ParameterList;
        if (parameterList == null)
          return EmptyList<IParameterDeclaredElement>.InstanceList;

        if (parameterList.Parameters.IsEmpty)
          return EmptyList<IParameterDeclaredElement>.InstanceList;

        return parameterList.Parameters.Select(parameter => parameter.DeclaredElement).ToList();
      }
    }

    protected override IProcedureDeclaration GetDeclaration(IJamIdentifier identifier)
    {
      return ProcedureDeclarationNavigator.GetByName(identifier);
    }
  }
}