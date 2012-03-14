using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Resolve;
using JetBrains.ReSharper.Daemon.UsageChecking;
using JetBrains.ReSharper.Feature.Services.VisualElements;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Feature.Services;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.DaemonStage
{
  [DaemonStage(StagesBefore = new[] { typeof(SmartResolverStage) }, StagesAfter = new[] { typeof(CollectUsagesStage) })]
  public class IdentifierHighlightingStage : PsiDaemonStageBase
  {
    private readonly ResolveHighlighterRegistrar myResolveHighlighterRegistrar;

    public IdentifierHighlightingStage(ResolveHighlighterRegistrar resolveHighlighterRegistrar)
    {
      myResolveHighlighterRegistrar = resolveHighlighterRegistrar;
    }

    public override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
    {
      if (!IsSupported(process.SourceFile))
        return null;
      return new IdentifierHighlighterProcess(process, myResolveHighlighterRegistrar, settings);
    }
  }

  internal class IdentifierHighlighterProcess : PsiIncrementalDaemonStageProcessBase
  {
    private readonly bool myIdentifierHighlightingEnabled;
    private readonly ResolveProblemHighlighter myResolveProblemHighlighter;
    //private readonly PsiIdentifierHighlighter myIdentifierHighlighter;
    private readonly VisualElementHighlighter myVisualElementHighlighter;

    public IdentifierHighlighterProcess(IDaemonProcess daemonProcess, ResolveHighlighterRegistrar resolveHighlighterRegistrar, IContextBoundSettingsStore settingsStore)
      : base(daemonProcess, settingsStore)
    {

      //myIdentifierHighlightingEnabled = settingsStore.GetValue(HighlightingSettingsAccessor.IdentifierHighlightingEnabled);
      myResolveProblemHighlighter = new ResolveProblemHighlighter(File, resolveHighlighterRegistrar);
      //myIdentifierHighlighter = new PsiIdentifierHighlighter(File);
      myVisualElementHighlighter = new VisualElementHighlighter(PsiLanguage.Instance, settingsStore);
    }

    private bool IdentifierHighlightingEnabled
    {
      get { return myIdentifierHighlightingEnabled; }
    }

    public override void Execute(Action<DaemonStageResult> commiter)
    {
      // Override is not redundant to see this stage in profiler
      base.Execute(commiter);
    }

    public override void VisitNode(ITreeNode element, IHighlightingConsumer consumer)
    {
      if ((element is ITokenNode) && ((ITokenNode)element).GetTokenType().IsWhitespace)
        return;

      //if (IdentifierHighlightingEnabled)
        //myIdentifierHighlighter.Highlight(element, (range, highlighting) => consumer.AddHighlighting(highlighting, range));

     // HighlightingInfo info = myVisualElementHighlighter.CreateColorHighlightingInfo(element);
      //IColorReference colorReference = this.myVisualElementFactory.GetColorReference(element);
      //DocumentRange? colorConstantRange = colorReference.ColorConstantRange;
      DocumentRange? colorConstantRange = element.GetNavigationRange();
      RuleName ruleName = element as RuleName;
      if (ruleName != null)
      {
        ResolveResultWithInfo resolve = ruleName.Resolve();
        if ((resolve != null) && ((resolve.Result.DeclaredElement != null) || (resolve.Result.Candidates.Count > 0)))
        {
          addHighLighting(colorConstantRange, element, consumer);
          return;
        }
      }
    }

    private void addHighLighting(DocumentRange? range, ITreeNode element, IHighlightingConsumer consumer)
    {
      //IColorReference colorReference = this.myVisualElementFactory.GetColorReference(element);
      HighlightingInfo info = new HighlightingInfo(range.Value, new PsiIdentifierHighlighting(element),new Severity?(), (string)null);
      //HighlightingInfo info = new HighlightingInfo(range.Value, new ColorHighlighting(element),new Severity?(), (string)null);
      //HighlightingInfo info = myVisualElementHighlighter.CreateColorHighlightingInfo(element);
      while (element.Parent != null)
      {
        element = element.Parent;
      }
      IFile file = element as IFile;
      if (info != null)
      {
        consumer.AddHighlighting(info.Highlighting, file);
      }
      //if (info != null) consumer.AddHighlighting(new PsiKeywordHighlighting(element), file);

      myResolveProblemHighlighter.CheckForResolveProblems(consumer, element);
      //SyntaxErrorHighlighter.Run(consumer, element);            
    }
  }
}
