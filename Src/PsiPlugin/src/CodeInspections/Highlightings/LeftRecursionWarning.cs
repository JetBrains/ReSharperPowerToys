using System;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("LeftRecursion", null, HighlightingGroupIds.LanguageUsage, "Left Recursion", @"
          Left Recursion", Severity.WARNING, false, Internal = false)]

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Highlightings
{
    [ConfigurableSeverityHighlighting("LeftRecursion", "PSI", OverlapResolve = OverlapResolveKind.WARNING, ToolTipFormatString = "Left Recursion")]
  //[StaticSeverityHighlighting(Severity.INFO, HighlightingGroupIds.CompilerWarnings,
    //OverlapResolve = OverlapResolveKind.WARNING, ShowToolTipInStatusBar = false)]
  class LeftRecursionWarning : IHighlightingWithRange, ICustomAttributeIdHighlighting
  {
    private readonly ITreeNode myElement;
    private readonly string myError = "Left Recursion";
    private const string AtributeId = HighlightingAttributeIds.WARNING_ATTRIBUTE;

    public LeftRecursionWarning(ITreeNode element)
    {
      myElement = element;
    }

    public LeftRecursionWarning(ITreeNode element, String message)
    {
      myElement = element;
      myError = message;
    }
    public bool IsValid()
    {
      return true;
    }

    public string ToolTip
    {
      get { return myError; }
    }

    public string ErrorStripeToolTip
    {
      get { return myError; }
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
