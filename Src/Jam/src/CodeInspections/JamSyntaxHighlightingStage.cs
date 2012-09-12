using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.Jam.CodeInspections.Highlightings;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.CodeInspections
{
  [DaemonStage(StagesBefore = new[] { typeof (LanguageSpecificDaemonStage) })]
  public class JamSyntaxHighlightingStage : JamDaemonStageBase
  {
    protected override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind, IJamFile file)
    {
      return new Process(process, settings, file);
    }

    private class Process : JamDaemonProcessBase
    {
      public Process(IDaemonProcess process, IContextBoundSettingsStore settingsStore, IJamFile file) : base(process, settingsStore, file) {}

      public override void VisitNode(ITreeNode node, IHighlightingConsumer context)
      {
        var tokenNodeType = node.GetTokenType();
        if (tokenNodeType != null)
        {
          if (tokenNodeType.IsComment)
            context.AddHighlighting(new JamIdentifierHighlighting(JamHighlightingAttributeIds.JAM_COMMENT), node.GetDocumentRange(), File);
          else if (tokenNodeType.IsKeyword)
            context.AddHighlighting(new JamIdentifierHighlighting(JamHighlightingAttributeIds.JAM_KEYWORD), node.GetDocumentRange(), File);
          else if (tokenNodeType.IsStringLiteral)
            context.AddHighlighting(new JamIdentifierHighlighting(JamHighlightingAttributeIds.JAM_STRING_LITERAL), node.GetDocumentRange(), File);
          else if (tokenNodeType.IsConstantLiteral)
            context.AddHighlighting(new JamIdentifierHighlighting(JamHighlightingAttributeIds.JAM_NUMERIC_LITERAL), node.GetDocumentRange(), File);
        }

        base.VisitNode(node, context);
      }
    }
  }
}