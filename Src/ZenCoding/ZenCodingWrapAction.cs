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

using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.Application.CommandProcessing;
using JetBrains.Application.DataContext;
using JetBrains.DataFlow;
using JetBrains.DocumentModel.Transactions;
using JetBrains.IDE;
using JetBrains.Interop.WinApi;
using JetBrains.ProjectModel;
using JetBrains.TextControl;
using JetBrains.Threading;
using JetBrains.UI;
using JetBrains.UI.Icons;
using JetBrains.UI.PopupWindowManager;
using JetBrains.Util;
using DataConstants = JetBrains.UI.DataConstants;
using JetBrains.ReSharper.Features.Browsing.Bookmarks;

namespace JetBrains.ReSharper.PowerToys.ZenCoding
{
  [ActionHandler("PowerToys.ZenCodingWrap")]
  public class ZenCodingWrapAction : ZenCodingActionBase
  {
    public override bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      return context.CheckAllNotNull(DocumentModel.DataContext.DataConstants.DOCUMENT_SELECTION) &&
        base.Update(context, presentation, nextUpdate);
    }

    public override void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      var solution = context.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      if (solution == null)
        return;

      var textControl = context.GetData(TextControl.DataContext.DataConstants.TEXT_CONTROL);
      if (textControl == null)
        return;

      var shellLocks = solution.GetComponent<IShellLocks>();
      var lifetimeDefinition = Lifetimes.Define(EternalLifetime.Instance, "ZenCodingWrap");
      var lifetime = lifetimeDefinition.Lifetime;
      var windowContextSource = context.GetData(DataConstants.PopupWindowContextSource);

      // Layouter
      // Achtung! You MUST either pass the layouter to CreatePopupWindow or dispose of it, don't let it drift off
      IPopupWindowContext ctxToUse;

      if (windowContextSource != null)
      {
        IPopupWindowContext windowContext = windowContextSource.Create(lifetime);
        var ctxTextControl = windowContext as TextControlPopupWindowContext;
        ctxToUse = ctxTextControl == null
                     ? windowContext
                     : ctxTextControl.OverrideLayouter(
                       lifetime,
                       lifetimeLayouter =>
                       new DockingLayouter(lifetimeLayouter,
                                           new TextControlAnchoringRect(
                                             lifetimeLayouter,
                                             ctxTextControl.TextControl,
                                             ctxTextControl.TextControl.Caret.Offset(),
                                             shellLocks),
                                           Anchoring2D.AnchorTopOrBottom));
      }
      else
      {
        ctxToUse = solution.GetComponent<MainWindowPopupWindowContext>().Create(lifetime);
      }

      var form = new ZenCodingWrapForm(lifetime, solution.GetComponent<IThemedIconManager>());

      // Popup support
      var window = solution.GetComponent<PopupWindowManager>().CreatePopupWindow(
        lifetimeDefinition, form, ctxToUse, HideFlags.All & ~HideFlags.Scrolling);
      window.HideMethod = FormHideMethod.Visibility;
      window.Closed +=
        (sender, args) =>
        ReentrancyGuard.Current.ExecuteOrQueue(
          "ZenCodingWrap",
          () =>
            {
              if (form.DialogResult == DialogResult.Cancel)
                return;

              string abbr = form.TextBox.Text.Trim();
              if (abbr.IsEmpty())
              {
                Win32Declarations.MessageBeep(MessageBeepType.Error);
                return;
              }

              var commandProcessor = solution.GetComponent<ICommandProcessor>();
              var documentTransactionManager = solution.GetComponent<DocumentTransactionManager>();

              using (commandProcessor.UsingCommand("ZenCodingWrap"))
              {
                documentTransactionManager.StartTransaction("ZenCodingWrap");

                TextRange selection = textControl.Selection.OneDocRangeWithCaret();
                if (!solution.IsValid())
                  return;

                int insertPoint;
                IProjectFile projectFile = textControl.GetProjectFile(solution);
                string expanded = GetEngine(solution).WrapWithAbbreviation
                  (abbr, textControl.Document.GetText(selection),GetDocTypeForFile(projectFile), out insertPoint);
                CheckAndIndent(solution, projectFile, textControl, selection, expanded, insertPoint);

                documentTransactionManager.CommitTransaction(null);
              }
            });
      window.ShowWindow();
    }
  }
}
