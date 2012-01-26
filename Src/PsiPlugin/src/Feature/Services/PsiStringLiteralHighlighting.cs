using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("string", null, HighlightingGroupIds.LanguageUsage, "string", @"
          string", Severity.INFO, false, Internal = false)]
namespace JetBrains.ReSharper.PsiPlugin.Feature.Services
{
    [ConfigurableSeverityHighlighting("string", "PSI", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = myMessage)]
  internal class PsiStringLiteralHighlighting : ICustomAttributeIdHighlighting, IHighlightingWithRange
  {
    private ITreeNode myElement;
    private readonly string myAtributeId = HighlightingAttributeIds.TYPE_INTERFACE_ATTRIBUTE;
    private const string myMessage = "string";

    public PsiStringLiteralHighlighting(ITreeNode element)
    {
      myElement = element;
    }

    public bool IsValid()
    {
      return true;
    }

    public string ToolTip
    {
      get { return "string"; }
    }

    public string ErrorStripeToolTip
    {
      get { return "string"; }
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
