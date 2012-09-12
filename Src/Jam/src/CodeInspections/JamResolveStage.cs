using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Resolve;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.CodeInspections
{
  [DaemonStage(StagesBefore = new[] {typeof (JamIdentifierHighlightingStage)})]
  public class JamResolveStage : JamDaemonStageBase
  {
    private readonly ResolveHighlighterRegistrar myResolveHighlighterRegistrar;

    public JamResolveStage(ResolveHighlighterRegistrar resolveHighlighterRegistrar)
    {
      myResolveHighlighterRegistrar = resolveHighlighterRegistrar;
    }

    protected override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind, IJamFile file)
    {
      return new Process(process, settings, file, myResolveHighlighterRegistrar);
    }

    private class Process : JamDaemonProcessBase
    {
      private readonly JamResolveProblemHighlighter myJamResolveProblemHighlighter;

      public Process(IDaemonProcess process, IContextBoundSettingsStore settingsStore, IJamFile file, ResolveHighlighterRegistrar resolveHighlighterRegistrar) : base(process, settingsStore, file)
      {
         myJamResolveProblemHighlighter = new JamResolveProblemHighlighter(File, resolveHighlighterRegistrar);
      }

      public override void ProcessAfterInterior(ITreeNode element, IHighlightingConsumer consumer)
      {
        myJamResolveProblemHighlighter.CheckForResolveProblems(element, consumer);
      }
    }
  }
}