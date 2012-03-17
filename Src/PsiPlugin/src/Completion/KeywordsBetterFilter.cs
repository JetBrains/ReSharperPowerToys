using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Util;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.PsiPlugin.Completion.LookupItems;

namespace JetBrains.ReSharper.PsiPlugin.Completion
{
  class KeywordsBetterFilter : ILookupItemsPreference
  {
    public int Order
    {
      get { return 10; }
    }

    public IEnumerable<ILookupItem> FilterItems(ICollection<ILookupItem> items)
    {
      return items.Where(x => x is PsiKeywordLookupItem || x is TemplateLookupItem);
    }
  }
}
