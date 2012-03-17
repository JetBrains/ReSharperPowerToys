using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("SyntaxError", null, HighlightingGroupIds.LanguageUsage, "Syntax Error", @"
          Syntax error", Severity.ERROR, false, Internal = false)]
namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Highlightings
{
  [ConfigurableSeverityHighlighting("SyntaxError", "PSI", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = Error)]
  class PsiErrorElementHighlighting : IHighlightingWithRange, ICustomAttributeIdHighlighting
  {
    private readonly ITreeNode myElement;
    private const string Error = "Syntax error";
    private const string AtributeId = HighlightingAttributeIds.ERROR_ATTRIBUTE;

    public PsiErrorElementHighlighting(ITreeNode element)
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

    public string AttributeId
    {
      get { return AtributeId; }
    }
  }
}
