using System;
using JetBrains.Application;
using JetBrains.Application.Configuration;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
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
    private static GlobalSettingsTable GlobalSettingsTable;

    public ComplexityAnalysisDaemonStage(Daemon.Daemon daemon, Lifetime lifetime, GlobalSettingsTable table)
    {
      GlobalSettingsTable = table;
      // Listen to the changes of the threshold, recalculate on such an event
      ThresholdProperty.Change.Advise(lifetime, args => daemon.Invalidate());
    }

    /// <summary>
    /// This method provides a <see cref="IDaemonStageProcess"/> instance which is assigned to highlighting a single document
    /// </summary>
    public IDaemonStageProcess CreateProcess(IDaemonProcess process, DaemonProcessKind kind)
    {
      if (process == null)
        throw new ArgumentNullException("process");

      return new ComplexityAnalysisDaemonStageProcess(process);
    }

    public ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile)
    {
      // We want to add markers to the right-side stripe as well as contribute to document errors
      return ErrorStripeRequest.STRIPE_AND_ERRORS;
    }

    /// <summary>
    /// A simple accessor for <see cref="ThresholdProperty"/>.
    /// </summary>
    public static int Threshold
    {
      get
      {
        return ThresholdProperty.Value;
      }
    }

    /// <summary>
    /// Gets a property object for the threshold setting.
    /// You may sink the value change notifications.
    /// </summary>
    public static IProperty<int> ThresholdProperty
    {
      get
      {
        // Get the property from the R# property bag by its name. The value will be persisted between R# runs.
        // As it's the only place where we use the name, there's no use of declaring a constant for it.
        // This is also the only place where we specify the default value for the property.
        return GlobalSettingsTable.IntProperties["CyclomaticComplexityThreshold", 20];
      }
    }
  }
}