using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.UsageChecking;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections
{
  [DaemonStage(StagesBefore = new[] { typeof(SmartResolverStage) }, StagesAfter = new[] { typeof(CollectUsagesStage) })]
  public class IdentifierHighlightingStage : PsiDaemonStageBase
  {
    public override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
    {
      if (!IsSupported(process.SourceFile))
        return null;
      return new IdentifierHighlighterProcess(process, settings);
    }
  }
}
