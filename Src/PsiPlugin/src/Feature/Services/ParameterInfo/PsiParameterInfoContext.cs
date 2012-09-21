using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ParameterInfo;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Completion;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services.ParameterInfo
{
  public class PsiParameterInfoContext : IParameterInfoContext
  {
    private readonly ICandidate[] myCandidates;
    private readonly TextRange myTextRange;
    //private readonly PsiServices myServices;
    private readonly IPsiSourceFile myGetSourceFile;
    private IRuleNameUsage myRuleNameUsage;

    public PsiParameterInfoContext(IRuleNameUsage ruleNameUsage, int argumentIndex)
    {
      myRuleNameUsage = ruleNameUsage;
      myCandidates = GetCandidates(ruleNameUsage);
      NamedArguments = EmptyArray<string>.Instance;
      Argument = argumentIndex;
      myTextRange = ruleNameUsage.Parameters.FirstChild.GetNavigationRange().TextRange;
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
        candidates[0] = new PsiParameterInfoCandidate(psiRuleSignature, ruleDeclaration.GetSourceFile());
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
