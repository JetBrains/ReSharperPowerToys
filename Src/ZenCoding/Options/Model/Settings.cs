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
using System.Xml;
using System.Xml.Serialization;
using JetBrains.Application.Configuration;
using JetBrains.Util;
using JetBrains.Util.Logging;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model
{
  public class Settings : IXmlReadable
  {
    public IList<FileAssociation> FileAssociations { get; private set; }

    void IXmlReadable.ReadFromXml(XmlElement element)
    {
      if (element == null)
      {
        FileAssociations = EmptyList<FileAssociation>.InstanceList;
        return;
      }

      try
      {
        var serializer = new XmlSerializer(GetType());
        using (var reader = XmlReader.Create(new StringReader(element.InnerXml)))
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
  }
}