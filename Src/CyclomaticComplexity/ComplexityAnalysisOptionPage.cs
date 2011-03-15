using JetBrains.ReSharper.Features.Environment.Options.Inspections;
using JetBrains.UI.Options;
using JetBrains.UI.Options.Helpers;

namespace JetBrains.ReSharper.PowerToys.CyclomaticComplexity
{
  /// <summary>
  /// Implements an options page that holds a set of setting editors stacked in lines from top to bottom.
  /// </summary>
  [OptionsPage(PID, "Complexity Analysis", "JetBrains.ReSharper.PowerToys.CyclomaticComplexity.ComplexityOptionPage.png", ParentId = CodeInspectionPage.PID)]
  public class ComplexityAnalysisOptionPage : AStackPanelOptionsPage
  {
    private const string PID = "PowerToys.CyclomaticComplexity";

    /// <summary>
    /// Creates new instance of ComplexityAnalysisOptionPage
    /// </summary>
    public ComplexityAnalysisOptionPage()
      : base(PID)
    {
      InitControls();
    }

    private void InitControls()
    {
      Controls.Spin spin; // This variable may be reused if there's more than one spin on the page
      Controls.HorzStackPanel stack;

      // The upper cue banner, stacked in the first line of our page, docked to full width with word wrapping, as needed
      Controls.Add(new Controls.Label(Stringtable.Options_Banner));

      // Some spacing
      Controls.Add(UI.Options.Helpers.Controls.Separator.DefaultHeight);

      // A horizontal stack of a text label and a spin-edit
      Controls.Add(stack = new Controls.HorzStackPanel());
      stack.Controls.Add(new Controls.Label(Stringtable.Options_ThresholdLabel)); // The first column of the stack
      stack.Controls.Add(spin = new Controls.Spin());

      // Set up the spin we've just added
      spin.Maximum = new decimal(new[] {500, 0, 0, 0});
      spin.Minimum = new decimal(new[] {1, 0, 0, 0});
      spin.Value = new decimal(new[] {1, 0, 0, 0});

      // This binding will take the initial value from ComplexityAnalysisElementProcessor, put it into the edit, and pass back from UI to the control if the OK button is hit
      Bind(ComplexityAnalysisDaemonStage.ThresholdProperty, spin.IntegerValue);
    }
  }
}