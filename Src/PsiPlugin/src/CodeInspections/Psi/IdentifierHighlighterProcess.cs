using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi.Highlightings;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi
{
  internal class IdentifierHighlighterProcess : PsiIncrementalDaemonStageProcessBase
  {
    public IdentifierHighlighterProcess(IDaemonProcess daemonProcess, IContextBoundSettingsStore settingsStore)
      : base(daemonProcess, settingsStore)
    {
    }

    public override void VisitRuleDeclaredName(IRuleDeclaredName ruleDeclaredName, IHighlightingConsumer consumer)
    {
      DocumentRange colorConstantRange = ruleDeclaredName.GetDocumentRange();
      AddHighLighting(colorConstantRange, ruleDeclaredName, consumer, new PsiRuleHighlighting(ruleDeclaredName));
      base.VisitRuleDeclaredName(ruleDeclaredName, consumer);
    }

    public override void VisitRuleName(IRuleName ruleNameParam, IHighlightingConsumer consumer)
    {
      var ruleName = ruleNameParam as RuleName;
      if (ruleName != null)
      {
        DocumentRange colorConstantRange = ruleName.GetDocumentRange();

        ResolveResultWithInfo resolve = ruleName.RuleNameReference.Resolve();

        bool isRuleResolved = resolve.Result.DeclaredElement != null || (resolve.Result.Candidates.Count > 0);
        if (isRuleResolved)
        {
          AddHighLighting(colorConstantRange, ruleName, consumer, new PsiRuleHighlighting(ruleName));
        }
        else
        {
          AddHighLighting(colorConstantRange, ruleName, consumer, new PsiUnresolvedRuleReferenceHighlighting(ruleName));
        }

        base.VisitRuleName(ruleName, consumer);
      }
    }

    public override void VisitVariableName(IVariableName variableNameParam, IHighlightingConsumer consumer)
    {
      DocumentRange colorConstantRange = variableNameParam.GetDocumentRange();
      var variableName = variableNameParam as VariableName;
      if (variableName != null)
      {
        ResolveResultWithInfo resolve = variableName.Resolve();
        if ((resolve != null) && ((resolve.Result.DeclaredElement != null) || (resolve.Result.Candidates.Count > 0)))
        {
          AddHighLighting(colorConstantRange, variableNameParam, consumer, new PsiVariableHighlighting(variableNameParam));
        }
        else
        {
          AddHighLighting(colorConstantRange, variableNameParam, consumer, new PsiUnresolvedVariableReferenceHighlighting(variableName));
        }
      }
    }

    public override void VisitPathName(IPathName pathNameParam, IHighlightingConsumer consumer)
    {
      DocumentRange colorConstantRange = pathNameParam.GetDocumentRange();
      var pathName = pathNameParam as PathName;
      if (pathName != null)
      {
        ResolveResultWithInfo resolve = pathName.Resolve();
        if ((resolve != null) && ((resolve.Result.DeclaredElement != null) || (resolve.Result.Candidates.Count > 0)))
        {
          AddHighLighting(colorConstantRange, pathNameParam, consumer, new PsiRuleHighlighting(pathNameParam));
        }
        else
        {
          AddHighLighting(colorConstantRange, pathNameParam, consumer, new PsiUnresolvedPathReferenceHighlighting(pathName));
        }
      }
    }

    private void AddHighLighting(DocumentRange range, ITreeNode element, IHighlightingConsumer consumer, IHighlighting highlighting)
    {
      var info = new HighlightingInfo(range, highlighting, new Severity?());
      IFile file = element.GetContainingFile();
      if (file != null)
      {
        consumer.AddHighlighting(info.Highlighting, file);
      }
    }
  }
}
