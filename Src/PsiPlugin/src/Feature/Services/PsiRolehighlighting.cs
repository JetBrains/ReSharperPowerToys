using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("Role", null, HighlightingGroupIds.CodeSmell, "Role",
    @"Role", Severity.INFO, false, Internal = false)]

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services
{
  [ConfigurableSeverityHighlighting("Role", "PSI", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = "Role")]
  internal class PsiRoleHighlighting : ICustomAttributeIdHighlighting, IHighlightingWithRange
  {
    private ITreeNode myElement;
    private readonly string myAtributeId = HighlightingAttributeIds.PARAMETER_IDENTIFIER_ATTRIBUTE;

    public PsiRoleHighlighting(ITreeNode element)
    {
      myElement = element;
    }

    public bool IsValid()
    {
      return true;
    }

    public string ToolTip
    {
      get { return "Role"; }
    }

    public string ErrorStripeToolTip
    {
      get { return "Role"; }
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
