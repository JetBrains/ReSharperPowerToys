using System;
using JetBrains.Application;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;

namespace JetBrains.ReSharper.PowerToys.CyclomaticComplexity
{
  /// <summary>
  /// Daemon stage for comlexity analysis. This class is automatically loaded by ReSharper daemon 
  /// because it's marked with the attribute.
  /// </summary>
  [DaemonStage]
  public class ComplexityAnalysisDaemonStage : IDaemonStage
  {
    public ComplexityAnalysisDaemonStage()
    {
      // Listen to the changes of the threshold, recalculate on such an event
      ThresholdProperty.Change.Advise(delegate
                                        {
                                          var currentSolution = SolutionManager.Instance.CurrentSolution;
                                          if (currentSolution == null)
                                            return;
                                          // VS has just started, or the solution has been closed — nothing to do

                                          Daemon.Daemon.GetInstance(currentSolution).Invalidate();
                                        });

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

    public ErrorStripeRequest NeedsErrorStripe(IProjectFile projectFile)
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
        return GlobalSettingsTable.Instance.IntProperties["CyclomaticComplexityThreshold", 20];
      }
    }
  }
}