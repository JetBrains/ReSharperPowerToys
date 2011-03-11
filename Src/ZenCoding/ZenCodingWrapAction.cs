using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.DataFlow;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel;
using JetBrains.DocumentModel.Transactions;
using JetBrains.IDE;
using JetBrains.Interop.WinApi;
using JetBrains.TextControl;
using JetBrains.Threading;
using JetBrains.UI;
using JetBrains.UI.Interop;
using JetBrains.UI.PopupWindowManager;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ZenCoding
{
  [ActionHandler]
  public class PowerToys_ZenCodingWrapAction : ZenCodingActionBase
  {
    private Lifetime lifetime;
    private readonly DocumentTransactionManager documentTransactionManager;

    public PowerToys_ZenCodingWrapAction(Lifetime lifetime, DocumentTransactionManager documentTransactionManager)
    {
      this.lifetime = lifetime;
      this.documentTransactionManager = documentTransactionManager;
    }

    public override bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      return context.CheckAllNotNull(IDE.DataConstants.DOCUMENT_SELECTION) &&
        base.Update(context, presentation, nextUpdate);
    }

    public override void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      Lifetimes.Using(lifetime =>
      {
        
      });

      var solution = context.GetData(IDE.DataConstants.SOLUTION);
      Assertion.AssertNotNull(solution, "solution == null");
      var textControl = context.GetData(IDE.DataConstants.TEXT_CONTROL);
      Assertion.AssertNotNull(textControl, "textControl == null");

      var windowContext = context.GetData(UI.DataConstants.POPUP_WINDOW_CONTEXT);

      // Layouter
      // Achtung! You MUST either pass the layouter to CreatePopupWindow or dispose of it, don't let it drift off
      IPopupWindowContext ctxToUse;
      IPopupLayouter layouterToUse;

      if (windowContext != null)
      {
        var ctxTextControl = windowContext as TextControlPopupWindowContext;
        if (ctxTextControl != null)
        {
          layouterToUse = new DockingLayouter(
            new TextControlAnchoringRect(ctxTextControl.TextControl, ctxTextControl.TextControl.Caret.Offset()), Anchoring2D.AnchorLeftOrRightOnly);
          ctxToUse = ctxTextControl;
        }
        else
        {
          layouterToUse = windowContext.CreateLayouter();
          ctxToUse = windowContext;
        }
      }
      else
      {
        ctxToUse = PopupWindowContext.Empty;
        layouterToUse = ctxToUse.CreateLayouter(lifetime);
      }

      var form = new ZenCodingWrapForm(lifetime);
      var window = PopupWindowManager.CreatePopupWindow(form, layouterToUse, ctxToUse, HideFlags.Escape, true);
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
          using (ReadLockCookie.Create())
          using (CommandCookie.Create("ZenCodingWrap"))
          {
            // use transaction manager?
            using (var cookie = documentTransactionManager.EnsureWritable(textControl.Document))
            {
              if (cookie.EnsureWritableResult != EnsureWritableResult.SUCCESS)
                return;

              
              var selection = textControl.Selection.DocRange;
              Assertion.Assert(selection.IsValid, "selection is not valid");

              int insertPoint;
              var expanded = GetEngine(solution).WrapWithAbbreviation(
                abbr, textControl.Selection.GetSelectionText(), GetDocTypeForFile(GetProjectFile(context)), out insertPoint);
              CheckAndIndent(solution, textControl, selection, expanded, insertPoint);
            }
          }
        }
        finally
        {
          window.Dispose();
          form.Dispose();
        }
      });
      window.ShowWindow();
    }
  }
}
