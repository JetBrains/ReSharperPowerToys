using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.ParameterInfo;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services.ParameterInfo
{
  public class PsiParameterInfoContext : IParameterInfoContext
  {
    private readonly ICandidate[] myCandidates;
    private readonly TextRange myTextRange;

    public PsiParameterInfoContext(IRuleNameUsage ruleNameUsage, int argumentIndex)
    {
      myCandidates = GetCandidates(ruleNameUsage);
      NamedArguments = EmptyArray<string>.Instance;
      Argument = argumentIndex;
      if (ruleNameUsage.Parameters.FirstChild != null)
      {
        myTextRange = ruleNameUsage.Parameters.FirstChild.GetNavigationRange().TextRange;
      } else
      {
        myTextRange = TextRange.InvalidRange;
      }
    }

    #region Implementation of IParameterInfoContext

    public int Argument { get; private set; }
    public string[] NamedArguments { get; set; }

    private ICandidate[] GetCandidates(IRuleNameUsage ruleNameUsage)
    {
      var ruleName = ruleNameUsage.Name;
      var ruleDeclaration = ruleName.RuleNameReference.Resolve().DeclaredElement as IRuleDeclaration;
      if(ruleDeclaration != null)
      {
        var psiRuleSignature = new PsiRuleSignature(ruleDeclaration);
        var candidates = new ICandidate[1];
        candidates[0] = new PsiParameterInfoCandidate(psiRuleSignature);
        return candidates;
      }
      return null;
    }

    public ICandidate DefaultCandidate
    {
      get
      {
        return myCandidates[0];
      }
    }
    public ICandidate[] Candidates
    {
      get
      {
        return myCandidates;
      }
    }
    public TextRange Range
    {
      get { return myTextRange; }
    }
    public Type ParameterListNodeType 
    { 
      get { return typeof (IRuleParameters); }
    }
    public ICollection<Type> ParameterNodeTypes
    {
      get { return new[] { typeof(IRuleDeclaration) }; }
    }

    #endregion
  }
}
