using System;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections
{
  public class PsiFileIndexProcess : PsiDaemonStageProcessBase
  {
    public PsiFileIndexProcess(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
      : base(process, settingsStore)
    {
    }

    public override void Execute(Action<DaemonStageResult> commiter)
    {
      HighlightInFile((file, consumer) => file.ProcessDescendants(this, consumer), commiter);
    }
  }
}
