using System;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi
{
  public class PsiFileIndexProcess : PsiDaemonStageProcessBase
  {
    public PsiFileIndexProcess(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
      : base(process, settingsStore)
    {
    }

    public override void Execute(Action<DaemonStageResult> committer)
    {
      HighlightInFile((file, consumer) => file.ProcessDescendants(this, consumer), committer);
    }
  }
}
