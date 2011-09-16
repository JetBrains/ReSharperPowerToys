using System.Windows.Controls.Primitives;
using JetBrains.Application.src.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Features.Common.Options;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;

namespace JetBrains.ReSharper.PowerToys.OptionsPage
{
  [OptionsPage(PID, "Sample Page", "JetBrains.ReSharper.PowerToys.OptionsPage.samplePage.gif", ParentId = ToolsPage.PID)]
  public partial class SampleOptionPage : IOptionsPage
  {
    private const string PID = "SamplePageId";

    public SampleOptionPage(Lifetime lifetime, OptionsSettingsSmartContext settings)
    {
      InitializeComponent();
      settings.SetBinding(lifetime, (SampleSettings s) => s.SampleOption, mySampleOptionCheckBox, ToggleButton.IsCheckedProperty);
    }

    public EitherControl Control
    {
      get { return this; }
    }

    public string Id
    {
      get { return PID; }
    }

    public bool OnOk()
    {
      return true;
    }

    public bool ValidatePage()
    {
      return true;
    }
  }
}
