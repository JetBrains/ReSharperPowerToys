using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services.ParameterInfo
{
  public class PsiRuleSignature
  {
    private IList<IDeclaredElement> myParameters = new List<IDeclaredElement>();

    public PsiRuleSignature(IRuleDeclaration ruleDeclaration)
    {
      var parameters = ruleDeclaration.Parameters;
      if(parameters != null)
      {
        var child = parameters.FirstChild;
        while(child != null)
        {
          var ruleName = child as IRuleName;
          if(ruleName != null)
          {
            var declaredElement = ruleName.RuleNameReference.Resolve().DeclaredElement;
            if (declaredElement != null)
            {
              myParameters.Add(declaredElement);
            } else
            {
              var candidates = ruleName.RuleNameReference.Resolve().Result.Candidates;
              foreach(var candidate in candidates)
              {
                if (candidate is IRuleDeclaration)
                {
                  myParameters.Add(candidate);
                  break;
                }
              }
            }
          }
          child = child.NextSibling;
        }
      }
    }

    public IList<IDeclaredElement> Parameters { 
      get
      {
        return myParameters;
      }
    }
  }
}