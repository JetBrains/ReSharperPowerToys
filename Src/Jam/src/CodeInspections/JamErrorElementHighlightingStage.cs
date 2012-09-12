using System;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.Jam.CodeInspections.Highlightings;
using JetBrains.ReSharper.Psi.Jam.Parsing;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.CodeInspections
{
  [DaemonStage(StagesBefore = new[] { typeof (JamSyntaxHighlightingStage) })]
  public class JamErrorElementHighlightingStage : JamDaemonStageBase
  {
    protected override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind, IJamFile file)
    {
      return new Process(process, settings, file);
    }

    private class Process : JamDaemonProcessBase
    {
      public Process(IDaemonProcess process, IContextBoundSettingsStore settingsStore, IJamFile file) : base(process, settingsStore, file) {}

      public override void VisitNode(ITreeNode node, IHighlightingConsumer consumer)
      {
        var errorElement = node as IErrorElement;
        if (errorElement != null)
        {
          if (errorElement.GetTextLength() == 0)
          {
            var parent = errorElement.Parent;
            while ((parent != null) && (parent.GetTextLength() == 0))
              parent = parent.Parent;

            if (parent != null)
              consumer.AddHighlighting(new JamSyntaxError(parent.GetDocumentRange()), File);
          }
          else
            consumer.AddHighlighting(new JamSyntaxError(errorElement.GetDocumentRange(), errorElement.ErrorDescription), File);
        }
        else if (node.GetTokenType() == JamTokenType.COMMENT)
        {
          var text = node.GetText();
          {
            if (!text.EndsWith("*/", StringComparison.OrdinalIgnoreCase))
            {
              var range = node.GetDocumentRange();
              consumer.ConsumeHighlighting(range, new JamSyntaxError(range, "Invalid comment"));
            }
          }
        }
        else if (node.GetTokenType() == JamTokenType.STRING_LITERAL)
        {
          var text = node.GetText();
          if (text.StartsWith("\"", StringComparison.OrdinalIgnoreCase))
          {
            if (!text.EndsWith("\"", StringComparison.OrdinalIgnoreCase))
            {
              var range = node.GetDocumentRange();
              consumer.ConsumeHighlighting(range, new JamSyntaxError(range, "Invalid string value"));
            }
          }
        }
      }
    }
  }
}