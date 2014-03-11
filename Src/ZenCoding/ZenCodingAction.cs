/*
 * Copyright 2007-2014 JetBrains
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.Application.CommandProcessing;
using JetBrains.Application.DataContext;
using JetBrains.DocumentModel.Transactions;
using JetBrains.Interop.WinApi;
using JetBrains.ProjectModel;
using JetBrains.TextControl;
using JetBrains.Util;
using JetBrains.ReSharper.Features.Browsing.Bookmarks;

namespace JetBrains.ReSharper.PowerToys.ZenCoding
{
  [ActionHandler("PowerToys.ZenCoding")]
  public class ZenCodingAction : ZenCodingActionBase
  {
    public override void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      var solution = context.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      Assertion.AssertNotNull(solution, "solution == null");
      var textControl = context.GetData(TextControl.DataContext.DataConstants.TEXT_CONTROL);
      Assertion.AssertNotNull(textControl, "textControl == null");

      var commandProcessor = Shell.Instance.GetComponent<ICommandProcessor>();
      var documentTransactionManager = solution.GetComponent<DocumentTransactionManager>();

      using (commandProcessor.UsingCommand("ZenCoding"))
      {
        using (documentTransactionManager.CreateTransactionCookie(DefaultAction.Commit, "ZenCoding"))
        {
          string abbr;
          var abbrRange = textControl.Selection.OneDocRangeWithCaret();
          if (abbrRange.IsValid && abbrRange.Length > 0)
          {
            abbr = textControl.Document.GetText(abbrRange);
          }
          else
          {
            var coords = textControl.Caret.PositionValue.ToDocLineColumn();
            int start;
            var engine = GetEngine(solution);
            var lineText = textControl.Document.GetLineText(coords.Line);
            abbr = engine.FindAbbreviationInLine(lineText, (int) coords.Column, out start);
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
          var projectFile = textControl.GetProjectFile(solution);
          var expanded = GetEngine(solution).ExpandAbbreviation(abbr, GetDocTypeForFile(projectFile), out insertPoint);
          CheckAndIndent(solution, projectFile, textControl, abbrRange, expanded, insertPoint);
        }
      }
    }
  }
}
