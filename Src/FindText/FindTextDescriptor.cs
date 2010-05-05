using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.Search;
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

        if (foundCount == section.FilteredCount)
        {
          // all results are shown
          title = string.Format("Found {0} {1}", foundCount, form);
        }
        else
        {
          // some results are filtered
          title = string.Format("Displaying {0} of {1} found {2}", section.FilteredCount, foundCount, form);
        }
      }
      else
        title = string.Format("No locations found");
      return title;
    }
  }
}