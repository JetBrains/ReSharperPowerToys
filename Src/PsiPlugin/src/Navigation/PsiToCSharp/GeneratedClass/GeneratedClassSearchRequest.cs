using System;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.PsiToCSharp.GeneratedClass
{
  public class GeneratedClassSearchRequest : GeneratedSearchRequest
  {

    public GeneratedClassSearchRequest(IDeclaredElement declaredElement)
    {
      if (declaredElement == null)
        throw new ArgumentNullException("declaredElement");
      Logger.Assert(declaredElement.IsValid(), "declaredElement should be valid");

      mySolution = declaredElement.GetPsiServices().Solution;
      var ruleDeclaration = declaredElement as RuleDeclaration;
      if( ruleDeclaration == null)
        throw new ArgumentNullException("ruleDeclaration");

      if (ruleDeclaration.DerivedClasses.Count() > 0)
      {
        myTarget = new DeclaredElementEnvoy<IDeclaredElement>(ruleDeclaration.DerivedClasses.First());
      }
      else
      {
        myTarget = new DeclaredElementEnvoy<IDeclaredElement>(declaredElement);
      }
    }
  }
}
