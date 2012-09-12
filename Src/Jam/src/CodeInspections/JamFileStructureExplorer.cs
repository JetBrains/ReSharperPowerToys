using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.CodeInspections
{
  [FileStructureExplorer]
  public class JamFileStructureExplorer : IFileStructureExplorer
  {
    public IFileStructure Run(IDaemonProcess process, IPsiSourceFile psiSourceFile, IContextBoundSettingsStore settingsStore, IFile file)
    {
      var jamFile = file as IJamFile;
      return jamFile == null ? null : new JamFileStructure(jamFile);
    }
  }
}