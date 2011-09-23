using JetBrains.Application.src.Settings;
using JetBrains.ReSharper.Psi.CodeStyle;

namespace JetBrains.ReSharper.PowerToys.CyclomaticComplexity
{
  [SettingsKey(typeof(CodeInspectionSettings), "Complexity Analysis")]
  public class ComplexityAnalysisSettings
  {
    [SettingsEntry(20, "Threshold")]
    public readonly int Threshold;
  }
}