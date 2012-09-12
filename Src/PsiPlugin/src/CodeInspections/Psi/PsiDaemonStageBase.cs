using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi
{
  public abstract class PsiDaemonStageBase : IDaemonStage
  {
    #region IDaemonStage Members

    public abstract IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind);

    public virtual ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
    {
      if (!IsSupported(sourceFile))
      {
        return ErrorStripeRequest.NONE;
      }

      var properties = sourceFile.Properties;
      if (!properties.ProvidesCodeModel || properties.IsNonUserFile)
      {
        return ErrorStripeRequest.NONE;
      }

      return ErrorStripeRequest.STRIPE_AND_ERRORS;
    }

    #endregion

    [CanBeNull]
    public static IPsiFile GetPsiFile(IPsiSourceFile sourceFile)
    {
      PsiManager manager = PsiManager.GetInstance(sourceFile.GetSolution());
      manager.AssertAllDocumentAreCommited();
      return manager.GetPsiFile<PsiLanguage>(new DocumentRange(sourceFile.Document, 0)) as IPsiFile;
    }

    protected bool IsSupported(IPsiSourceFile sourceFile)
    {
      if (sourceFile == null || !sourceFile.IsValid())
      {
        return false;
      }

      IPsiFile psiFile = GetPsiFile(sourceFile);
      return psiFile != null && psiFile.Language.Is<PsiLanguage>();
    }
  }
}
