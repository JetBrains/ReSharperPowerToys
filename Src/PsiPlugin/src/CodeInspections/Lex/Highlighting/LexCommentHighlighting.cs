using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("Comment", null, HighlightingGroupIds.LanguageUsage, "Comment", @"
          Comment", Severity.INFO, false, Internal = false)]
namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Lex.Highlighting
{
  [ConfigurableSeverityHighlighting("Comment", "Lex", OverlapResolve = OverlapResolveKind.NONE, ToolTipFormatString = "Comment")]
  internal class LexCommentHighlighting : ICustomAttributeIdHighlighting, IHighlightingWithRange
  {
    private const string AtributeId = HighlightingAttributeIds.JAVA_SCRIPT_XML_DOC_TAG;
    private readonly ITreeNode myNode;

    public LexCommentHighlighting(ITreeNode node)
    {
      myNode = node;
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
      get { return AtributeId; }
    }

    #endregion

    #region IHighlightingWithRange Members

    public DocumentRange CalculateRange()
    {
      return myNode.GetDocumentRange();
    }

    #endregion
  }
}
