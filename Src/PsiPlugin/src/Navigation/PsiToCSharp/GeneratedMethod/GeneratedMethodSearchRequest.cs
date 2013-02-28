using System;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
using JetBrains.Util;
using JetBrains.Util.Logging;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.PsiToCSharp.GeneratedMethod
{
  public class GeneratedMethodSearchRequest : GeneratedSearchRequest
  {
    public GeneratedMethodSearchRequest(IDeclaredElement declaredElement)
    {
      if (declaredElement == null)
        throw new ArgumentNullException("declaredElement");
      Logger.Assert(declaredElement.IsValid(), "declaredElement should be valid");

      mySolution = declaredElement.GetPsiServices().Solution;
      var ruleDeclaration = declaredElement as RuleDeclaration;
      if( ruleDeclaration == null)
        throw new ArgumentNullException("ruleDeclaration");

      if (ruleDeclaration.DerivedParserMethods.Count() > 0)
      {
        myTarget = new DeclaredElementEnvoy<IDeclaredElement>(ruleDeclaration.DerivedParserMethods.First());
      }
      else
      {
        myTarget = new DeclaredElementEnvoy<IDeclaredElement>(declaredElement);
      }
    }
  }
}
