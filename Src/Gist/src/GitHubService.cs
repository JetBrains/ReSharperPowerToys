/*
 * Copyright 2007-2011 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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