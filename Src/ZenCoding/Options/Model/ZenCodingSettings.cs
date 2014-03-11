/*
 * Copyright 2007-2014 JetBrains
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
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.Application.Settings.Store;
using JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model.Handlers;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model
{
  [SettingsKey(typeof(EnvironmentSettings), "Zen Coding")]
  public class ZenCodingSettings
  {
    private readonly IPatternHandler[] myHandlers;

    public ZenCodingSettings()
    {
      myHandlers = new IPatternHandler[] {new FileExtensionPatternHandler(), new RegexPatternHandler()};
    }

    [SettingsIndexedEntry("File Associations")]
    public IIndexedEntry<int, FileAssociation> FileAssociations { get; set; }

    [SettingsEntryAttribute(false, "Is upgraded")]
    public bool IsUpgraded { get; set; }

    public bool IsSupportedFile(string fileName)
    {      
      return FileAssociations.EnumIndexedValues().Any(pair => HandlerMatch(pair.Value, fileName));
    }

    private bool HandlerMatch(FileAssociation a, string fileName)
    {
      return myHandlers.FirstOrDefault(x => x.Matches(a, fileName)) != null;
    }

    public DocType GetDocType(string fileName)
    {
      var fileAssociationPair = FileAssociations.EnumIndexedValues().FirstOrDefault(pair => HandlerMatch(pair.Value, fileName));
      if (fileAssociationPair.Value != null)
        return fileAssociationPair.Value.DocType;

      throw new NotSupportedException();
    }
  }
}