using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections
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
}
