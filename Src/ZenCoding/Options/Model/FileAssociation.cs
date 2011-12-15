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
using JetBrains.Application.Settings;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model
{
  public class FileAssociation : ICloneable
  {
    [SettingsEntry(null, "Pattern")]
    public string Pattern { get; set; }

    [SettingsEntry(DocType.None, "Doc Type")]
    public DocType DocType { get; set; }

    [SettingsEntry(null, "Pattern Type")]
    public PatternType PatternType { get; set; }

    [SettingsEntry(false, "Enabled")]
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