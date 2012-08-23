using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeAnnotations;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.CodeInspections.Lex.Highlighting;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Lex
{
  [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
  public class ErrorElementHighlighting : LexDaemonStageBase
  {
    public ErrorElementHighlighting(CodeAnnotationsCache codeAnnotationsCache)
    {
    }

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
        var element = node as IErrorElement;
        if (element != null)
        {
          if (element.GetTextLength() == 0)
          {
            ITreeNode parent = element.Parent;
            while ((parent != null) && (parent.GetTextLength() == 0))
            {
              parent = parent.Parent;
            }
            if (parent != null)
            {
              AddHighlighting(consumer, parent);
            }
          }
          else
          {
            AddHighlighting(consumer, element);
          }
        }
      }

      private void AddHighlighting([NotNull] IHighlightingConsumer consumer, [NotNull] ITreeNode expression)
      {
        consumer.AddHighlighting(new LexErrorElementHighlighting(expression), File);
      }
    }

    #endregion
  }
}
