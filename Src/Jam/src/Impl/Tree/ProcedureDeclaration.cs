using JetBrains.ReSharper.Psi.Impl;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  internal partial class ProcedureDeclaration
  {
    int? IResolveIsolationScope.ModificationStamp { get; set; }

    public new IProcedureDeclaredElement DeclaredElement
    {
      get { return (IProcedureDeclaredElement) base.DeclaredElement; }
    }

    protected override IDeclaredElement CreateDeclaredElement(IDeclaration declaration)
    {
      return new JamProcedureDeclaredElement((IProcedureDeclaration) declaration);
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