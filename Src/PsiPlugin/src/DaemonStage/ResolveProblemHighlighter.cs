using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Resolve;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin
  .DaemonStage
{
  public class ResolveProblemHighlighter
  {
    private readonly ResolveHighlighterRegistrar myRegistrar;
    private readonly IReferenceProvider myReferenceProvider;

    public ResolveProblemHighlighter([NotNull] ITreeNode root, ResolveHighlighterRegistrar resolveHighlighterRegistrar)
    {
      Assertion.Assert(root != null, "root != null");

      myRegistrar = resolveHighlighterRegistrar;
      myReferenceProvider = ((IFileImpl)root.GetContainingFile()).ReferenceProvider;
    }

    public void CheckForResolveProblems(IHighlightingConsumer consumer, ITreeNode element)
    {
      foreach (IReference reference in element.GetReferences(null, myReferenceProvider))
        CheckForResolveProblems(consumer, reference);
    }

    public void CheckForResolveProblems(IHighlightingConsumer consumer, IReference reference)
    {
      var qualifiableReference = reference as IQualifiableReference;
      if (qualifiableReference != null && qualifiableReference.IsQualified)
      {
        IQualifier qualifier = qualifiableReference.GetQualifier();
        if (qualifier != null && !qualifier.Resolved)
          return;
      }

      ResolveErrorType error = reference.CheckResolveResult();
      if (error == null)
        throw new InvalidOperationException("ResolveErrorType is null for reference " + reference.GetType().FullName);

      if (error == ResolveErrorType.DYNAMIC) return;
      if (error == ResolveErrorType.IGNORABLE) return;

      if (error == ResolveErrorType.OK)
      {
        CheckForObsolete(consumer, reference);
      }
      else if (myRegistrar.ContainsHandler(PsiLanguage.Instance, error))
      {
        var highlighting = myRegistrar.GetResolveHighlighting(reference, error);
      }
      else if (error != ResolveErrorType.DYNAMIC)
      {
        //consumer.AddHighlighting(new NotResolvedError(reference));
      }
    }

    private static void CheckForObsolete(IHighlightingConsumer consumer, IReference reference)
    {
      var result = ObsoleteUtil.IsReferencingObsoleteEntity(reference);
      if (result == null)
        return;
    }
  }
}
