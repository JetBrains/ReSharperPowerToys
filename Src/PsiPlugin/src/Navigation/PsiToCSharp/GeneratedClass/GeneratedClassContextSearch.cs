using System.Linq;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.PsiToCSharp.GeneratedClass
{
  [FeaturePart]
  public class GeneratedClassContextSearch :  GeneratedContextSearch<GeneratedClassSearchRequest>
  {
    protected override GeneratedClassSearchRequest CreateSearchRequest(IDataContext dataContext, IDeclaredElement declaredElement, IDeclaredElement initialTarget)
    {
      return new GeneratedClassSearchRequest(declaredElement);
    }

    internal override bool HasDerivedElements(RuleDeclaration ruleDeclaration)
    {
      return ruleDeclaration.DerivedClasses.Any();
    }
  }
}
