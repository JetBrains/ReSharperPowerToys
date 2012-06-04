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
      throw new NotImplementedException();
    }
  }
}
