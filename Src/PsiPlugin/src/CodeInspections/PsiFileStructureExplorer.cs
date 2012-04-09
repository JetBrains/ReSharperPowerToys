using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections
{
  [FileStructureExplorer]
  public class PsiFileStructureExplorer : IFileStructureExplorer
  {
    public IFileStructure Run(IDaemonProcess process, IPsiSourceFile psiSourceFile, IContextBoundSettingsStore settingsStore)
    {
      var file = (IPsiFile)psiSourceFile.GetPsiFile<PsiLanguage>(new DocumentRange(psiSourceFile.Document, 0));
      if (file == null)
        return null;

      return new PsiFileStructure(file, settingsStore);
    }

    public IFileStructure Run(IDaemonProcess process, IPsiSourceFile psiSourceFile, IContextBoundSettingsStore settingsStore, IFile file)
    {
      var psiFile = file as IPsiFile;
      return new PsiFileStructure(psiFile, settingsStore);
    }
  }
}
