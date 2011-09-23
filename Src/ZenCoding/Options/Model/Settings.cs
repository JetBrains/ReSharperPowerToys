using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using JetBrains.Application;
using JetBrains.Application.Components;
using JetBrains.Application.Configuration;
using JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model.Handlers;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model
{
  [ShellComponent(ProgramConfigurations.VS_ADDIN)]
  public class Settings : IXmlExternalizable
  {
    public static readonly Settings Default =
      new Settings
      {
        FileAssociations = new List<FileAssociation>
        {
          new FileAssociation
          {
            Pattern = ".spark",
            PatternType = PatternType.FileExtension,
            DocType = DocType.Html,
            Enabled = true
          },
          new FileAssociation
          {
            Pattern = ".less",
            PatternType = PatternType.FileExtension,
            DocType = DocType.Css,
            Enabled = true
          },
          new FileAssociation
          {
            Pattern = @".*\.xslt?$",
            PatternType = PatternType.Regex,
            DocType = DocType.Xsl,
            Enabled = true
          }
        }
      };

    private readonly IPatternHandler[] myHandlers;

    public Settings()
    {
      myHandlers = new IPatternHandler[] {new FileExtensionPatternHandler(), new RegexPatternHandler()};
      FileAssociations = new List<FileAssociation>();
    }

    public static Settings Instance
    {
      get { return Shell.Instance.GetComponent<Settings>(); }
    }

    public List<FileAssociation> FileAssociations { get; set; }

    #region IXmlExternalizableShellComponent Members

    void IXmlReadable.ReadFromXml(XmlElement element)
    {
      if (element == null)
      {
        FileAssociations = Default.FileAssociations;
        return;
      }

      try
      {
        var serializer = new XmlSerializer(GetType());
        using (XmlReader reader = XmlReader.Create(new StringReader(element.InnerXml)))
        {
          var settings = (Settings) serializer.Deserialize(reader);
          FileAssociations = settings.FileAssociations;
        }
      }
      catch (Exception ex)
      {
        Logger.LogException("Failed to load ZenCoding settings", ex);
      }
    }

    void IXmlWritable.WriteToXml(XmlElement element)
    {
      element.SetAttribute("version", "1");
      var serializer = new XmlSerializer(GetType());

      using (var sw = new StringWriter())
      {
        using (XmlWriter writer = XmlWriter.Create(sw))
        {
          serializer.Serialize(writer, this);
        }

        var document = new XmlDocument();
        document.LoadXml(sw.GetStringBuilder().ToString());

        element.InnerXml = document.DocumentElement.OuterXml;
      }
    }

    #endregion

    public bool IsSupportedFile(string fileName)
    {
      return FileAssociations.Any(a => HandlerMatch(a, fileName));
    }

    private bool HandlerMatch(FileAssociation a, string fileName)
    {
      return myHandlers.FirstOrDefault(x => x.Matches(a, fileName)) != null;
    }

    public DocType GetDocType(string fileName)
    {
      var fileAssociation = FileAssociations.FirstOrDefault(a => HandlerMatch(a, fileName));
      if (fileAssociation != null)
        return fileAssociation.DocType;

      throw new NotSupportedException();
    }
  }
}