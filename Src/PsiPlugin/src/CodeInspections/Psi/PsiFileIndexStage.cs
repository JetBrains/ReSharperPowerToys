using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi
{
  [DaemonStage]
  public class PsiFileIndexStage : PsiDaemonStageBase
  {
    public override ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
    {
      return IsSupported(sourceFile) ? ErrorStripeRequest.STRIPE_AND_ERRORS : ErrorStripeRequest.NONE;
    }

    public override IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
    {
      if (!IsSupported(process.SourceFile))
      {
        return EmptyList<IDaemonStageProcess>.InstanceList;
      }
      return new List<IDaemonStageProcess> { new PsiFileIndexProcess(process, settings) };
    }
  }
}
