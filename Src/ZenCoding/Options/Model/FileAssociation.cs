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
using System.Globalization;
using System.Windows.Markup;
using JetBrains.Util;
using JetBrains.Util.Reflection;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model
{
  [ValueSerializer(typeof(FileAssociationSerializer))]
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

    public bool Equals(FileAssociation other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Equals(other.Pattern, Pattern) && Equals(other.DocType, DocType) && Equals(other.PatternType, PatternType) && other.Enabled.Equals(Enabled);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != typeof (FileAssociation)) return false;
      return Equals((FileAssociation) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        int result = (Pattern != null ? Pattern.GetHashCode() : 0);
        result = (result*397) ^ DocType.GetHashCode();
        result = (result*397) ^ PatternType.GetHashCode();
        result = (result*397) ^ Enabled.GetHashCode();
        return result;
      }
    }
  }

  public class FileAssociationSerializer : ValueSerializerBase<FileAssociation>
  {
    private const string PATTERN_TYPE = "PatternType";
    private const string PATTERN = "Pattern";
    private const string DOCTYPE = "DocType";
    private const string ENABLED = "Enabled";

    public FileAssociationSerializer() : base(FileAssociationToString, StringToFileAssociation) { }

    private static FileAssociation StringToFileAssociation(string s, ILogger logger)
    {
      return TypeConverterUtil.FromStringThruXml(s, element =>
      {
        var pattern = element.ReadAttribute(PATTERN);
        var docType = element.ReadAttribute(DOCTYPE).ToEnum<DocType>(DocType.None);
        var patternType = element.ReadAttribute(PATTERN_TYPE).ToEnum<PatternType>(PatternType.FileExtension);
        bool enabled = false;
        bool.TryParse(element.ReadAttribute(ENABLED), out enabled);

        return new FileAssociation()
        {
          DocType = docType,
          Enabled = enabled,
          Pattern = pattern,
          PatternType = patternType
        };
      });
    }

    private static string FileAssociationToString(FileAssociation fileAssociation, ILogger logger)
    {
      return TypeConverterUtil.ToStringThruXml(element =>
      {
        element.CreateAttributeWithNonEmptyValue(PATTERN, fileAssociation.Pattern);
        element.CreateAttributeWithNonEmptyValue(DOCTYPE, fileAssociation.DocType.ToString());
        element.CreateAttributeWithNonEmptyValue(PATTERN_TYPE, fileAssociation.PatternType.ToString());
        element.CreateAttributeWithNonEmptyValue(ENABLED, fileAssociation.Enabled.ToString(CultureInfo.InvariantCulture));
      });
    }
  }
}