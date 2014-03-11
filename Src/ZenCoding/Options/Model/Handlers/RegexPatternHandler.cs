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

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model.Handlers
{
  internal class RegexPatternHandler : IPatternHandler
  {
    private readonly IDictionary<string, Regex> myCache = new Dictionary<string, Regex>();

    public bool Matches(FileAssociation fileAssociation, string fileName)
    {
      if (!fileAssociation.Enabled)
        return false;

      if (fileAssociation.PatternType != PatternType.Regex)
        return false;

      var expr = GetRegex(fileAssociation);

      return expr.IsMatch(fileName);
    }

    private Regex GetRegex(FileAssociation fileAssociation)
    {
      if (myCache.ContainsKey(fileAssociation.Pattern))
        return myCache[fileAssociation.Pattern];

      return new Regex(fileAssociation.Pattern,
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
    }
  }
}