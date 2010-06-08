using JetBrains.ReSharper.Daemon;

namespace JetBrains.ReSharper.PowerToys.CyclomaticComplexity
{
  /// <summary>
  /// The highlighting that warns about high complexity
  /// </summary>
  [StaticSeverityHighlighting(Severity.WARNING)]
  public class ComplexityWarning : IHighlighting
  {
    private readonly string myTooltip;

    public ComplexityWarning(string toolTip)
    {
      myTooltip = toolTip;
    }

    public string ToolTip
    {
      get { return myTooltip; }
    }

    public string ErrorStripeToolTip
    {
      get { return myTooltip; }
    }

    public virtual int NavigationOffsetPatch
    {
      get { return 0; }
    }

    public bool IsValid()
    {
      return true;
    }
  }
}