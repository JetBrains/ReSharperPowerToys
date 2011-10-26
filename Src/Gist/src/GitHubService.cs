using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.Communication;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.Application.src.Settings;
using JetBrains.ReSharper.PowerToys.Gist.GitHub;
using RestSharp;

namespace JetBrains.ReSharper.PowerToys.Gist
{
  [ShellComponent]
  public class GitHubService
  {
    private readonly WebProxySettingsReader myProxySettingsReader;
    private readonly SettingsStore mySettingsStore;

    public GitHubService(WebProxySettingsReader proxySettingsReader, SettingsStore settingsStore)
    {
      myProxySettingsReader = proxySettingsReader;
      mySettingsStore = settingsStore;
    }

    public GitHubClient GetClient([NotNull] IDataContext context)
    {
      var boundSettings = mySettingsStore.BindToContextTransient(ContextRange.Smart((lt, _) => context));

      var proxy = myProxySettingsReader.GetProxySettings(boundSettings);
      var settings = boundSettings.GetKey<GitHubSettings>(SettingsOptimization.DoMeSlowly);

      var client = new GitHubClient { Proxy = proxy };
      if (!settings.IsAnonymous)
        client.Authenticator = new HttpBasicAuthenticator(settings.Username, settings.Password);

      return client;
    }
  }
}