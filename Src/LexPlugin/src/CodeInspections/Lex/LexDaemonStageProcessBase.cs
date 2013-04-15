using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.LexPlugin.CodeInspections.Lex
{
  public abstract class LexDaemonStageProcessBase : TreeNodeVisitor<IHighlightingConsumer>, IRecursiveElementProcessor<IHighlightingConsumer>, IDaemonStageProcess
  {
    private readonly IDaemonProcess myDaemonProcess;
    private readonly ILexFile myFile;

    private readonly IContextBoundSettingsStore mySettingsStore;

    protected LexDaemonStageProcessBase(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
    {
      myDaemonProcess = process;
      mySettingsStore = settingsStore;
      PsiServices = process.Solution.GetPsiServices();
      myFile = LexDaemonStageBase.GetPsiFile(myDaemonProcess.SourceFile);
    }

    private IPsiServices PsiServices { get; set; }


    public ILexFile File
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
      var psiElement = element as ILexTreeNode;
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

    protected void HighlightInFile(Action<ILexFile, IHighlightingConsumer> fileHighlighter, Action<DaemonStageResult> commiter)
    {
      var consumer = new DefaultHighlightingConsumer(this, mySettingsStore);
      fileHighlighter(File, consumer);
      commiter(new DaemonStageResult(consumer.Highlightings));
    }
  }
}
