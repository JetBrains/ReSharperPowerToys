using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.Settings;
using JetBrains.Application.src.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Dependencies;
using JetBrains.ReSharper.Psi.GeneratedCode;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.DaemonStage
{
  [FileStructureExplorer]
  public class PsiFileStructureExplorer : IFileStructureExplorer
  {
    public IFileStructure Run(IDaemonProcess process, IPsiSourceFile psiSourceFile, IContextBoundSettingsStore settingsStore)
    {
      var file = (IPsiFile)psiSourceFile.GetPsiFile<PsiLanguage>();
      if (file == null)
        return null;

      return new PsiFileStructure(process, file, settingsStore);
    }
  }

  public class PsiFileStructure : FileStructureWithRegionsBase
  {
    private readonly IDaemonProcess myDaemonProcess;
    private readonly IPsiFile myFile;

    private readonly JetHashSet<IPsiTypeMemberDeclaration> myTypeMembersToRehighlight = new JetHashSet<IPsiTypeMemberDeclaration>();

    public PsiFileStructure(IDaemonProcess daemonProcess, IPsiFile file, IContextBoundSettingsStore settingsStore)
      : base(file, settingsStore.EnumEntryIndices(GeneratedCodeSettingsAccessor.GeneratedCodeRegions).ToHashSet())
    {
      myDaemonProcess = daemonProcess;
      myFile = file;
      ProcessFile();
    }

    public ICollection<IPsiTypeMemberDeclaration> MembersToRehighlight
    {
      get { return myTypeMembersToRehighlight; }
    }

    private void ProcessFile()
    {
      new RecursiveElementProcessor(node =>
      {
        if (node is IPsiCommentNode)
        {
          ProcessComment(node.GetTreeStartOffset(), ((IPsiCommentNode)node).CommentText);
        }

      }).Process(myFile);

      CloseAllRanges(myFile);
    }
  }
}
