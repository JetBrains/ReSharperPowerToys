using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("LeftRecursion", null, HighlightingGroupIds.LanguageUsage, "Left recursion", @"
          Left recursion", JetBrains.ReSharper.Daemon.Severity.WARNING, false, Internal = false)]
namespace JetBrains.ReSharper.PsiPlugin.Feature.Services
{
  [ConfigurableSeverityHighlighting("LeftRecursion", "PSI", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = myMessage)]
  class LeftRecursionWarning : IHighlightingWithRange
  {
    private ITreeNode myElement;
    private const string myMessage = "Left recursion";
    private string myError = "Left recursion";

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
  }
}
