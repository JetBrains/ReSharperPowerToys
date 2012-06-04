using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Feature.Services.ContextNavigation.ContextSearches;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.Navigation
{
  [FeaturePart]
  class GeneratedContextSearch : IContextSearch
  {
    public bool IsAvailable(IDataContext dataContext)
    {
      throw new NotImplementedException();
    }

    public bool IsApplicable(IDataContext dataContext)
    {
      var psiProperty = dataContext.GetData(ReSharper.Psi.Services.DataConstants.DECLARED_ELEMENT);
      if (psiProperty != null)
        return true;
      return false;
    }
  }
}
