using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections
{
  public abstract class PsiDaemonStageBase : IDaemonStage
  {
    public abstract IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind);

    public virtual ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
    {
      if (!IsSupported(sourceFile))
        return ErrorStripeRequest.NONE;

      var properties = sourceFile.Properties;
      if (!properties.ProvidesCodeModel || properties.IsNonUserFile)
        return ErrorStripeRequest.NONE;

      return ErrorStripeRequest.STRIPE_AND_ERRORS;
    }

    [CanBeNull]
    public static IPsiFile GetPsiFile(IPsiSourceFile sourceFile)
    {
      var manager = PsiManager.GetInstance(sourceFile.GetSolution());
      manager.AssertAllDocumentAreCommited();
      return manager.GetPsiFile<PsiLanguage>(sourceFile) as IPsiFile;
    }

    protected virtual bool IsSupported(IPsiSourceFile sourceFile)
    {
      if (sourceFile == null || !sourceFile.IsValid())
        return false;

      var psiFile = GetPsiFile(sourceFile);
      return psiFile != null && psiFile.Language.Is<PsiLanguage>();
    }
  }
}