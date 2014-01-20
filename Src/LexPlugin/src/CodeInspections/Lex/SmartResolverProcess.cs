using System;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace JetBrains.ReSharper.LexPlugin.CodeInspections.Lex
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

    public void Execute(Action<DaemonStageResult> committer)
    {
    }
  }
}
