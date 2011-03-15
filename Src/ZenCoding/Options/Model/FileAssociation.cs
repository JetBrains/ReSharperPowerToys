using System;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model
{
  public class FileAssociation : ICloneable
  {
    public string Pattern { get; set; }

    public DocType DocType { get; set; }

    public PatternType PatternType { get; set; }

    public bool Enabled { get; set; }

    public object Clone()
    {
      return new FileAssociation
      {
        Pattern = Pattern,
        PatternType = PatternType,
        DocType = DocType,
        Enabled = Enabled
      };
    }

    public void CopyFrom(FileAssociation other)
    {
      Pattern = other.Pattern;
      PatternType = other.PatternType;
      DocType = other.DocType;
      Enabled = other.Enabled;
    }
  }
}