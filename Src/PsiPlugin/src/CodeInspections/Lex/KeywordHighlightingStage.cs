using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.CodeInspections.Lex.Highlighting;
using JetBrains.ReSharper.PsiPlugin.Psi.Lex.Parsing;
using JetBrains.ReSharper.PsiPlugin.Psi.Lex.Tree.Impl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Lex
{
  [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
  public class KeywordHighlightingStage : LexDaemonStageBase
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

    private class KeywordHighlightingProcess : LexIncrementalDaemonStageProcessBase
    {
      public KeywordHighlightingProcess(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
        : base(process, settingsStore)
      {
      }

      public override void VisitNode(ITreeNode node, IHighlightingConsumer consumer)
      {
        String s = node.GetText();
        //if (LexLexer.IsKeyword(s))
        var keywordToken = node as ITokenNode;
        if((keywordToken != null) && ( LexTokenType.KEYWORDS.Contains(keywordToken.GetTokenType())))
        {
          AddHighlighting(consumer, node);
        }
        else
        {
          var token = node as LexGenericToken;
          if (token != null)
          {
            if (token.GetTokenType().IsStringLiteral)
            {
              AddHighlighting(consumer, new LexStringLiteralHighlighting(node));
            }
            else if (token.GetTokenType().IsComment)
            {
              AddHighlighting(consumer, new LexCommentHighlighting(node));
            }
          }
        }
      }

      private void AddHighlighting([NotNull] IHighlightingConsumer consumer, [NotNull] ITreeNode expression)
      {
        consumer.AddHighlighting(new LexKeywordHighlighting(expression), File);
      }

      private void AddHighlighting([NotNull] IHighlightingConsumer consumer, IHighlighting highlighting)
      {
        consumer.AddHighlighting(highlighting, File);
      }
    }

    #endregion
  }
}
