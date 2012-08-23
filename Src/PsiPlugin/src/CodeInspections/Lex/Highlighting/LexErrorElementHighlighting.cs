using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.UI.Resources;


[assembly: RegisterConfigurableSeverity("SyntaxError", null, HighlightingGroupIds.LanguageUsage, "Syntax Error", @"
          Syntax error", Severity.ERROR, false, Internal = false)]
namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Lex.Highlighting
{
  [ConfigurableSeverityHighlighting("SyntaxError", "Lex", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = Error)]
  public class LexErrorElementHighlighting : IHighlightingWithRange, ICustomAttributeIdHighlighting
  {
    private const string Error = "Syntax error";
    private const string AtributeId = HighlightingAttributeIds.ERROR_ATTRIBUTE;
    private readonly ITreeNode myElement;

    public LexErrorElementHighlighting(ITreeNode element)
    {
      myElement = element;
    }

    #region ICustomAttributeIdHighlighting Members

    public string AttributeId
    {
      get { return AtributeId; }
    }

    #endregion

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
  }
}
