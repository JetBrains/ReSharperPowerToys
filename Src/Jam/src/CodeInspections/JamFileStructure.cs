using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.Jam.Tree;

namespace JetBrains.ReSharper.Psi.Jam.CodeInspections
{
  public class JamFileStructure : FileStructureBase
  {
    private readonly IJamFile myFile;

    public JamFileStructure([NotNull] IJamFile file) : base(file)
    {
      myFile = file;
      ProcessFile();
    }

    private void ProcessFile()
    {
      new RecursiveElementProcessor<IJamCommentNode>(node => ProcessComment(node.GetTreeStartOffset(), node.CommentText)).Process(myFile);
      CloseAllRanges(myFile);
    }
  }
}