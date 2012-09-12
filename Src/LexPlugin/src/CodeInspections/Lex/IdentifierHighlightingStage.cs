using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.UsageChecking;
using JetBrains.Util;

namespace JetBrains.ReSharper.LexPlugin.CodeInspections.Lex
{
  [DaemonStage(StagesBefore = new[] { typeof(SmartResolverStage) }, StagesAfter = new[] { typeof(CollectUsagesStage) })]
  public class IdentifierHighlightingStage : LexDaemonStageBase
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
