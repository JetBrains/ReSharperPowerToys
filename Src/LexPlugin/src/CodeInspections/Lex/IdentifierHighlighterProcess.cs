using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.LexPlugin.CodeInspections.Lex.Highlighting;
using JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.LexPlugin.CodeInspections.Lex
{
  public class IdentifierHighlighterProcess : LexIncrementalDaemonStageProcessBase
  {
    public IdentifierHighlighterProcess(IDaemonProcess daemonProcess, IContextBoundSettingsStore settingsStore)
      : base(daemonProcess, settingsStore)
    {
    }

    private void AddHighLighting(DocumentRange range, ITreeNode element, IHighlightingConsumer consumer, IHighlighting highlighting)
    {
      var info = new HighlightingInfo(range, highlighting, new Severity?());
      IFile file = element.GetContainingFile();
      if (file != null)
      {
        consumer.AddHighlighting(info.Highlighting, file);
      }
    }

    public override void VisitTokenTypeName(ITokenTypeName tokenTypeNameParam, IHighlightingConsumer consumer)
    {
      DocumentRange colorConstantRange = tokenTypeNameParam.GetDocumentRange();
      AddHighLighting(colorConstantRange, tokenTypeNameParam, consumer, new LexTokenHighlighting(tokenTypeNameParam));
      base.VisitTokenTypeName(tokenTypeNameParam, consumer);
    }

    public override void VisitTokenDeclaredName(ITokenDeclaredName tokenDeclaredNameParam, IHighlightingConsumer consumer)
    {
      DocumentRange colorConstantRange = tokenDeclaredNameParam.GetDocumentRange();
      AddHighLighting(colorConstantRange, tokenDeclaredNameParam, consumer, new LexTokenHighlighting(tokenDeclaredNameParam));
      base.VisitTokenDeclaredName(tokenDeclaredNameParam, consumer);
    }

    public override void VisitStateName(IStateName stateNameParam, IHighlightingConsumer consumer)
    {
      DocumentRange colorConstantRange = stateNameParam.GetDocumentRange();
      ResolveResultWithInfo resolve = stateNameParam.StateNameReference.Resolve();
      if ((resolve == null) || ((resolve.Result.DeclaredElement == null) && (resolve.Result.Candidates.Count == 0)))
      {
        AddHighLighting(colorConstantRange, stateNameParam, consumer, new LexUnresolvedStateHighlighting(stateNameParam));
      } else
      {
        AddHighLighting(colorConstantRange, stateNameParam, consumer, new LexStateHighlighting(stateNameParam));
      }
      base.VisitStateName(stateNameParam, consumer);
    }

    public override void VisitStateDeclaredName(IStateDeclaredName stateDeclaredNameParam, IHighlightingConsumer consumer)
    {
      DocumentRange colorConstantRange = stateDeclaredNameParam.GetDocumentRange();
      AddHighLighting(colorConstantRange, stateDeclaredNameParam, consumer, new LexStateHighlighting(stateDeclaredNameParam));
      base.VisitStateDeclaredName(stateDeclaredNameParam, consumer);
    }
  }
}
