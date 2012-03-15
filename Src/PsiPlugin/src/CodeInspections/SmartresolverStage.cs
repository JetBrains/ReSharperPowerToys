﻿using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections
{
  [DaemonStage(StagesBefore = new[] { typeof(GlobalFileStructureCollectorStage) }, StagesAfter = new[] { typeof(IdentifierHighlightingStage) })]
  public class SmartResolverStage : PsiDaemonStageBase
  {
    public override ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
    {
      return ErrorStripeRequest.NONE;
    }

    public override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
    {
      if (!IsSupported(process.SourceFile))
        return null;
      return new SmartResolverProcess(process);
    }
  }
}