using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("UnresolvedState", null, HighlightingGroupIds.LanguageUsage, "Unresolved state", @"
          Unresolved state", Severity.ERROR, false, Internal = false)]

namespace JetBrains.ReSharper.LexPlugin.CodeInspections.Lex.Highlighting
{
  [ConfigurableSeverityHighlighting("UnresolvedState", "Lex", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = Error)]
  class LexUnresolvedStateHighlighting : IHighlighting
  {
    private const string Error = "Unresolved reference";
    private readonly ITreeNode myElement;
    private readonly IReference myReference;

    public LexUnresolvedStateHighlighting(IStateName element)
    {
      myElement = element;

      myReference = element.StateNameReference;

    }

    public bool IsValid()
    {
      return true;
    }

    public string ToolTip
    {
      get { return Error; }
    }

    public string ErrorStripeToolTip
    {
      get { return Error; }
    }

    public int NavigationOffsetPatch
    {
      get { return 0; }
    }

    public DocumentRange CalculateRange()
    {
      return myElement.GetNavigationRange();
    }

    public IReference Reference
    {
      get { return myReference; }
    }


  }
}
