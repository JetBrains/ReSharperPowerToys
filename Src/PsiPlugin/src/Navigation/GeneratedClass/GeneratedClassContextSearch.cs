using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ContextNavigation.ContextSearches;
using JetBrains.ReSharper.Feature.Services.ContextNavigation.ContextSearches.BaseSearches;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Services;
using JetBrains.ReSharper.PsiPlugin.Navigation.GeneratedClass;
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.GeneratedClass
{
  [FeaturePart]
  public class GeneratedClassContextSearch :  GeneratedContextSearch<GeneratedClassSearchRequest>
  {
    protected override GeneratedClassSearchRequest CreateSearchRequest(IDataContext dataContext, IDeclaredElement declaredElement)
    {
      return new GeneratedClassSearchRequest(declaredElement);
    }

    internal override bool HasDerivedElements(RuleDeclaration ruleDeclaration)
    {
      return ruleDeclaration.DerivedClasses.Any();
    }
  }
}
