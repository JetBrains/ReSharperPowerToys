using JetBrains.Application.Settings;
using JetBrains.ReSharper.Features.Finding;

namespace JetBrains.ReSharper.PowerToys.FindText
{
  [SettingsKey(typeof(SearchSettings), "Find Text Feature State")]
  public class FindTextSettings
  {
    [SettingsEntry(0, "Last Used Flags")]
    public FindTextSearchFlags LastUsedFlags;

    [SettingsEntry(null, "Last Used Text")]
    public string LastUsedText;
  }
}