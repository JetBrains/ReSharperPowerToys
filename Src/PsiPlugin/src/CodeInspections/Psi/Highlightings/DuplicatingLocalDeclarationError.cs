using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("Duplicate declaration", null, HighlightingGroupIds.LanguageUsage, "Duplicate declaration", @"
          Duplicate declaration", Severity.WARNING, false, Internal = false)]

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi.Highlightings
{
  [ConfigurableSeverityHighlighting("Duplicate declaration", "PSI", OverlapResolve = OverlapResolveKind.WARNING, ToolTipFormatString = "Duplicate declaration")]
  internal class DuplicatingLocalDeclarationError : ICustomAttributeIdHighlighting
  {
    private const string Error = "Duplicate declaration";
    private const string AtributeId = HighlightingAttributeIds.ERROR_ATTRIBUTE;
    private readonly ITreeNode myElement;

    public DuplicatingLocalDeclarationError(ITreeNode element)
    {
      myElement = element;
    }

    public string AttributeId
    {
      get { return AtributeId; }
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
