using System;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ContextNavigation.ContextSearches.BaseSearches;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Services;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.ReSharper.PsiPlugin.src.Refactoring;
using JetBrains.Util.Collections;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.CSharpToPsi
{
  [FeaturePart]
  public class CSharpToPsiContextSearch : ContextSearchBase<CSharpToPsiSearchRequest>
  {
    protected override CSharpToPsiSearchRequest CreateSearchRequest(IDataContext dataContext, IDeclaredElement declaredElement, IDeclaredElement initialTarget)
    {
      return new CSharpToPsiSearchRequest(declaredElement);
    }

    public override bool IsAvailable(IDataContext dataContext)
    {
      var textControl = dataContext.GetData(TextControl.DataContext.DataConstants.TEXT_CONTROL);
      ISolution solution = dataContext.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      var token = TextControlToPsi.GetSourceTokenAtCaret(solution, textControl);

      var referenceName = token.Parent as IReferenceName;
      if (referenceName != null)
      {
        if (referenceName.Reference.CurrentResolveResult != null)
        {
          var declaredElement = referenceName.Reference.CurrentResolveResult.DeclaredElement;

          var @class = declaredElement as IClass;
          if(@class != null)
          {
            return DerivedDeclaredElementUtil.GetPrimaryDeclaredElementForClass(@class) != null;
          }

          var @method = declaredElement as  IMethod;
          if(@method != null)
          {
            return DerivedDeclaredElementUtil.GetPrimaryDeclaredElementForMethod(@method) != null;
          }

          var @interface = declaredElement as IInterface;
          if(@interface != null)
          {
            return DerivedDeclaredElementUtil.GetPrimaryDeclaredElementForInterface(@interface) != null;
          }
        }
        return false;
      }

      var classDeclaration = token.Parent as IClassDeclaration;
      if (classDeclaration != null)
      {
        var @class = classDeclaration.DeclaredElement as IClass;

        return ((@class != null) && (DerivedDeclaredElementUtil.GetPrimaryDeclaredElementForClass(@class) != null));
      }

      var methodDeclaration = token.Parent as IMethodDeclaration;
      if (methodDeclaration != null)
      {
        var @method = methodDeclaration.DeclaredElement;

        return ((@method != null) && (DerivedDeclaredElementUtil.GetPrimaryDeclaredElementForMethod(@method) != null));
      }

      var interfaceDeclaration = token.Parent as IInterfaceDeclaration;
      if (interfaceDeclaration != null)
      {
        var @interface = interfaceDeclaration.DeclaredElement as IInterface;

        return ((@interface != null) && (DerivedDeclaredElementUtil.GetPrimaryDeclaredElementForInterface(@interface) != null));
      }

      var constructorDeclaration = token.Parent as IConstructorDeclaration;
      if (constructorDeclaration != null)
      {
        var @class = constructorDeclaration.GetContainingTypeDeclaration().DeclaredElement as IClass;

        return ((@class != null) && (DerivedDeclaredElementUtil.GetPrimaryDeclaredElementForClass(@class) != null));
      }

      return false;
    }

    public override bool IsApplicable(IDataContext dataContext)
    {
      //return IsAvailable(dataContext);

      var textControl = dataContext.GetData(TextControl.DataContext.DataConstants.TEXT_CONTROL);
      ISolution solution = dataContext.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);

      var token = TextControlToPsi.GetSourceTokenAtCaret(solution, textControl);
      if ((token.Parent is IReferenceName) || token.Parent is IClassDeclaration || token.Parent is IConstructorDeclaration || token.Parent is IMethodDeclaration || token.Parent is IInterfaceDeclaration)
        return true;
      return false;
    }

    protected override bool IsAvailableInternal(IDataContext dataContext)
    {
      return IsAvailable(dataContext);
    }
  }
}