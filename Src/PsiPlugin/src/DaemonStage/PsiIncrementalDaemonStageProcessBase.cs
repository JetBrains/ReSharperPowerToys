using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.DaemonStage
{
  /// <summary>
  /// Base class for daemon stages which can incrementally re-highlight changed function only
  /// </summary>
  public abstract class PsiIncrementalDaemonStageProcessBase : PsiDaemonStageProcessBase
  {
    private readonly IContextBoundSettingsStore mySettingsStore;
    private readonly OneToListMap<IPsiTypeMemberDeclaration, TextRange> myMemberRanges = new OneToListMap<IPsiTypeMemberDeclaration, TextRange>();

    protected PsiIncrementalDaemonStageProcessBase(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
      : base(process,settingsStore)
    {
      mySettingsStore = settingsStore;
    }

    private void ExploreDocumentRanges(PsiFileStructure structure)
    {
      foreach (var declaration in structure.MembersToRehighlight)
      {
        ITreeNode rangeElement = declaration;

        myMemberRanges.AddValueRange(declaration, File.GetIntersectingRanges(rangeElement.GetTreeTextRange()).Where(r => r.Document == Document).Select(r => r.TextRange));
      }
    }

    public override void Execute(Action<DaemonStageResult> commiter)
    {
      var structure = FileStructure;
      ExploreDocumentRanges(structure);

      var visibleMembers = new HashSet<IPsiTypeMemberDeclaration>(
        structure.MembersToRehighlight.Where(
          f => File.GetIntersectingRanges(f.GetTreeTextRange()).
                 Any(r => r.Document == Document && r.TextRange.Intersects(DaemonProcess.VisibleRange))));

      Action<IPsiTypeMemberDeclaration> memberHighlighter = declaration =>
                                                              {
                                                                if (myMemberRanges[declaration].IsEmpty())
                                                                  return;

                                                                var consumer = new DefaultHighlightingConsumer(this, mySettingsStore);
                                                                declaration.ProcessThisAndDescendants(new LocalProcessor(this, consumer));
                                                                if (myMemberRanges[declaration].Count == 1)
                                                                  commiter(new DaemonStageResult(consumer.Highlightings, myMemberRanges[declaration].First()));
                                                                else
                                                                  myMemberRanges[declaration].ForEach(
                                                                    range => commiter(new DaemonStageResult(consumer.Highlightings.Where(highlighting => highlighting.Range.TextRange.StrictIntersects(range)).ToList(), range)));
                                                              };

      Action globalHighlighter = () =>
                                   {
                                     var consumer = new DefaultHighlightingConsumer(this, mySettingsStore);
                                     File.ProcessThisAndDescendants(new GlobalProcessor(this, consumer));
                                     commiter(new DaemonStageResult(consumer.Highlightings) { Layer = 1 });
                                   };

      using (var fibers = DaemonProcess.CreateFibers())
      {
        // highlight visible functions
        visibleMembers.ForEach(decl => fibers.EnqueueJob(() => memberHighlighter(decl)));

        // highlgiht global space
        if (DaemonProcess.FullRehighlightingRequired)
          fibers.EnqueueJob(globalHighlighter);

        // highlight invisible functions
        structure.MembersToRehighlight
          .Where(decl => !visibleMembers.Contains(decl))
          .ForEach(decl => fibers.EnqueueJob(() => memberHighlighter(decl)));
      }

      // remove all old highlightings
      if (DaemonProcess.FullRehighlightingRequired)
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

    private class LocalProcessor : ProcessorBase
    {
      public LocalProcessor(PsiDaemonStageProcessBase process, IHighlightingConsumer consumer)
        : base(process, consumer)
      {
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