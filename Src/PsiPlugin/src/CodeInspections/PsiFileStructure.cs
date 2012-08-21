using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.GeneratedCode;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections
{
  public class PsiFileStructure : FileStructureWithRegionsBase
  {
    private readonly IPsiFile myFile;

    public PsiFileStructure([NotNull] IPsiFile file, IContextBoundSettingsStore settingsStore)
      : base(file, settingsStore.EnumEntryIndices(GeneratedCodeSettingsAccessor.GeneratedCodeRegions).ToHashSet())
    {
      myFile = file;
      ProcessFile();
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
