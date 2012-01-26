using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("Rule", null, HighlightingGroupIds.CodeSmell, "Rule",
    @"Rule", Severity.INFO, false, Internal = false)]

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services
{
     [ConfigurableSeverityHighlighting("Rule", "PSI", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = "Rule")]
  internal class PsiRuleHighlighting : ICustomAttributeIdHighlighting, IHighlightingWithRange
  {
    private ITreeNode myElement;
    private readonly string myAtributeId = HighlightingAttributeIds.METHOD_IDENTIFIER_ATTRIBUTE;

    public PsiRuleHighlighting(ITreeNode element)
    {
      myElement = element;
    }

    public bool IsValid()
    {
      return true;
    }

    public string ToolTip
    {
      get { return "Rule"; }
    }

    public string ErrorStripeToolTip
    {
      get { return "Rule"; }
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
