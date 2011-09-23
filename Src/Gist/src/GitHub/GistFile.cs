using System.Runtime.Serialization;

namespace JetBrains.ReSharper.PowerToys.Gist.GitHub
{
  [DataContract]
  public class GistFile
  {
    [DataMember(Name = "size")]
    public int Size { get; set; }
    [DataMember(Name = "filename")]
    public string Filename { get; set; }
    [DataMember(Name = "raw_url")]
    public string RawUrl { get; set; }
    [DataMember(Name = "content")]
    public string Content { get; set; }
  }
}