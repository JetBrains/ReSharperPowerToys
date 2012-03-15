using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.GeneratedCode;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections
{
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