using JetBrains.Application.DataContext;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ContextNavigation.ContextSearches.BaseSearches;
using JetBrains.ReSharper.Feature.Services.Search.SearchRequests;
using JetBrains.ReSharper.Psi.Services;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.PsiToCSharp
{
  public abstract class GeneratedContextSearch<TSearchRequest> : ContextSearchBase<TSearchRequest>
    where TSearchRequest : SearchRequest
  {
    internal abstract bool HasDerivedElements(RuleDeclaration ruleDeclaration);

    public override bool IsAvailable(IDataContext dataContext)
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
      } else if (token.Parent is IPathName)
      {
        var reference = (token.Parent as IPathName).RuleNameReference;
        var declaredElement = reference.Resolve().DeclaredElement;
        ruleDeclaration = declaredElement as RuleDeclaration;        
      }
      if (ruleDeclaration != null)
      {
        var psiFile = ruleDeclaration.GetContainingNode<PsiFile>();
        if (psiFile != null) psiFile.CollectDerivedElements(ruleDeclaration);
        return HasDerivedElements(ruleDeclaration);
      }

      return false;
    }

    public override bool IsContextApplicable(IDataContext dataContext)
    {
      var textControl = dataContext.GetData(TextControl.DataContext.DataConstants.TEXT_CONTROL);
      ISolution solution = dataContext.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);

      var token = TextControlToPsi.GetSourceTokenAtCaret(solution, textControl);
      if ((token.Parent is IRuleName) || (token.Parent is IRuleDeclaredName) || (token.Parent is PathName) )
        return true;
      return false;
    }

    protected override bool IsAvailableInternal(IDataContext dataContext)
    {
      return IsAvailable(dataContext);
    }
  }
}
