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
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Navigation
{
  [FeaturePart]
  public class GeneratedContextSearch :  ContextSearchBase<GeneratedSearchRequest>
  {

    public bool IsAvailable(IDataContext dataContext)
    {
      var textControl = dataContext.GetData(TextControl.DataContext.DataConstants.TEXT_CONTROL);
      ISolution solution = dataContext.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      var token = TextControlToPsi.GetSourceTokenAtCaret(solution, textControl);
      RuleDeclaration ruleDeclaration = null;
      if (token.Parent is IRuleDeclaredName)
      {
        ruleDeclaration = token.Parent.Parent as RuleDeclaration;
      }
      else if (token.Parent is IRuleName)
      {
        var reference = (token.Parent as IRuleName).RuleNameReference;
        var declaredElement = reference.Resolve().DeclaredElement;
        ruleDeclaration = declaredElement as RuleDeclaration;
      }
      if (ruleDeclaration != null && ruleDeclaration.DerivedClasses.Any())
      {
        return true;
      }

      return false;
    }

    public override bool IsApplicable(IDataContext dataContext)
    {
      var textControl = dataContext.GetData(TextControl.DataContext.DataConstants.TEXT_CONTROL);
      ISolution solution = dataContext.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);

      var token = TextControlToPsi.GetSourceTokenAtCaret(solution, textControl);
      if ((token.Parent is IRuleName) || (token.Parent is IRuleDeclaredName))
        return true;
      return false;
    }

    protected override GeneratedSearchRequest CreateSearchRequest(IDataContext dataContext, IDeclaredElement declaredElement)
    {
      return new GeneratedSearchRequest(declaredElement);
    }

    protected override bool IsAvailableInternal(IDataContext dataContext)
    {
      return IsAvailable(dataContext);
    }
  }
}
