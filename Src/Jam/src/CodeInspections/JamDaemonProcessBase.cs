using System;
using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.CodeInspections
{
  public abstract class JamDaemonProcessBase : TreeNodeVisitor<IHighlightingConsumer>, IRecursiveElementProcessor<IHighlightingConsumer>, IDaemonStageProcess
  {
    private readonly IDaemonProcess myDaemonProcess;
    private readonly IContextBoundSettingsStore mySettingsStore;
    private readonly IDocument myDocument;
    private readonly IJamFile myFile;

    protected JamDaemonProcessBase(IDaemonProcess process, IContextBoundSettingsStore settingsStore, IJamFile file)
    {
      myDaemonProcess = process;
      mySettingsStore = settingsStore;
      myFile = file;
      myDocument = process.Document;
    }

    public IDaemonProcess DaemonProcess
    {
      get { return myDaemonProcess; }
    }

    public IPsiSourceFile SourceFile
    {
      get { return myDaemonProcess.SourceFile; }
    }

    public IDocument Document
    {
      get { return myDocument; }
    }

    public IPsiModule PsiModule
    {
      get { return DaemonProcess.SourceFile.PsiModule; }
    }

    public IJamFile File
    {
      get { return myFile; }
    }

    public bool IsProcessingFinished(IHighlightingConsumer context)
    {
      if (myDaemonProcess.InterruptFlag)
        throw new ProcessCancelledException();
      return false;
    }

    public virtual void Execute(Action<DaemonStageResult> commiter)
    {
      HighlightInFile((file, consumer) => file.ProcessDescendants(this, consumer), commiter);
    }

    public virtual bool InteriorShouldBeProcessed(ITreeNode element, IHighlightingConsumer consumer)
    {
      return !IsProcessingFinished(consumer);
    }

    public virtual void ProcessBeforeInterior(ITreeNode element, IHighlightingConsumer consumer)
    {
    }

    public virtual void ProcessAfterInterior(ITreeNode element, IHighlightingConsumer consumer)
    {
      var node = element as IJamTreeNode;

      if (node != null && !node.IsWhitespaceToken())
        node.Accept(this, consumer);
      else
        VisitNode(element, consumer);
    }

    protected void HighlightInFile(Action<IJamFile, IHighlightingConsumer> fileHighlighter, Action<DaemonStageResult> commiter)
    {
      var consumer = new DefaultHighlightingConsumer(this, mySettingsStore);
      fileHighlighter(File, consumer);
      commiter(new DaemonStageResult(consumer.Highlightings));
    }
  }
}