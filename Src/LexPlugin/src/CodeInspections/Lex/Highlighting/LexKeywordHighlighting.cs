﻿using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("Keyword", null, HighlightingGroupIds.LanguageUsage, "Keyword", @"
          Keyword", Severity.INFO, false, Internal = false)]

namespace JetBrains.ReSharper.LexPlugin.CodeInspections.Lex.Highlighting
{
  [ConfigurableSeverityHighlighting("Keyword", "Lex", OverlapResolve = OverlapResolveKind.NONE, ToolTipFormatString = "Keyword")]
  internal class LexKeywordHighlighting : ICustomAttributeIdHighlighting
  {
    private const string AtributeId = HighlightingAttributeIds.CONSTANT_IDENTIFIER_ATTRIBUTE;
    private readonly ITreeNode myElement;

    public LexKeywordHighlighting(ITreeNode element)
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

    public string AttributeId
    {
      get { return AtributeId; }
    }

    public DocumentRange CalculateRange()
    {
      return myElement.GetNavigationRange();
    }
  }
}
