using System;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections
{
  /// <summary>
  /// Base class for daemon stages which can incrementally re-highlight changed function only
  /// </summary>
  public abstract class PsiIncrementalDaemonStageProcessBase : PsiDaemonStageProcessBase
  {
    private readonly IContextBoundSettingsStore mySettingsStore;

    protected PsiIncrementalDaemonStageProcessBase(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
      : base(process,settingsStore)
    {
      mySettingsStore = settingsStore;
    }


    public override void Execute(Action<DaemonStageResult> commiter)
    {
      Action globalHighlighter = () =>
                                   {
                                     var consumer = new DefaultHighlightingConsumer(this, mySettingsStore);
                                     File.ProcessThisAndDescendants(new GlobalProcessor(this, consumer));
                                     commiter(new DaemonStageResult(consumer.Highlightings) { Layer = 1 });
                                   };

      using (var fibers = DaemonProcess.CreateFibers())
      {

        // highlgiht global space
        //if (DaemonProcess.FullRehighlightingRequired)
          fibers.EnqueueJob(globalHighlighter);
      }

      // remove all old highlightings
      //if (DaemonProcess.FullRehighlightingRequired)
        commiter(new DaemonStageResult(EmptyArray<HighlightingInfo>.Instance));
    }

    private class ProcessorBase : IRecursiveElementProcessor
    {
      private readonly PsiDaemonStageProcessBase myProcess;
      private readonly IHighlightingConsumer myConsumer;

      protected ProcessorBase(PsiDaemonStageProcessBase process, IHighlightingConsumer consumer)
      {
        myProcess = process;
        myConsumer = consumer;
      }

      public bool ProcessingIsFinished
      {
        get { return myProcess.IsProcessingFinished(myConsumer); }
      }

      public virtual void ProcessBeforeInterior(ITreeNode element)
      {
        myProcess.ProcessBeforeInterior(element, myConsumer);
      }

      public virtual void ProcessAfterInterior(ITreeNode element)
      {
        myProcess.ProcessAfterInterior(element, myConsumer);
      }

      public virtual bool InteriorShouldBeProcessed(ITreeNode element)
      {
        return myProcess.InteriorShouldBeProcessed(element, myConsumer);
      }
    }

    private class GlobalProcessor : ProcessorBase
    {
      public GlobalProcessor(PsiDaemonStageProcessBase process, IHighlightingConsumer consumer)
        : base(process, consumer)
      {
      }
    }
  }
}