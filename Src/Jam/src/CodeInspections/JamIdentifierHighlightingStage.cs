using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.Jam.CodeInspections.Highlightings;
using JetBrains.ReSharper.Psi.Jam.Impl;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.CodeInspections
{
  [DaemonStage(StagesBefore = new[] { typeof (JamErrorElementHighlightingStage) })]
  public class JamIdentifierHighlightingStage : JamDaemonStageBase
  {
    protected override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind, IJamFile file)
    {
      if (!settings.GetValue(HighlightingSettingsAccessor.IdentifierHighlightingEnabled))
        return null;

      return new Process(process, settings, file);
    }

    private class Process : JamDaemonProcessBase
    {
      public Process(IDaemonProcess process, IContextBoundSettingsStore settingsStore, IJamFile file) : base(process, settingsStore, file) {}

      public override void VisitGlobalVariableDeclaration(IGlobalVariableDeclaration globalVariableDeclarationParam, IHighlightingConsumer context)
      {
        var identifier = globalVariableDeclarationParam.Name;

        if (identifier != null)
          AddIdentifierHighlighting(context, identifier.GetDocumentRange(), JamDeclaredElementType.GlobalVariable);

        base.VisitGlobalVariableDeclaration(globalVariableDeclarationParam, context);
      }

      public override void VisitLocalVariableDeclarationStatement(ILocalVariableDeclarationStatement localVariableDeclarationStatementParam, IHighlightingConsumer context)
      {
        var identifier = localVariableDeclarationStatementParam.Name;

        if (identifier != null)
          AddIdentifierHighlighting(context, identifier.GetDocumentRange(), JamDeclaredElementType.LocalVariable);

        base.VisitLocalVariableDeclarationStatement(localVariableDeclarationStatementParam, context);
      }

      public override void VisitProcedureDeclaration(IProcedureDeclaration procedureDeclarationParam, IHighlightingConsumer context)
      {
        var identifier = procedureDeclarationParam.Name;

        if (identifier != null)
          AddIdentifierHighlighting(context, identifier.GetDocumentRange(), JamDeclaredElementType.Procedure);

        base.VisitProcedureDeclaration(procedureDeclarationParam, context);
      }

      public override void VisitInvocationExpression(IInvocationExpression invocationExpressionParam, IHighlightingConsumer context)
      {
        var identifier = invocationExpressionParam.Name;

        if (identifier != null)
          AddIdentifierHighlighting(context, identifier.GetDocumentRange(), JamDeclaredElementType.Procedure);

        base.VisitInvocationExpression(invocationExpressionParam, context);
      }

      public override void VisitParameter(Tree.IParameter parameterParam, IHighlightingConsumer context)
      {
        var identifier = parameterParam.Name;

        if (identifier != null)
          AddIdentifierHighlighting(context, identifier.GetDocumentRange(), JamDeclaredElementType.Parameter);

        base.VisitParameter(parameterParam, context);
      }

      public override void VisitIdentifierExpression(IIdentifierExpression identifierExpressionParam, IHighlightingConsumer context)
      {
        var identifier = identifierExpressionParam.LiteralToken;

        if (identifier != null)
        {
          var declaredElement = identifierExpressionParam.Reference.Resolve().DeclaredElement;
          if (declaredElement != null)
            AddIdentifierHighlighting(context, identifier.GetDocumentRange(), declaredElement.GetElementType());
        }

        base.VisitIdentifierExpression(identifierExpressionParam, context);
      }

      private void AddIdentifierHighlighting(IHighlightingConsumer context, DocumentRange range, DeclaredElementType elementType)
      {
        var attributeId = JamHighlightingAttributeIds.GetAttributeId(elementType);
        if (attributeId != null)
          context.AddHighlighting(new JamIdentifierHighlighting(attributeId), range, File);
      }
    }
  }
}