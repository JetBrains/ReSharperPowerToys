using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.Application.CommandProcessing;
using JetBrains.DocumentManagers;
using JetBrains.DocumentManagers.Transactions;
using JetBrains.DocumentManagers.impl;
using JetBrains.DocumentModel;
using JetBrains.DocumentModel.Transactions;
using JetBrains.Interop.WinApi;
using JetBrains.ProjectModel;
using JetBrains.TextControl;
using JetBrains.UI.Interop;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ZenCoding
{
  [ActionHandler("PowerToys.ZenCoding")]
  public class PowerToys_ZenCodingAction : ZenCodingActionBase
  {
    public override void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      var solution = context.GetData(IDE.DataConstants.SOLUTION);
      Assertion.AssertNotNull(solution, "solution == null");
      var textControl = context.GetData(IDE.DataConstants.TEXT_CONTROL);
      Assertion.AssertNotNull(textControl, "textControl == null");

      var commandProcessor = Shell.Instance.GetComponent<ICommandProcessor>();
      var documentTransactionManager = solution.GetComponent<DocumentTransactionManager>();

      using (commandProcessor.UsingCommand("ZenCoding"))
      {
        documentTransactionManager.StartTransaction("ZenCoding");

        string abbr;

        TextRange abbrRange = textControl.Selection.UnionOfDocRanges();
        // todo: selected text
        /*
        if (abbrRange.IsValid && abbrRange.Length > 0)
        {
          abbr = textControl.Document.GetText(abbrRange);
        }
        else
        { */
          var coords = textControl.Caret.PositionValue.ToDocLineColumn();
          int start;
          var engine = GetEngine(solution);
          var lineText = textControl.Document.GetLineText(coords.Line);
          abbr = engine.FindAbbreviationInLine(lineText, (int)coords.Column, out start);
          if (start == -1)
          {
            Win32Declarations.MessageBeep(MessageBeepType.Error);
            return;
          }
          abbrRange = TextRange
            .FromLength(textControl.Caret.PositionValue.ToDocOffset(), -abbr.Length)
            .Normalized();
        //}

        int insertPoint;
        var expanded = GetEngine(solution).ExpandAbbreviation(abbr, GetDocTypeForFile(GetProjectFile(context)), out insertPoint);
        CheckAndIndent(solution, textControl, abbrRange, expanded, insertPoint);

        documentTransactionManager.CommitTransaction();
      }
    }
  }
}
