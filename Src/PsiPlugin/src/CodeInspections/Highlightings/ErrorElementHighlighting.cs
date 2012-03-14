using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Feature.Services.VisualElements;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeAnnotations;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.Util.Special;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Highlightings
{
  [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
  public class ErrorElementHighlighting : PsiDaemonStageBase
  {
    private readonly CodeAnnotationsCache myCodeAnnotationsCache;

    public ErrorElementHighlighting(CodeAnnotationsCache codeAnnotationsCache)
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
      //return project != null ? new LocalizableElementProcess(settings.GetValue((PsiLocalizationOptionsSettings lo) => lo.DontAnalyseVerbatimStrings), LocalizableProperty.GetLocalizableProperty(settings), LocalizableInspectorProperty.GetLocalizableInspectorProperty(settings), myCodeAnnotationsCache, process, settings) : null;
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
        IErrorElement element = node as IErrorElement;
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
