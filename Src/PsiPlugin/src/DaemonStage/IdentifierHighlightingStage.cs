using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages.Resolve;
using JetBrains.ReSharper.Daemon.UsageChecking;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.PsiPlugin.Tree;

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
}
