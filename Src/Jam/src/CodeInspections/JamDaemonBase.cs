using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Jam.CodeInspections
{
  public abstract class JamDaemonStageBase : IDaemonStage
  {
    public virtual ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
    {
      if (!IsSupported(sourceFile))
        return ErrorStripeRequest.NONE;

      return ErrorStripeRequest.STRIPE_AND_ERRORS;
    }

    public IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
    {
      if (!IsSupported(process.SourceFile))
        yield break;

      var manager = PsiManager.GetInstance(process.SourceFile.GetSolution());
      manager.AssertAllDocumentAreCommited();

      var stageProcess = CreateProcess(process, settings, processKind, GetJamPsiFile(process));
      if (stageProcess != null) yield return stageProcess;
    }

    protected abstract IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind, IJamFile file);

    protected static bool IsSupported([CanBeNull] IPsiSourceFile sourceFile)
    {
      if (sourceFile != null && sourceFile.IsValid())
        return sourceFile.PrimaryPsiLanguage.IsExactly<JamLanguage>();

      return false;
    }

    [NotNull]
    private static IJamFile GetJamPsiFile([NotNull] IDaemonProcess process)
    {
      var jamFile = (IJamFile) process.SourceFile.GetNonInjectedPsiFile<JamLanguage>();
      Assertion.Assert(jamFile != null, "jamFile != null");
      return jamFile;
    }
  }
}
