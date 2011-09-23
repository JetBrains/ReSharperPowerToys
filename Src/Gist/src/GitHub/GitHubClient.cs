using RestSharp;

namespace JetBrains.ReSharper.PowerToys.Gist.GitHub
{
  public class GitHubClient : RestClient
  {
    public GitHubClient() : base("https://api.github.com") { }
  }
}