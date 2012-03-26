using System;
using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections
{
  public abstract class PsiDaemonStageProcessBase : TreeNodeVisitor<IHighlightingConsumer>, IRecursiveElementProcessor<IHighlightingConsumer>, IDaemonStageProcess
  {
    private readonly IDaemonProcess myDaemonProcess;
    private readonly IPsiFile myFile;

    private readonly IContextBoundSettingsStore mySettingsStore;

    private IPsiServices PsiServices { get; set; }

    public IDaemonProcess DaemonProcess
    {
      get { return myDaemonProcess; }
    }

    protected PsiDaemonStageProcessBase(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
    {
      myDaemonProcess = process;
      mySettingsStore = settingsStore;
      PsiServices = process.Solution.GetPsiServices();
      myFile = PsiDaemonStageBase.GetPsiFile(myDaemonProcess.SourceFile);
    }


    protected void HighlightInFile(Action<IPsiFile, IHighlightingConsumer> fileHighlighter, Action<DaemonStageResult> commiter)
    {
      var consumer = new DefaultHighlightingConsumer(this, mySettingsStore);
      fileHighlighter(File, consumer);
      commiter(new DaemonStageResult(consumer.Highlightings));
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
      var psiElement = element as IPsiTreeNode;
      if (psiElement != null)
      {
        var tokenNode = psiElement as ITokenNode;
        if (tokenNode == null || !tokenNode.GetTokenType().IsWhitespace)
          psiElement.Accept(this, consumer);
      }
      else
      {
        //VisitNode(element, consumer);
      }
    }

    #endregion
  }
}