using System;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Highlightings
{
  [StaticSeverityHighlighting(Severity.INFO, HighlightingGroupIds.CompilerWarnings,
    OverlapResolve = OverlapResolveKind.ERROR, ShowToolTipInStatusBar = false)]
  class DuplicatingLocalDeclarationError : IHighlightingWithRange, ICustomAttributeIdHighlighting
  {
    private readonly ITreeNode myElement;
    private readonly string myError = "Duplicate declaration";
    private const string AtributeId = HighlightingAttributeIds.ERROR_ATTRIBUTE;

    public DuplicatingLocalDeclarationError(ITreeNode element)
    {
      myElement = element;
    }

    public DuplicatingLocalDeclarationError(ITreeNode element, String message)
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
