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
          serializer.Serialize(writer, this);

        var document = new XmlDocument();
        document.LoadXml(sw.GetStringBuilder().ToString());

        var documentElement = document.DocumentElement;
        Assertion.AssertNotNull(documentElement, "documentElement != null");

        element.InnerXml = documentElement.OuterXml;
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