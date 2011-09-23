using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JetBrains.ReSharper.PowerToys.Gist.GitHub
{
  [DataContract]
  public class Gist
  {
    [DataMember(Name = "html_url")]
    public string HtmlUrl { get; set; }
    [DataMember(Name = "url")]
    public string Url { get; set; }
    [DataMember(Name = "id")]
    public long Id { get; set; }
    [DataMember(Name = "user")]
    public User User { get; set; }
    [DataMember(Name = "description")]
    public string Description { get; set; }
    [DataMember(Name = "public")]
    public bool IsPublic { get; set; }
    [DataMember(Name = "files")]
    public Dictionary<string, GistFile> Files { get; set; }
    [DataMember(Name = "comments")]
    public int Comments { get; set; }
    [DataMember(Name = "git_pull_url")]
    public string GitPullUrl { get; set; }
    [DataMember(Name = "git_push_url")]
    public string GitPushUrl { get; set; }
    [DataMember(Name = "created_at")]
    public DateTime CreatedAt { get; set; }
  }
}