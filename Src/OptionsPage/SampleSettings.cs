using JetBrains.Application.src.Settings;

namespace JetBrains.ReSharper.PowerToys.OptionsPage
{
  [SettingsKey(typeof(EnvironmentSettings), "My sample settings")]
  public class SampleSettings
  {
    [SettingsEntry(false, "Whether to do or not")]
    public bool SampleOption { get; set; }
  }
}