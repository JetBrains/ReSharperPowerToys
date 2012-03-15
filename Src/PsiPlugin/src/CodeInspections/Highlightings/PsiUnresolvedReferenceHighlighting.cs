using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("UnresolvedReference", null, HighlightingGroupIds.LanguageUsage, "Unresolved reference", @"
          Unresolved reference", JetBrains.ReSharper.Daemon.Severity.ERROR, false, Internal = false)]
namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Highlightings
{
  [ConfigurableSeverityHighlighting("UnresolvedReference", "PSI", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = Error)]
  class PsiUnresolvedReferenceHighlighting : IHighlightingWithRange
  {
    private readonly ITreeNode myElement;
    private const string Error = "Unresolved reference";

    public PsiUnresolvedReferenceHighlighting(ITreeNode element)
    {
      myElement = element;
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
  }
}
