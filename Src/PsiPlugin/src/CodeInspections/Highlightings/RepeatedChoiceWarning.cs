using System;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Highlightings
{
  [StaticSeverityHighlighting(Severity.INFO, HighlightingGroupIds.CompilerWarnings,
    OverlapResolve = OverlapResolveKind.WARNING, ShowToolTipInStatusBar = false)]
  class RepeatedChoiceWarning : IHighlightingWithRange
  {
    private ITreeNode myElement;
    private string myError = "Repeated choice";

    public RepeatedChoiceWarning(ITreeNode element)
    {
      myElement = element;
    }

    public RepeatedChoiceWarning(ITreeNode element, String message)
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
  }
}
