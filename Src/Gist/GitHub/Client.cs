using RestSharp;

namespace JetBrains.ReSharper.PowerToys.Gist.GitHub
{
  public class Client : RestClient
  {
    public Client() : base("https://api.github.com") { }
  }
}