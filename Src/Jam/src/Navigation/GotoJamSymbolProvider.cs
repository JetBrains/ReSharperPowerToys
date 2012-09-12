using System.Collections.Generic;
using JetBrains.Application;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Goto;
using JetBrains.ReSharper.Feature.Services.Goto.GotoProviders;
using JetBrains.ReSharper.Feature.Services.Occurences;
using JetBrains.ReSharper.Feature.Services.Search;
using JetBrains.ReSharper.Psi.Jam.Cache;
using System.Linq;
using JetBrains.Util;
using JetBrains.Util.Special;

namespace JetBrains.ReSharper.Psi.Jam.Navigation
{
  [FeaturePart]
  public class CssGotoSymbolProvider : CachedGotoSymbolBase<JamSymbolsCache>, IGotoSymbolProvider
  {
    protected override IEnumerable<string> GetNames(JamSymbolsCache cache, INavigationScope scope)
    {
      using (ReadLockCookie.Create())
        return cache.GetAllNames();
    }

    protected override JamSymbolsCache GetCache(INavigationScope scope, ISolution solution, GotoContext gotoContext)
    {
      return solution.GetComponent<JamSymbolsCache>();
    }

    public override IEnumerable<IOccurence> GetOccurencesByMatchingInfo(MatchingInfo navigationInfo, INavigationScope scope, GotoContext gotoContext)
    {
      var solution = scope.GetSolution();

      var cache = GetCache(scope, solution, gotoContext);
      var symbols = cache.GetSymbols(navigationInfo.Identifier);

      return symbols.SelectNotNull(symbol => symbol.GetDeclaration().IfNotNull(declaration => declaration.DeclaredElement)).Select(element => new DeclaredElementOccurence(element)).Cast<IOccurence>();
    }

    public override bool IsApplicable(INavigationScope scope, GotoContext gotoContext)
    {
      return scope is SolutionNavigationScope;
    }
  }
}