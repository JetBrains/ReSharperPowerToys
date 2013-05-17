using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.GeneratedCode;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi
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
        var commentNode = node as IPsiCommentNode;
        if (commentNode != null)
          ProcessComment(commentNode, commentNode.CommentText);

      }).Process(myFile);

      CloseAllRanges(myFile);
    }
  }
}
