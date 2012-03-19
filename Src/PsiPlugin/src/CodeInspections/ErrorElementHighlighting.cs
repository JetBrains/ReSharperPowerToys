using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeAnnotations;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.CodeInspections.Highlightings;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections
{
  [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
  public class ErrorElementHighlighting : PsiDaemonStageBase
  {
    public ErrorElementHighlighting(CodeAnnotationsCache codeAnnotationsCache)
    {
    }

    public override ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
    {
      return ErrorStripeRequest.STRIPE_AND_ERRORS;
    }

    public override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
    {
      if (!IsSupported(process.SourceFile))
        return null;
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
        var element = node as IErrorElement;
        if (element != null)
        {
          if (element.GetTextLength() == 0)
          {
            var parent = element.Parent;
            while( (parent != null) && (parent.GetTextLength() == 0))
            {
              parent = parent.Parent;
            }
            if(parent != null)
            {
              AddHighlighting(consumer,parent);
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
        consumer.AddHighlighting(new PsiErrorElementHighlighting(expression), File);
      }
    }
  }
}
