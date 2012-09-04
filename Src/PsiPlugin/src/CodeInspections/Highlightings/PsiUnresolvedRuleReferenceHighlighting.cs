using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;

[assembly: RegisterConfigurableSeverity("UnresolvedReference", null, HighlightingGroupIds.LanguageUsage, "Unresolved reference", @"
          Unresolved reference", Severity.ERROR, false, Internal = false)]

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Highlightings
{
  [ConfigurableSeverityHighlighting("UnresolvedReference", "PSI", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = Error)]
  public class PsiUnresolvedRuleReferenceHighlighting : IHighlightingWithRange
  {
    private const string Error = "Unresolved reference";
    private readonly ITreeNode myElement;
    private PsiRuleReference myReference;

    public PsiUnresolvedRuleReferenceHighlighting(ITreeNode element)
    {
      myElement = element;

      if (element is RuleName)
      {
        myReference = (element as RuleName).RuleNameReference;
      }

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
