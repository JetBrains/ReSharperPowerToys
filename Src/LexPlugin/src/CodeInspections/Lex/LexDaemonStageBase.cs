using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.LexPlugin.Grammar;
using JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Files;


namespace JetBrains.ReSharper.LexPlugin.CodeInspections.Lex
{
  public abstract class LexDaemonStageBase : IDaemonStage
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
    public static ILexFile GetPsiFile(IPsiSourceFile sourceFile)
    {
      var psiServices = sourceFile.GetPsiServices();
      psiServices.Files.AssertAllDocumentAreCommitted();
      return psiServices.Files.GetDominantPsiFile<LexLanguage>(sourceFile) as ILexFile;
    }

    protected bool IsSupported(IPsiSourceFile sourceFile)
    {
      if (sourceFile == null || !sourceFile.IsValid())
      {
        return false;
      }

      ILexFile lexFile = GetPsiFile(sourceFile);
      return lexFile != null && lexFile.Language.Is<LexLanguage>();
    }
  }
}
