using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("Keyword", null, HighlightingGroupIds.LanguageUsage, "Keyword", @"
          Keyword", Severity.INFO, false, Internal = false)]

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Highlightings
{
  [ConfigurableSeverityHighlighting("Keyword", "PSI", OverlapResolve = OverlapResolveKind.NONE, ToolTipFormatString = "Keyword")]
  //[StaticSeverityHighlighting(Severity.INFO, HighlightingGroupIds.CodeInfo,
    //OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = false)]
  internal class PsiKeywordHighlighting : ICustomAttributeIdHighlighting, IHighlightingWithRange
  {
    private readonly ITreeNode myElement;
    private const string AtributeId = HighlightingAttributeIds.CONSTANT_IDENTIFIER_ATTRIBUTE;

    public PsiKeywordHighlighting(ITreeNode element)
    {
      myElement = element;
    }

    public bool IsValid()
    {
      return true;
    }

    public string ToolTip
    {
      get { return null; }
    }

    public string ErrorStripeToolTip
    {
      get { return null; }
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