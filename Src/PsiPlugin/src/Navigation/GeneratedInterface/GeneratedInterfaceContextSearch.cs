using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.GeneratedInterface
{
  [FeaturePart]
  public class GeneratedInterfaceContextSearch : GeneratedContextSearch<GeneratedInterfaceSearchRequest>
  {
    protected override GeneratedInterfaceSearchRequest CreateSearchRequest(IDataContext dataContext, IDeclaredElement declaredElement, IDeclaredElement initialTarget)
    {
      return new GeneratedInterfaceSearchRequest(declaredElement);
    }

    internal override bool HasDerivedElements(RuleDeclaration ruleDeclaration)
    {
      return ruleDeclaration.DerivedInterfaces.Any();
    }
  }
}
