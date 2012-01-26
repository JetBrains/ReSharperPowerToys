using System;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.DaemonStage
{
  public abstract class PsiDaemonStageProcessBase : TreeNodeVisitor<IHighlightingConsumer>, IRecursiveElementProcessor<IHighlightingConsumer>, IDaemonStageProcess
  {
    private readonly IDaemonProcess myDaemonProcess;
    private readonly IDocument myDocument;
    private readonly IPsiFile myFile;

    protected readonly IContextBoundSettingsStore mySettingsStore;

    protected IPsiServices PsiServices { get; private set; }

    public IDaemonProcess DaemonProcess
    {
      get { return myDaemonProcess; }
    }

    [NotNull]
    public PsiFileStructure FileStructure
    {
      get
      {
        var globalStructureProcess = DaemonProcess.GetStageProcess<GlobalFileStructureCollectorStage.Process>();
        Assertion.Assert(globalStructureProcess != null, "globalStructureProcess != null");
        var PsiFileStructure = globalStructureProcess.Get<PsiFileStructure>();
        Assertion.Assert(PsiFileStructure != null, "PsiFileStructure != null");
        return PsiFileStructure;
      }
    }

    protected PsiDaemonStageProcessBase(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
    {
      myDaemonProcess = process;
      mySettingsStore = settingsStore;
      PsiServices = process.Solution.GetPsiServices();
      myFile = PsiDaemonStageBase.GetPsiFile(myDaemonProcess.SourceFile);
    }

    /*protected PsiDaemonStageProcessBase(IDaemonProcess process)
    {
      myDaemonProcess = process;
      myFile = PsiDaemonStageBase.GetPsiFile(myDaemonProcess.SourceFile);
      myDocument = process.Document;
      //myTypeConversionRule = new PsiTypeConversionRule(PsiModule);
    }*/

    protected void HighlightInFile(Action<IPsiFile, IHighlightingConsumer> fileHighlighter, Action<DaemonStageResult> commiter)
    {
      var consumer = new DefaultHighlightingConsumer(this, mySettingsStore);
      fileHighlighter(File, consumer);
      commiter(new DaemonStageResult(consumer.Highlightings));
    }

    public IDocument Document
    {
      get { return myDocument; }
    }

    public IPsiModule PsiModule
    {
      get { return DaemonProcess.PsiModule; }
    }

    public IPsiFile File
    {
      get { return myFile; }
    }

    public abstract void Execute(Action<DaemonStageResult> commiter);

    #region IRecursiveElementProcessor Members

    virtual public bool InteriorShouldBeProcessed(ITreeNode element, IHighlightingConsumer context)
    {
      return true;
    }

    public bool IsProcessingFinished(IHighlightingConsumer context)
    {
      if (myDaemonProcess.InterruptFlag)
        throw new ProcessCancelledException();
      return false;
    }

    virtual public void ProcessBeforeInterior(ITreeNode element, IHighlightingConsumer consumer)
    {
    }

    virtual public void ProcessAfterInterior(ITreeNode element, IHighlightingConsumer consumer)
    {
      var PsiElement = element as IPsiTreeNode;
      if (PsiElement != null)
      {
        var tokenNode = PsiElement as ITokenNode;
        if (tokenNode == null || !tokenNode.GetTokenType().IsWhitespace)
          PsiElement.Accept(this, consumer);
      }
      else
      {
        VisitNode(element, consumer);
      }
    }

    #endregion
  }
}