using JetBrains.Application;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.TextControl;

namespace JetBrains.ReSharper.PsiPlugin.Intentions.CreateFromUsage
{
  [ShellComponent]
  public class PsiIntentionResultBehavior
  {
    private HotspotSessionExecutor HotspotSessionExecutor { get; set; }

    public PsiIntentionResultBehavior(HotspotSessionExecutor hotspotSessionExecutor)
    {
      HotspotSessionExecutor = hotspotSessionExecutor;
    }

    public void OnHotspotSessionExecutionStarted(PsiIntentionResult result, ITextControl textControl)
    {
      OnHotspotSessionExecutionStartedInternal(result, textControl);
    }

    protected virtual void OnHotspotSessionExecutionStartedInternal(PsiIntentionResult result, ITextControl textControl)
    {
      var hotspotSessionUi = HotspotSessionExecutor.CurrentSession;
      if (hotspotSessionUi == null)
        SetCaretPosition(textControl, result);
      else
        hotspotSessionUi.HotspotSession.Closed += (session, type) =>
          {
            if (type != TerminationType.Finished) return;
            SetCaretPosition(textControl, result);
          };
    }

    protected static void SetCaretPosition(ITextControl textControl, PsiIntentionResult result)
    {
      if (result.PrefferedSelection != DocumentRange.InvalidRange)
      {
        textControl.Selection.SetRange(result.PrefferedSelection.TextRange);
      }
    }
  }
}