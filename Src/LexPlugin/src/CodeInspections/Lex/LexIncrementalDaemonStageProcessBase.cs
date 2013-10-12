using System;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.LexPlugin.CodeInspections.Lex
{
  public class LexIncrementalDaemonStageProcessBase : LexDaemonStageProcessBase
  {
    private readonly IContextBoundSettingsStore mySettingsStore;

    protected LexIncrementalDaemonStageProcessBase(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
      : base(process, settingsStore)
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

    #region Nested type: GlobalProcessor

    private class GlobalProcessor : ProcessorBase
    {
      public GlobalProcessor(LexDaemonStageProcessBase process, IHighlightingConsumer consumer)
        : base(process, consumer)
      {
      }
    }

    #endregion

    #region Nested type: ProcessorBase

    private class ProcessorBase : IRecursiveElementProcessor
    {
      private readonly IHighlightingConsumer myConsumer;
      private readonly LexDaemonStageProcessBase myProcess;

      protected ProcessorBase(LexDaemonStageProcessBase process, IHighlightingConsumer consumer)
      {
        myProcess = process;
        myConsumer = consumer;
      }

      #region IRecursiveElementProcessor Members

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

      #endregion
    }

    #endregion
  }
}