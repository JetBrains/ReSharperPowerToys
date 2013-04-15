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

using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.Search;
using JetBrains.ReSharper.Feature.Services.Search.SearchRequests;
using JetBrains.ReSharper.Features.Common.Occurences;
using JetBrains.ReSharper.Features.Finding.Search;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.FindText
{
  /// <summary>
  /// Describes text search
  /// </summary>
  public class FindTextDescriptor : SearchDescriptor
  {
    public FindTextDescriptor(SearchRequest request)
      : base(request)
    {}

    public FindTextDescriptor(SearchRequest request, ICollection<IOccurence> results)
      : base(request, results)
    {}

    /// <summary>
    /// Provides custom title for find results view tab
    /// </summary>
    /// <returns></returns>
    public override string GetResultsTitle(OccurenceSection section)
    {
      string form = "location";

      string title;
      int foundCount = section.TotalCount;
      if (foundCount > 0)
      {
        if (foundCount > 1)
          form = NounUtil.GetPlural(form);

        title = 
          foundCount == section.FilteredCount ? 
          string.Format("Found {0} {1}", foundCount, form) : 
          string.Format("Displaying {0} of {1} found {2}", section.FilteredCount, foundCount, form);
      }
      else
        title = string.Format("No locations found");
      return title;
    }
  }
}