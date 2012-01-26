using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Feature.Services.VisualElements;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeAnnotations;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Feature.Services;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.Util;
using JetBrains.Util.Special;

namespace JetBrains.ReSharper.PsiPlugin.DaemonStage
{
  [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
  public class KeywordHighlighting : PsiDaemonStageBase
  {
    private readonly CodeAnnotationsCache myCodeAnnotationsCache;

    public KeywordHighlighting(CodeAnnotationsCache codeAnnotationsCache)
    {
      myCodeAnnotationsCache = codeAnnotationsCache;
    }

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
      private readonly VisualElementHighlighter myHighlighter;
      private readonly IDocument myDocument;

      public KeywordHighlightingProcess(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
        : base(process, settingsStore)
      {
        myHighlighter = new VisualElementHighlighter(PsiLanguage.Instance, settingsStore);
        myDocument = process.Document;
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
