using System.Runtime.Serialization;

namespace JetBrains.ReSharper.PowerToys.Gist.GitHub
{
  [DataContract]
  public class User
  {
    [DataMember(Name = "login")]
    public string Login { get; set; }
    [DataMember(Name = "id")]
    public long Id { get; set; }
    [DataMember(Name = "gravatar_url")]
    public string GravatarUrl { get; set; }
    [DataMember(Name = "url")]
    public string Url { get; set; }
  }
}