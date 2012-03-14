using System;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeAnnotations;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.CodeInspections.Highlightings;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.Util.Special;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections
{
  [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
  public class KeywordHighlightingStage : PsiDaemonStageBase
  {
    public override ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
    {
      return ErrorStripeRequest.STRIPE_AND_ERRORS;
    }

    public override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
    {
      if (!IsSupported(process.SourceFile))
        return null;

      var project = process.SourceFile.ToProjectFile().IfNotNull(file => file.GetProject());
      IDocument document = process.Document;
      return new KeywordHighlightingProcess(process, settings);
    }

    private class KeywordHighlightingProcess : PsiIncrementalDaemonStageProcessBase
    {
      public KeywordHighlightingProcess(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
        : base(process, settingsStore)
      {
      }

      public override void VisitNode(ITreeNode node, IHighlightingConsumer consumer)
      {
        String s = node.GetText();
        if (PsiLexer.isKeyword(s))
        {
          AddHighlighting(consumer, node);
        } else
        {
          PsiGenericToken token = node as PsiGenericToken;
          if(token != null)
          {
            if(token.GetTokenType().IsStringLiteral)
            {
              AddHighlighting(consumer, new PsiStringLiteralHighlighting(node));  
            } else if (token.GetTokenType().IsComment)
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
  }
}
