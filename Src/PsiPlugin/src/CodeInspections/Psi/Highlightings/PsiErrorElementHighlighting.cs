using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("SyntaxError", null, HighlightingGroupIds.LanguageUsage, "Syntax Error", @"
          Syntax error", Severity.ERROR, false, Internal = false)]

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi.Highlightings
{
  [ConfigurableSeverityHighlighting("SyntaxError", "PSI", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = Error)]
  internal class PsiErrorElementHighlighting : ICustomAttributeIdHighlighting
  {
    private const string Error = "Syntax error";
    private const string OurAttributeId = HighlightingAttributeIds.ERROR_ATTRIBUTE;
    private readonly ITreeNode myElement;

    public PsiErrorElementHighlighting(ITreeNode element)
    {
      myElement = element;
    }

    #region ICustomAttributeIdHighlighting Members

    public string AttributeId
    {
      get { return OurAttributeId; }
    }

    #endregion

    #region IHighlighting Members

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
  }
}
