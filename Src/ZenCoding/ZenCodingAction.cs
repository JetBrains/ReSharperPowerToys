using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.Application.CommandProcessing;
using JetBrains.DocumentManagers;
using JetBrains.DocumentManagers.Transactions;
using JetBrains.DocumentManagers.impl;
using JetBrains.DocumentModel;
using JetBrains.DocumentModel.Transactions;
using JetBrains.Interop.WinApi;
using JetBrains.UI.Interop;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ZenCoding
{
  [ActionHandler]
  public class PowerToys_ZenCodingAction : ZenCodingActionBase
  {
    private DocumentManager documentManager;
    private readonly ICommandProcessor commandProcessor;
    private readonly SolutionDocumentTransactionManager documentTransactionManager;

    public PowerToys_ZenCodingAction(DocumentManager documentManager, ICommandProcessor commandProcessor, SolutionDocumentTransactionManager documentTransactionManager)
    {
      this.documentManager = documentManager;
      this.commandProcessor = commandProcessor;
      this.documentTransactionManager = documentTransactionManager;
    }

    public override void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      var solution = context.GetData(IDE.DataConstants.SOLUTION);
      Assertion.AssertNotNull(solution, "solution == null");
      var textControl = context.GetData(IDE.DataConstants.TEXT_CONTROL);
      Assertion.AssertNotNull(textControl, "textControl == null");

      
      using (commandProcessor.UsingCommand("ZenCoding"))
      {
        documentTransactionManager.StartTransaction("ZenCoding");

        string abbr;
        
        TextRange abbrRange = textControl.Selection.DocRange;
        if (abbrRange.IsValid && !abbrRange.IsEmpty)
        {
          abbr = textControl.Document.GetText(abbrRange);
        }
        else
        {
          var coords = textControl.Caret.PositionValue.ToDocLineColumn();
          int start;
          abbr = GetEngine(solution).FindAbbreviationInLine(textControl.Document.GetLineText(coords.Line), (int)coords.Column, out start);
          if (start == -1)
          {
            Win32Declarations.MessageBeep(MessageBeepType.Error);
            return;
          }
          abbrRange = TextRange
            .FromLength(textControl.Caret.PositionValue.ToDocOffset(), -abbr.Length)
            .Normalized();
        }

        int insertPoint;
        var expanded = GetEngine(solution).ExpandAbbreviation(abbr, GetDocTypeForFile(GetProjectFile(context)), out insertPoint);
        CheckAndIndent(solution, textControl, abbrRange, expanded, insertPoint);

        documentTransactionManager.CommitTransaction();
      }
    }
  }
}
