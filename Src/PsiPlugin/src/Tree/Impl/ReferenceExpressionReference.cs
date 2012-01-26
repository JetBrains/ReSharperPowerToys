using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  /*internal class ReferenceExpressionReference : TreeReferenceBase<IReferenceExpressionReferenceOwner>, IReferenceExpressionReference, IQualifiableReference
  {
    public ReferenceExpressionReference(IReferenceExpressionReferenceOwner owner) : base(owner)
    {
    }

    public override ResolveResultWithInfo ResolveWithoutCache()
    {
      var services = myOwner.GetPsiServices().Solution.GetComponent<JavaScriptServices>();
      var context = JavaScriptResolveContext.CreateInitialContext(services, myOwner.GetSourceFile());
      return ResolveWithContextWithoutCache(GetSymbolTableInternal(context), services);
    }

    public override string GetName()
    {
      return myOwner.GetReferenceExpressionReferenceName();
    }

    public ISymbolTable GetSymbolTable(JavaScriptResolveContext resolveContext)
    {
      var allSymbolTable = GetSymbolTableInternal(resolveContext);

      var kind = JsQualifierKind.General;

      if (resolveContext.SymbolTableMode.SmartProperties && !IsQualified)
        kind = JsQualifierKind.SmartFilterTop;

      var filters = new List<ISymbolFilter> { new JavaScriptLevelsFilter(), new VisibilityFilter(GetElement().GetSourceFile(), kind, resolveContext.JavaScriptServices) };

      return new MultipleFilterSymbolTable(allSymbolTable, filters.ToArray());
    }

    public override ISymbolTable GetReferenceSymbolTable(bool useReferenceName)
    {
      var services = myOwner.GetPsiServices().Solution.GetComponent<JavaScriptServices>();
      return GetSymbolTableInternal(JavaScriptResolveContext.CreateInitialContext(services, myOwner.GetSourceFile()));
    }

    private ISymbolTable GetSymbolTableInternal(JavaScriptResolveContext context)
    {
      var qualifier = GetQualifier(context);
      if (qualifier != null)
        return qualifier.GetSymbolTable(context.SymbolTableMode);

      return ResolveUtil.BuildSymbolTableForReference(this, context.SymbolTableMode);
    }

    public override TreeTextRange GetTreeTextRange()
    {
      return myOwner.GetReferenceExpressionrReferenceTreeRange();
    }

    public override IReference BindTo(IDeclaredElement element)
    {
      return JavaScriptReferenceBindingUtil.BindReferenceTo((IReferenceExpressionReference)this, element, myOwner.BindingModification);
    }

    public override IReference BindTo(IDeclaredElement element, ISubstitution substitution)
    {
      return BindTo(element);
    }

    public override IAccessContext GetAccessContext()
    {
      return new JavaScriptReferenceAccessContext(myOwner.GetPsiModule());
    }

    private ISymbolFilter[] GetSymbolFilters(JavaScriptServices services)
    {
      if (IsQualified)
        return new ISymbolFilter[] {
          new ExactNameFilter(GetName()),
          GetGeneralVisibilityFilter(services),
        };
      return new ISymbolFilter[] {
        new ExactNameFilter(GetName()),
        GetGeneralVisibilityFilter(services),
        new JavaScriptLevelsFilter()
      };
    }

    private VisibilityFilter GetGeneralVisibilityFilter(JavaScriptServices services)
    {
      var element = GetElement();
      var file = element.GetContainingFile();
      if (file == null)
        return new VisibilityFilter(element.GetSourceFile(), JsQualifierKind.General, services);
      var ret = file.PersistentUserData.GetData(KeyCachedVisibility);
      if (ret == null)
      {
        ret = new VisibilityFilter(file.GetSourceFile(), JsQualifierKind.General, services);
        file.PersistentUserData.PutData(KeyCachedVisibility, ret);
      }
      return ret;
    }

    private ResolveResultWithInfo ResolveWithContextWithoutCache(ISymbolTable symbolTable, JavaScriptServices services)
    {
      if (symbolTable == EmptySymbolTable.INSTANCE)
        return ResolveResultWithInfo.Unresolved;
      if (symbolTable == EmptySymbolTable.INSTANCE)
        return ResolveResultWithInfo.Unresolved;
      
      var filters = GetSymbolFilters(services);
      var name = GetName();
      symbolTable = symbolTable.Filter(name, filters);

      var resolveResult = symbolTable.GetResolveResult(GetName());

      if (resolveResult.ResolveErrorType != ResolveErrorType.NOT_RESOLVED && resolveResult.DeclaredElement != null && resolveResult.Result.Candidates.Count == 0)
        return new ResolveResultWithInfo(resolveResult.Result, ResolveErrorType.OK);

      // symbol doesn't have declarations...
      return new ResolveResultWithInfo(ResolveResultFactory.CreateResolveResult(new JavaScriptNotDeclaredProperty(GetName(), GetElement().GetSourceFile(), myOwner.GetPsiServices())), ResolveErrorType.OK);
    }

    private readonly CachedPsiValue<ResolveResultWithInfo> myResult = new CachedPsiValue<ResolveResultWithInfo>();
    private static readonly Key<VisibilityFilter> KeyCachedVisibility = new Key<VisibilityFilter>("myKeyCachedVisibility");

    public ResolveResultWithInfo Resolve(JavaScriptResolveContext context)
    {
      return myResult.GetValue(myOwner, ResolveWithoutCache);
    }

    public IJavaScriptType CalculateJsType(JavaScriptResolveContext context)
    {
      var typesFactory = context.TypesFactory;
      if (GetName() == JavaScriptPsiImplUtil.PrototypePropertyName)
      {
        // special handling of prototype property
        var qualifier = myOwner.GetQualifier(context);
        if (qualifier != null)
          return qualifier.GetJsType(context).GetConstructedType(context);
      }
      if (GetName() == JavaScriptPsiImplUtil.WindowPropertyName)
      {
        // special handling of window global property
        var qualifier = myOwner.GetQualifier(context);
        if (qualifier == null)
          return typesFactory.CreateWindowType(context, myOwner.GetSourceFile());
      }

      var declaredElement = Resolve(context).DeclaredElement as IJavaScriptTypeOwner;
      if (declaredElement == null)
        return context.TypesFactory.UnknownType;
      
      return declaredElement.GetJsType(context);
      
    }

    private static readonly Func<ReferenceExpressionReference, ResolveResultWithInfo> ResolveWithoutCacheFunc = reference => reference.ResolveWithoutCache();

    public override ResolveResultWithInfo Resolve()
    {
      return ResolveUtil.ResolveWithCache(this, ResolveWithoutCacheFunc);
    }

    public bool IsQualified
    {
      get
      {
        return myOwner.IsQualified;
      }
    }

    ResolveResultWithInfo IQualifiableReference.Resolve(ISymbolTable symbolTable, IAccessContext context)
    {
      return ResolveUtil.ResolveWithCache(this, _ => ResolveWithContextWithoutCache(symbolTable, myOwner.GetPsiServices().Solution.GetComponent<JavaScriptServices>()));
    }

    IQualifier IQualifiableReference.GetQualifier()
    {
      return GetQualifier(JavaScriptResolveContext.CreateInitialContext(myOwner.GetPsiServices().Solution.GetComponent<JavaScriptServices>(), myOwner.GetSourceFile()));
    }

    private IJavaScriptQualifier GetQualifier(JavaScriptResolveContext resolveContext)
    {
      return myOwner.GetQualifier(resolveContext);
    }
  }*/
}
