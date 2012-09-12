using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi
{
  [FileStructureExplorer]
  public class PsiFileStructureExplorer : IFileStructureExplorer
  {
    public IFileStructure Run(IDaemonProcess process, IPsiSourceFile psiSourceFile, IContextBoundSettingsStore settingsStore, IFile file)
    {
      var psiFile = file as IPsiFile;
      if (psiFile == null)
        return null;

      return new PsiFileStructure(psiFile, settingsStore);
    }
  }
}
