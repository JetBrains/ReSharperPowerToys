using System;
using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Resolve;
using JetBrains.ReSharper.Feature.Services.VisualElements;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Feature.Services;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.DaemonStage
{
  internal class IdentifierHighlighterProcess : PsiIncrementalDaemonStageProcessBase
  {
    private readonly ResolveProblemHighlighter myResolveProblemHighlighter;

    public IdentifierHighlighterProcess(IDaemonProcess daemonProcess, ResolveHighlighterRegistrar resolveHighlighterRegistrar, IContextBoundSettingsStore settingsStore)
      : base(daemonProcess, settingsStore)
    {
      myResolveProblemHighlighter = new ResolveProblemHighlighter(File, resolveHighlighterRegistrar);
    }

    public override void VisitNode(ITreeNode element, IHighlightingConsumer consumer)
    {
      if ((element is ITokenNode) && ((ITokenNode)element).GetTokenType().IsWhitespace)
        return;

      DocumentRange? colorConstantRange = element.GetNavigationRange();
      RuleName ruleName = element as RuleName;
      if (ruleName != null)
      {
        ResolveResultWithInfo resolve = ruleName.Resolve();
        if ((ruleName.Parent is IRuleDeclaration) || ((resolve != null) && ((resolve.Result.DeclaredElement != null) || (resolve.Result.Candidates.Count > 0))))
        {
          addHighLighting(colorConstantRange, element, consumer, new PsiRuleHighlighting((element)));
          return;
        } else
        {
          addHighLighting(colorConstantRange, element, consumer, new PsiUnresolvedReferenceHighlighting(element));
          return;
        }
      }
      VariableName variableName = element as VariableName;
      if (variableName != null)
      {
        ResolveResultWithInfo resolve = variableName.Resolve();
        if ((resolve != null) && ((resolve.Result.DeclaredElement != null) || (resolve.Result.Candidates.Count > 0)))
        {
          addHighLighting(colorConstantRange, element, consumer, new PsiVariableHighlighting(element));
        }  else
        {
          addHighLighting(colorConstantRange, element, consumer, new PsiUnresolvedReferenceHighlighting(element));
          return;
        }    
      }
      PathName pathName = element as PathName;
      if (pathName != null)
      {
        ResolveResultWithInfo resolve = pathName.Resolve();
        if ((resolve != null) && ((resolve.Result.DeclaredElement != null) || (resolve.Result.Candidates.Count > 0)))
        {
          addHighLighting(colorConstantRange, element, consumer, new PsiRuleHighlighting(element));
        }
        else
        {
          addHighLighting(colorConstantRange, element, consumer, new PsiUnresolvedReferenceHighlighting(element));
          return;
        }
      }
    }

    private void addHighLighting(DocumentRange? range, ITreeNode element, IHighlightingConsumer consumer, IHighlighting highlighting)
    {
      HighlightingInfo info = new HighlightingInfo(range.Value, highlighting, new Severity?(), (string)null);
      while (element.Parent != null)
      {
        element = element.Parent;
      }
      IFile file = element as IFile;
      if (info != null)
      {
        consumer.AddHighlighting(info.Highlighting, file);
      }

      myResolveProblemHighlighter.CheckForResolveProblems(consumer, element);         
    }
  }
}