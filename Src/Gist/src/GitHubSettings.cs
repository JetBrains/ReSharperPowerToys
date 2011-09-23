using JetBrains.Application.Communication;
using JetBrains.Application.src.Settings;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.Gist
{
  /// <summary>
  /// Gist settings.
  /// </summary>
  [SettingsKey(typeof (InternetSettings), "GitHub settings")]
  public class GitHubSettings
  {
    [SettingsEntry("", "GitHub username")]
    public string Username { get; set; }

    [SettingsEntry("", "GitHub password")]
    public string Password { get; set; }

    public bool IsAnonymous
    {
      get { return Username.IsEmpty() || Password.IsEmpty(); }
    }
  }
}
