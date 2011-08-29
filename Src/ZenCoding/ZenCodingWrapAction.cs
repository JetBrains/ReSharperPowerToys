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
using JetBrains.UI.PopupWindowManager;
using JetBrains.Util;
using DataConstants = JetBrains.UI.DataConstants;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Features.Browsing.Bookmarks;

namespace JetBrains.ReSharper.PowerToys.ZenCoding
{
  [ActionHandler("PowerToys.ZenCodingWrap")]
  public class PowerToys_ZenCodingWrapAction : ZenCodingActionBase
  {
    public override bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      return context.CheckAllNotNull(IDE.DataConstants.DOCUMENT_SELECTION) &&
        base.Update(context, presentation, nextUpdate);
    }

    public override void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      var solution = context.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      Assertion.AssertNotNull(solution, "solution == null");
      var textControl = context.GetData(TextControl.DataContext.DataConstants.TEXT_CONTROL);
      Assertion.AssertNotNull(textControl, "textControl == null");

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
        ctxToUse = ctxTextControl == null ? windowContext:
          ctxTextControl.OverrideLayouter(lifetime, lifetimeLayouter => new DockingLayouter(lifetimeLayouter, new TextControlAnchoringRect(lifetimeLayouter, ctxTextControl.TextControl, ctxTextControl.TextControl.Caret.Offset(), Shell.Instance.Locks), Anchoring2D.AnchorTopOrBottom));
      }
      else
      {
        ctxToUse = Shell.Instance.GetComponent<MainWindowPopupWindowContext>().Create(lifetime);
      }

      var form = new ZenCodingWrapForm(lifetime);
      // Popup support
      var window = Shell.Instance.GetComponent<PopupWindowManager>().CreatePopupWindow(lifetimeDefinition, form, ctxToUse, HideFlags.All & ~HideFlags.Scrolling);
      window.HideMethod = FormHideMethod.Visibility;
      window.Closed += (sender, args) => ReentrancyGuard.Current.ExecuteOrQueue("ZenCodingWrap", () =>
      {
        try
        {
          if (form.DialogResult == DialogResult.Cancel)
            return;
          var abbr = form.TextBox.Text.Trim();
          if (abbr.IsEmpty())
          {
            Win32Declarations.MessageBeep(MessageBeepType.Error);
            return;
          }

          var commandProcessor = Shell.Instance.GetComponent<ICommandProcessor>();
          var documentTransactionManager = solution.GetComponent<DocumentTransactionManager>();

          using (commandProcessor.UsingCommand("ZenCodingWrap"))
          {
            documentTransactionManager.StartTransaction("ZenCodingWrap");

            var selection = textControl.Selection.OneDocRangeWithCaret();
            Assertion.Assert(selection.IsValid, "selection is not valid");

            int insertPoint;
            var projectFile = textControl.GetProjectFile(solution);
            var expanded = GetEngine(solution)
              .WrapWithAbbreviation(abbr, textControl.Document.GetText(selection), GetDocTypeForFile(projectFile), out insertPoint);
            CheckAndIndent(solution, projectFile, textControl, selection, expanded, insertPoint);
            
            documentTransactionManager.CommitTransaction(null);
          }
        }
        finally
        {
          lifetimeDefinition.Terminate();
        }
      });
      lifetime.AddBracket(() => window.ShowWindow(), window.HideWindow);
      lifetime.AddDispose(window);
      lifetime.AddDispose(form);
    }
  }
}
