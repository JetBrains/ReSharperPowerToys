using System;
using JetBrains.Application.src.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PowerToys.CyclomaticComplexity
{
  /// <summary>
  /// Daemon stage for comlexity analysis. This class is automatically loaded by ReSharper daemon 
  /// because it's marked with the attribute.
  /// </summary>
  [DaemonStage]
  public class ComplexityAnalysisDaemonStage : IDaemonStage
  {
    public ComplexityAnalysisDaemonStage(Lifetime lifetime, Daemon.Daemon daemon, SettingsStore settingsStore)
    {
      var thresholdEntry = settingsStore.Schema.GetScalarEntry((ComplexityAnalysisSettings s) => s.Threshold);
      settingsStore.AdviseChange(lifetime, thresholdEntry, daemon.Invalidate);
    }

    /// <summary>
    /// This method provides a <see cref="IDaemonStageProcess"/> instance which is assigned to highlighting a single document
    /// </summary>
    public IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore2 settings, DaemonProcessKind kind)
    {
      if (process == null)
        throw new ArgumentNullException("process");

      return new ComplexityAnalysisDaemonStageProcess(process, settings.GetValue((ComplexityAnalysisSettings s) => s.Threshold));
    }

    public ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore2 settings)
    {
      // We want to add markers to the right-side stripe as well as contribute to document errors
      return ErrorStripeRequest.STRIPE_AND_ERRORS;
    }
  }
}