using JetBrains.Application;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Jam.Cache;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Jam.Util;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.Util;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ProjectModel;

namespace JetBrains.ReSharper.Psi.Jam.Resolve
{
  internal class VariableReference : CheckedReferenceBase<IIdentifierExpression>, ICompleteableReference
  {
    public VariableReference(IIdentifierExpression owner) : base(owner) {}

    public override string GetName()
    {
      return myOwner.LiteralToken != null ? myOwner.LiteralToken.Name : SharedImplUtil.MISSING_DECLARATION_NAME;
    }

    public override TreeTextRange GetTreeTextRange()
    {
      return myOwner.LiteralToken != null ? myOwner.LiteralToken.GetTreeTextRange() : TreeTextRange.InvalidRange;
    }

    public override ISymbolTable GetReferenceSymbolTable(bool useReferenceName)
    {
      var symbolsCache = myOwner.GetSolution().GetComponent<JamSymbolsCache>();

      var symbolTable = JamSymbolTableBuilder.BuildLocalVariableTable(myOwner);
      symbolTable = symbolTable.Merge(JamSymbolTableBuilder.BuildParameterTable(myOwner));
      symbolTable = symbolTable.Merge(JamSymbolTableBuilder.BuildGlobalVariableTable(symbolsCache));

      if (useReferenceName)
        symbolTable = symbolTable.Filter(GetName());

      return symbolTable;
    }

    public ISymbolTable GetCompletionSymbolTable()
    {
      return GetReferenceSymbolTable(false).Merge(JamSymbolTableBuilder.BuildProcedureTable(myOwner.GetSolution().GetComponent<JamSymbolsCache>()));
    }

    public override ResolveResultWithInfo ResolveWithoutCache()
    {
      var resolveResultWithInfo = GetReferenceSymbolTable(true).GetResolveResult(GetName());
      return new ResolveResultWithInfo(resolveResultWithInfo.Result, resolveResultWithInfo.Info.CheckResolveInfo(JamResolveErrorType.VARIABLE_NOT_RESOLVED));
    }

    public override IReference BindTo(IDeclaredElement element)
    {
      return BindTo(element, EmptySubstitution.INSTANCE);
    }

    public override IReference BindTo(IDeclaredElement element, ISubstitution substitution)
    {
      var variableDeclaredElement = element as IVariableDeclaredElement;
      if (variableDeclaredElement == null)
        return this;

      var identifierExpression = JamElementFactory.GetInstance(myOwner).CreateExpression<IIdentifierExpression>("$0", variableDeclaredElement.ShortName);
      if (identifierExpression == null)
        return this;

      using (WriteLockCookie.Create(myOwner.IsPhysical()))
      {
        var expression = ModificationUtil.ReplaceChild(myOwner, identifierExpression);
        return expression.Reference;
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