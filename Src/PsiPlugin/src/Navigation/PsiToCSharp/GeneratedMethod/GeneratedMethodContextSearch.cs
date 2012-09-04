using System.Linq;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.PsiToCSharp.GeneratedMethod
{
  [FeaturePart]
  public class GeneratedMethodContextSearch : GeneratedContextSearch<GeneratedMethodSearchRequest>
  {
    protected override GeneratedMethodSearchRequest CreateSearchRequest(IDataContext dataContext, IDeclaredElement declaredElement, IDeclaredElement initialTarget)
    {
      return new GeneratedMethodSearchRequest(declaredElement);
    }

    internal override bool HasDerivedElements(RuleDeclaration ruleDeclaration)
    {
      return ruleDeclaration.DerivedParserMethods.Any();
    }
  }
}
