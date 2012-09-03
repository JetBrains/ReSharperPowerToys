using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("Token", null, HighlightingGroupIds.LanguageUsage, "Token", @"
          Token", Severity.INFO, false, Internal = false)]

namespace JetBrains.ReSharper.LexPlugin.CodeInspections.Lex.Highlighting
{
  [ConfigurableSeverityHighlighting("Token", "Lex", OverlapResolve = OverlapResolveKind.NONE, ToolTipFormatString = "Token")]
  internal class LexTokenHighlighting : ICustomAttributeIdHighlighting, IHighlightingWithRange
  {
    private const string Attribute = HighlightingAttributeIds.METHOD_IDENTIFIER_ATTRIBUTE;
    private readonly ITreeNode myElement;

    public LexTokenHighlighting(ITreeNode element)
    {
      myElement = element;
    }

    #region ICustomAttributeIdHighlighting Members

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

    public string AttributeId
    {
      get { return Attribute; }
    }

    #endregion

    #region IHighlightingWithRange Members

    public DocumentRange CalculateRange()
    {
      return myElement.GetNavigationRange();
    }

    #endregion
  }
}
