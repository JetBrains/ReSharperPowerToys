using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Web.References;
using JetBrains.ReSharper.Psi.Web.Util;
using JetBrains.ReSharper.PsiPlugin.Util;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  public class PsiFileReference<TOwner, TToken> : PathReferenceBase<TOwner, TToken>
    where TOwner : ITreeNode
    where TToken : class, ITreeNode
  {
    public PsiFileReference(TOwner owner, IQualifier qualifier, TToken token, TreeTextRange rangeWithin)
      : base(owner, qualifier, token, rangeWithin)
    {
    }

    protected override IReference BindToInternal(IDeclaredElement declaredElement, ISubstitution substitution)
    {
      PsiTreeUtil.ReplaceChild(myOwner, myOwner.FirstChild, declaredElement.ShortName);
      return this;
    }

    public override ResolveResultWithInfo Resolve(ISymbolTable symbolTable, IAccessContext context)
    {
      return WebPathReferenceUtil.CheckResolveResut(this, base.Resolve(symbolTable, context));
    }

    public override ISymbolTable GetReferenceSymbolTable(bool useReferenceName)
    {
      return PsiPathReferenceUtil.GetReferenceSymbolTable(this, useReferenceName);
    }
  }
}
