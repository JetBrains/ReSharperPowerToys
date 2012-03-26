using System;
using JetBrains.ReSharper.Daemon;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections
{
  public class SmartResolverProcess : IDaemonStageProcess
  {
    private readonly IDaemonProcess myDaemonProcess;

    public SmartResolverProcess(IDaemonProcess daemonProcess)
    {
      myDaemonProcess = daemonProcess;
    }

    public IDaemonProcess DaemonProcess
    {
      get { return myDaemonProcess; }
    }

    public void Execute(Action<DaemonStageResult> commiter)
    {
    }
  }
}