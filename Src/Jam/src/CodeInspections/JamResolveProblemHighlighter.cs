using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Resolve;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Jam.CodeInspections.Highlightings;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Jam.CodeInspections
{
  public class JamResolveProblemHighlighter
  {
    private readonly IJamFile myFile;
    private readonly ResolveHighlighterRegistrar myRegistrar;

    public JamResolveProblemHighlighter([NotNull] IJamFile file, ResolveHighlighterRegistrar resolveHighlighterRegistrar)
    {
      Assertion.Assert(file != null, "file != null");

      myFile = file;
      myRegistrar = resolveHighlighterRegistrar;
    }

    public void CheckForResolveProblems(ITreeNode element, IHighlightingConsumer consumer)
    {
      foreach (var reference in element.GetReferences())
        CheckForResolveProblems(consumer, reference);
    }

    private void CheckForResolveProblems(IHighlightingConsumer consumer, IReference reference)
    {
      var qualifiableReference = reference as IQualifiableReference;
      if (qualifiableReference != null && qualifiableReference.IsQualified)
      {
        var qualifier = qualifiableReference.GetQualifier();
        if (qualifier != null && !qualifier.Resolved)
          return;
      }

      var error = reference.CheckResolveResult();
      if (error == null)
        throw new InvalidOperationException("ResolveErrorType is null for reference " + reference.GetType().FullName);

      if (error == ResolveErrorType.OK) return;
      if (error == ResolveErrorType.DYNAMIC) return;
      if (error == ResolveErrorType.IGNORABLE) return;
      if (!reference.GetDocumentRange().IsValid()) return;

      if (reference.Resolve().DeclaredElement == null)
      {
        var candidates = reference.Resolve().Result.Candidates;
        if (candidates.HasMultiple()) 
        {
          consumer.AddHighlighting(new JamAmbiguousReferenceError(reference), myFile);
          return;
        }
      }

      var highlighting = myRegistrar.GetResolveHighlighting(reference, error);
      consumer.AddHighlighting(highlighting ?? new JamSymbolNotResolvedError(reference), myFile);
    }
  }
}