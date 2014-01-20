using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("Repeated Choice", null, HighlightingGroupIds.LanguageUsage, "Repeated Choice", @"
          Repeated Choice", Severity.WARNING, false, Internal = false)]

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi.Highlightings
{
  [ConfigurableSeverityHighlighting("Repeated Choice", "PSI", OverlapResolve = OverlapResolveKind.NONE, ToolTipFormatString = "Repeated Choice")]
  internal class RepeatedChoiceWarning : ICustomAttributeIdHighlighting
  {
    private const string Error = "Repeated Choice";
    private const string OurAttributeId = HighlightingAttributeIds.WARNING_ATTRIBUTE;
    private readonly ITreeNode myElement;

    public RepeatedChoiceWarning(ITreeNode element)
    {
      myElement = element;
    }

    public string AttributeId
    {
      get { return OurAttributeId; }
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
