using JetBrains.Application.src.Settings;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;

namespace JetBrains.ReSharper.PowerToys.CyclomaticComplexity
{
  [SolutionComponent]
  public class ComplexityAnalysisInvalidateOnThresholdChange
  {
    public ComplexityAnalysisInvalidateOnThresholdChange(Lifetime lifetime, Daemon.Daemon daemon,
                                                         SettingsStore settingsStore)
    {
      SettingsScalarEntry thresholdEntry = settingsStore.Schema.GetScalarEntry((ComplexityAnalysisSettings s) => s.Threshold);
      settingsStore.AdviseChange(lifetime, thresholdEntry, daemon.Invalidate);
    }
  }
}