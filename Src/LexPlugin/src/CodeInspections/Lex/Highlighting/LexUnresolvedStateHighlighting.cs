using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("UnresolvedState", null, HighlightingGroupIds.LanguageUsage, "Unresolved state", @"
          Unresolved state", Severity.ERROR, false, Internal = false)]

namespace JetBrains.ReSharper.LexPlugin.CodeInspections.Lex.Highlighting
{
  [ConfigurableSeverityHighlighting("UnresolvedState", "Lex", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = Error)]
  class LexUnresolvedStateHighlighting : IHighlightingWithRange
  {
    private const string Error = "Unresolved reference";
    private readonly ITreeNode myElement;
    private IReference myReference;

    public LexUnresolvedStateHighlighting(IStateName element)
    {
      myElement = element;

      myReference = element.StateNameReference;

    }

    #region IHighlightingWithRange Members

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

    public IReference Reference
    {
      get { return myReference; }
    }


  }
}
