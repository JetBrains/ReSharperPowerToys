using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.Util;

[assembly: RegisterConfigurableSeverity("JamAmbiguousReferenceError", null, HighlightingGroupIds.CodeSmell, "Cannot resolve reference", @"Ambiguous reference.", Severity.ERROR, false, Internal = false)]

namespace JetBrains.ReSharper.Psi.Jam.CodeInspections.Highlightings
{
  [ConfigurableSeverityHighlighting("JamAmbiguousReferenceError", JamLanguage.Name, AttributeId = HighlightingAttributeIds.UNRESOLVED_ERROR_ATTRIBUTE, OverlapResolve = OverlapResolveKind.WARNING, ToolTipFormatString = "Ambiguous reference:{0}{1}match")]
  public class JamAmbiguousReferenceError : JamHighlightingBase, IHighlightingWithRange
  {
    private readonly string myMessage;
    private readonly IReference myReference;

    public JamAmbiguousReferenceError(IReference reference)
    {
      myReference = reference;
      myMessage = string.Format("Ambiguous reference:{0}{1}match", CandidatesString(reference.Resolve().Result.Candidates), Environment.NewLine);
    }

    public IReference Reference
    {
      get { return myReference; }
    }

    public string ToolTip
    {
      get { return myMessage; }
    }

    public string ErrorStripeToolTip
    {
      get { return ToolTip; }
    }

    public int NavigationOffsetPatch
    {
      get { return 0; }
    }

    DocumentRange IHighlightingWithRange.CalculateRange()
    {
      return Reference.GetDocumentRange();
    }

    public override bool IsValid()
    {
      return Reference.IsValid();
    }

    private static string CandidatesString(IList<IDeclaredElement> candidates)
    {
      return candidates.Select(element => string.Format("{0}  {1}", Environment.NewLine, DeclaredElementPresenter.Format(JamLanguage.Instance, DeclaredElementPresenter.KIND_NAME_PRESENTER, element))).OrderBy().AggregateString(string.Empty);
    }
  }
}