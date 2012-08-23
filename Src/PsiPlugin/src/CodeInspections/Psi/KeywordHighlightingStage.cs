using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi.Highlightings;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi
{
  [DaemonStage(StagesBefore = new[] { typeof (LanguageSpecificDaemonStage) })]
  public class KeywordHighlightingStage : PsiDaemonStageBase
  {
    public override ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
    {
      return ErrorStripeRequest.STRIPE_AND_ERRORS;
    }

    public override IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
    {
      if (!IsSupported(process.SourceFile))
      {
        return EmptyList<IDaemonStageProcess>.InstanceList;
      }

      return new List<IDaemonStageProcess> { new KeywordHighlightingProcess(process, settings) };
    }

    #region Nested type: KeywordHighlightingProcess

    private class KeywordHighlightingProcess : PsiIncrementalDaemonStageProcessBase
    {
      public KeywordHighlightingProcess(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
        : base(process, settingsStore)
      {
      }

      public override void VisitNode(ITreeNode node, IHighlightingConsumer consumer)
      {
        String s = node.GetText();
        if (PsiLexer.IsKeyword(s))
        {
          AddHighlighting(consumer, node);
        }
        else
        {
          var token = node as PsiGenericToken;
          if (token != null)
          {
            if (token.GetTokenType().IsStringLiteral)
            {
              AddHighlighting(consumer, new PsiStringLiteralHighlighting(node));
            }
            else if (token.GetTokenType().IsComment)
            {
              AddHighlighting(consumer, new PsiCommentHighlighting(node));
            }
          }
        }
      }

      private void AddHighlighting([NotNull] IHighlightingConsumer consumer, [NotNull] ITreeNode expression)
      {
        consumer.AddHighlighting(new PsiKeywordHighlighting(expression), File);
      }

      private void AddHighlighting([NotNull] IHighlightingConsumer consumer, IHighlighting highlighting)
      {
        consumer.AddHighlighting(highlighting, File);
      }
    }

    #endregion
  }
}
