using System;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl.Markup;

[assembly:RegisterConfigurableSeverity("Keyword", null, HighlightingGroupIds.CodeSmell, "Keyword",
    @"Keyword", Severity.INFO, false, Internal = false)]


namespace JetBrains.ReSharper.PsiPlugin.Feature.Services
{
   [ConfigurableSeverityHighlighting("Keyword", "PSI", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = "Keyword")]
  internal class PsiKeywordHighlighting : ICustomAttributeIdHighlighting, IHighlightingWithRange
  {
    private ITreeNode myElement;
    private readonly string myAtributeId = HighlightingAttributeIds.CONSTANT_IDENTIFIER_ATTRIBUTE;

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
      get { return "Keyword"; }
    }

    public string ErrorStripeToolTip
    {
      get { return "Keyword"; }
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
       get { return myAtributeId; }
     }
  }
}