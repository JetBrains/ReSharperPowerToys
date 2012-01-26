using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("DuplicateDeclaration", null, HighlightingGroupIds.LanguageUsage, "Duplicate declaration", @"
          Duplicate declaration", JetBrains.ReSharper.Daemon.Severity.ERROR, false, Internal = false)]

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services
{
  [ConfigurableSeverityHighlighting("DuplicateDeclaration", "PSI", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = myMessage)]
  class DuplicatingLocalDeclarationWarning : IHighlightingWithRange
  {
    private ITreeNode myElement;
    private const string myMessage = "Duplicate declaration";
    private string myError = "Duplicate declaration";

    public DuplicatingLocalDeclarationWarning(ITreeNode element)
    {
      myElement = element;
    }

    public DuplicatingLocalDeclarationWarning(ITreeNode element, String message)
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
