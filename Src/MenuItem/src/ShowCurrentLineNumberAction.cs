/*
 * Copyright 2007-2011 JetBrains
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
using JetBrains.Application.DataContext;
using JetBrains.TextControl;
using JetBrains.TextControl.Coords;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.MenuItem
{
  [ActionHandler("AddMenuItem.ShowCurrentLineNumber")]
  internal class ShowCurrentLineNumberAction : IActionHandler
  {
    #region IActionHandler Members

    bool IActionHandler.Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      // fetch focused text editor control
      ITextControl textControl = context.GetData(TextControl.DataContext.DataConstants.TEXT_CONTROL);

      // enable this action if we are in text editor, disable otherwise
      return textControl != null;
    }

    void IActionHandler.Execute(IDataContext context, DelegateExecute nextExecute)
    {
      ITextControl textControl = context.GetData(TextControl.DataContext.DataConstants.TEXT_CONTROL);
      if(textControl == null)
      {
        MessageBox.ShowError("Text control unavailable."); // Note: shouldn't actually get here due to Update() handling
        return;
      }

      // Fetch caret line number
      ITextControlPos caretOffset = textControl.Caret.Position.Value;
      var nTextControlLine = (int)caretOffset.ToTextControlLineColumn().Line;
      var nDocLine = (int)caretOffset.ToDocLineColumn().Line;

      // Note that we increment line number by one because "line number" in our API starts from zero
      string message;
      if(nTextControlLine == nDocLine)
        message = string.Format("Current line number is {0:N0}.", nTextControlLine + 1);
      else
        message = string.Format("Current text control line number is {0:N0}.\nCurrent document line number is {1:N0}.\n\nProbably, you have some hidden regions in the text " +
                                "editor, or Word Wrapping turned on. Hence the difference in line numbers.", nTextControlLine, nDocLine);
      MessageBox.ShowInfo(message, "AddMenuItem Sample Plugin");
    }

    #endregion
  }
}