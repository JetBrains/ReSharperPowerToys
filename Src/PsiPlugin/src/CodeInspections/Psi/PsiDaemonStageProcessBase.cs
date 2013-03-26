using System;
using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi
{
  public abstract class PsiDaemonStageProcessBase : TreeNodeVisitor<IHighlightingConsumer>, IRecursiveElementProcessor<IHighlightingConsumer>, IDaemonStageProcess
  {
    private readonly IDaemonProcess myDaemonProcess;
    private readonly IPsiFile myFile;

    private readonly IContextBoundSettingsStore mySettingsStore;

    protected PsiDaemonStageProcessBase(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
    {
      myDaemonProcess = process;
      mySettingsStore = settingsStore;
      myFile = PsiDaemonStageBase.GetPsiFile(myDaemonProcess.SourceFile);
    }

    protected IPsiFile File
    {
      get { return myFile; }
    }

    #region IDaemonStageProcess Members

    public IDaemonProcess DaemonProcess
    {
      get { return myDaemonProcess; }
    }

    public abstract void Execute(Action<DaemonStageResult> commiter);

    #endregion

    #region IRecursiveElementProcessor<IHighlightingConsumer> Members

    public virtual bool InteriorShouldBeProcessed(ITreeNode element, IHighlightingConsumer context)
    {
      return true;
    }

    public bool IsProcessingFinished(IHighlightingConsumer context)
    {
      if (myDaemonProcess.InterruptFlag)
      {
        throw new ProcessCancelledException();
      }
      return false;
    }

    public virtual void ProcessBeforeInterior(ITreeNode element, IHighlightingConsumer consumer)
    {
    }

    public virtual void ProcessAfterInterior(ITreeNode element, IHighlightingConsumer consumer)
    {
      var psiElement = element as IPsiTreeNode;
      if (psiElement != null)
      {
        var tokenNode = psiElement as ITokenNode;
        if (tokenNode == null || !tokenNode.GetTokenType().IsWhitespace)
        {
          psiElement.Accept(this, consumer);
        }
      }
      else
      {
        VisitNode(element, consumer);
      }
    }

    #endregion

    protected void HighlightInFile(Action<IPsiFile, IHighlightingConsumer> fileHighlighter, Action<DaemonStageResult> commiter)
    {
      var consumer = new DefaultHighlightingConsumer(this, mySettingsStore);
      fileHighlighter(File, consumer);
      commiter(new DaemonStageResult(consumer.Highlightings));
    }
  }
}
