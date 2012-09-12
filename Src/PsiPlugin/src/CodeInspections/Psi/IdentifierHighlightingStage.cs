﻿using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.UsageChecking;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi
{
  [DaemonStage(StagesBefore = new[] { typeof (SmartResolverStage) }, StagesAfter = new[] { typeof (CollectUsagesStage) })]
  public class IdentifierHighlightingStage : PsiDaemonStageBase
  {
    public override IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
    {
      if (!IsSupported(process.SourceFile))
      {
        return EmptyList<IDaemonStageProcess>.InstanceList;
      }
      return new List<IDaemonStageProcess> { new IdentifierHighlighterProcess(process, settings) };
    }
  }
}
