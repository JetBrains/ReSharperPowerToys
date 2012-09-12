using JetBrains.Application;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Jam.Cache;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Jam.Util;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Jam.Resolve
{
  internal class ProcedureReference : CheckedReferenceBase<IInvocationExpression>, ICompleteableReference
  {
    public ProcedureReference(IInvocationExpression owner) : base(owner) {}

    public override string GetName()
    {
      return myOwner.Name != null ? myOwner.Name.Name : SharedImplUtil.MISSING_DECLARATION_NAME;
    }

    public override TreeTextRange GetTreeTextRange()
    {
      return myOwner.Name != null ? myOwner.Name.GetTreeTextRange() : TreeTextRange.InvalidRange;
    }

    public override ISymbolTable GetReferenceSymbolTable(bool useReferenceName)
    {
      var symbolsCache = myOwner.GetSolution().GetComponent<JamSymbolsCache>();

      var symbolTable = JamSymbolTableBuilder.BuildProcedureTable(symbolsCache);

      if (useReferenceName)
        symbolTable = symbolTable.Filter(GetName());

      return symbolTable;
    }

    public ISymbolTable GetCompletionSymbolTable()
    {
      return GetReferenceSymbolTable(false);
    }

    public override ResolveResultWithInfo ResolveWithoutCache()
    {
      var resolveResultWithInfo = GetReferenceSymbolTable(true).GetResolveResult(GetName());
      return new ResolveResultWithInfo(resolveResultWithInfo.Result, resolveResultWithInfo.Info.CheckResolveInfo(JamResolveErrorType.PROCEDURE_NOT_RESOLVED));
    }

    public override IReference BindTo(IDeclaredElement element)
    {
      return BindTo(element, EmptySubstitution.INSTANCE);
    }

    public override IReference BindTo(IDeclaredElement element, ISubstitution substitution)
    {
      var procedureDeclaredElement = element as IProcedureDeclaredElement;
      if (procedureDeclaredElement == null)
        return this;

      var identifierExpression = JamElementFactory.GetInstance(myOwner).CreateExpression<IIdentifierExpression>("$0", procedureDeclaredElement.ShortName);
      if (identifierExpression == null)
        return this;

      using (WriteLockCookie.Create(myOwner.IsPhysical()))
      {
        var jamIdentifier = ModificationUtil.ReplaceChild(myOwner.Name, identifierExpression.LiteralToken);
        var invocationExpression = InvocationExpressionNavigator.GetByName(jamIdentifier);
        Assertion.AssertNotNull(invocationExpression, "invocationExpression is null");

        return invocationExpression.Reference;
      }
    }

    public override IAccessContext GetAccessContext()
    {
      return new ElementAccessContext(myOwner);
    }

    public override ISymbolFilter[] GetSymbolFilters()
    {
      return EmptyArray<ISymbolFilter>.Instance;
    }
  }
}