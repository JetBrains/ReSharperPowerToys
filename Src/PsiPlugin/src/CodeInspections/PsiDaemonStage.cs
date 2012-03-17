using System;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.DaemonStage
{
    class PsiDaemonStage : IDaemonStage
    {
        public IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
        {
            throw new NotImplementedException();
        }

        public ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settingsStore)
        {
            throw new NotImplementedException();
        }
    }
}
