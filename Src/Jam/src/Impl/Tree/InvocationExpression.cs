using JetBrains.ReSharper.Psi.Jam.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  internal partial class InvocationExpression
  {
    public IReference Reference { get; private set; }

    protected override void PreInit()
    {
      base.PreInit();
      Reference = new ProcedureReference(this);
    }

    public override ReferenceCollection GetFirstClassReferences()
    {
      return new ReferenceCollection(Reference);
    }
  }
}