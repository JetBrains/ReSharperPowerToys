using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("Variable", null, HighlightingGroupIds.CodeSmell, "Variable",
    @"Variable", Severity.INFO, false, Internal = false)]

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services
{
  [ConfigurableSeverityHighlighting("Variable", "PSI", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = "Variable")]
  internal class PsiVariableHighlighting : ICustomAttributeIdHighlighting, IHighlightingWithRange
  {
    private ITreeNode myElement;
    private readonly string myAtributeId = HighlightingAttributeIds.LOCAL_VARIABLE_IDENTIFIER_ATTRIBUTE;

    public PsiVariableHighlighting(ITreeNode element)
    {
      myElement = element;
    }

    public bool IsValid()
    {
      return true;
    }

    public string ToolTip
    {
      get { return "Variable"; }
    }

    public string ErrorStripeToolTip
    {
      get { return "Variable"; }
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
