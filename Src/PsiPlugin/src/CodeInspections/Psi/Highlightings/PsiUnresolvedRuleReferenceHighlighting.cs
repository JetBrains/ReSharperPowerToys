using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
using JetBrains.ReSharper.PsiPlugin.Resolve;

[assembly: RegisterConfigurableSeverity("UnresolvedReference", null, HighlightingGroupIds.LanguageUsage, "Unresolved reference", @"
          Unresolved reference", Severity.ERROR, false, Internal = false)]

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi.Highlightings
{
  [ConfigurableSeverityHighlighting("UnresolvedReference", "PSI", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = Error)]
  internal class PsiUnresolvedRuleReferenceHighlighting : IHighlightingWithRange
  {
    private const string Error = "Unresolved reference";
    private readonly ITreeNode myElement;
    private PsiRuleReference myReference;

    public PsiUnresolvedRuleReferenceHighlighting(RuleName element)
    {
      myElement = element;

       myReference = element.RuleNameReference;

    }

    #region IHighlightingWithRange Members

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

    #endregion

    public PsiRuleReference Reference
    {
      get { return myReference; }
    }
  }
}
