using System;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.PsiToCSharp.GeneratedInterface
{
  public class GeneratedInterfaceSearchRequest : GeneratedSearchRequest
  {

    public GeneratedInterfaceSearchRequest(IDeclaredElement declaredElement)
    {
      if (declaredElement == null)
        throw new ArgumentNullException("declaredElement");
      Logger.Assert(declaredElement.IsValid(), "declaredElement should be valid");

      mySolution = declaredElement.GetPsiServices().Solution;
      var ruleDeclaration = declaredElement as RuleDeclaration;
      if( ruleDeclaration == null)
        throw new ArgumentNullException("ruleDeclaration");

      if (ruleDeclaration.DerivedInterfaces.Count() > 0)
      {
        myTarget = new DeclaredElementEnvoy<IDeclaredElement>(ruleDeclaration.DerivedInterfaces.First());
      }
      else
      {
        myTarget = new DeclaredElementEnvoy<IDeclaredElement>(declaredElement);
      }
    }
  }
}
