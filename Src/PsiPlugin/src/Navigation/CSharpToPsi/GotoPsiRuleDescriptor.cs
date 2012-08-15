using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.Search;
using JetBrains.ReSharper.Feature.Services.Search.SearchRequests;
using JetBrains.ReSharper.Features.Common.Occurences;
using JetBrains.ReSharper.Features.Finding.Search;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.CSharpToPsi
{
  public class GotoPsiRuleDescriptor : SearchDescriptor
  {
    public GotoPsiRuleDescriptor(SearchRequest request, ICollection<IOccurence> results) : base(request, results)
    {
    }

    public override string GetResultsTitle(OccurenceSection section)
    {
      string form = "generated";

      string title;
      int foundCount = section.TotalCount;
      if (foundCount > 0)
      {
        if (foundCount > 1)
          form = NounUtil.GetPlural(form);

        title = foundCount == section.FilteredCount
                  ? string.Format("Found {0} {1}", foundCount, form)
                  : string.Format("Displaying {0} of {1} found {2}", section.FilteredCount, foundCount, form);
      }
      else
        title = string.Format("No psi rules found");
      return title;
    }
  }
}
