using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;

[assembly: RegisterConfigurableSeverity("UnresolvedReference", null, HighlightingGroupIds.LanguageUsage, "Unresolved reference", @"
          Unresolved reference", Severity.ERROR, false, Internal = false)]
namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi.Highlightings
{
   [ConfigurableSeverityHighlighting("UnresolvedReference", "PSI", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = Error)]
  internal class PsiUnresolvedVariableReferenceHighlighting : IHighlighting
  {
    private const string Error = "Unresolved reference";
    private readonly ITreeNode myElement;
    private readonly IReference myReference;

    public PsiUnresolvedVariableReferenceHighlighting(VariableName element)
    {
      myElement = element;

      myReference = element.Reference;

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
