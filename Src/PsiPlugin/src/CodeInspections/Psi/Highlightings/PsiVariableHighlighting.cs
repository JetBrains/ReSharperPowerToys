using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("Variable", null, HighlightingGroupIds.LanguageUsage, "Variable", @"
          Variable", Severity.INFO, false, Internal = false)]

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi.Highlightings
{
  [ConfigurableSeverityHighlighting("Variable", "PSI", OverlapResolve = OverlapResolveKind.NONE, ToolTipFormatString = "Variable")]
  internal class PsiVariableHighlighting : ICustomAttributeIdHighlighting
  {
    private const string OurAttributeId = HighlightingAttributeIds.LOCAL_VARIABLE_IDENTIFIER_ATTRIBUTE;
    private readonly ITreeNode myElement;

    public PsiVariableHighlighting(ITreeNode element)
    {
      myElement = element;
    }

    public bool IsValid()
    {
      return true;
    }

    public string ToolTip
    {
      get { return "null"; }
    }

    public string ErrorStripeToolTip
    {
      get { return "null"; }
    }

    public int NavigationOffsetPatch
    {
      get { return 0; }
    }

    public string AttributeId
    {
      get { return OurAttributeId; }
    }

    public DocumentRange CalculateRange()
    {
      return myElement.GetNavigationRange();
    }
  }
}
