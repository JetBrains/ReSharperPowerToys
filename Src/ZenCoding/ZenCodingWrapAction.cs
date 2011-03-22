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
using DataConstants = JetBrains.UI.DataConstants;

namespace JetBrains.ReSharper.PowerToys.ZenCoding
{
  //[ActionHandler("PowerToys.ZenCodingWrap")]
  public class PowerToys_ZenCodingWrapAction : ZenCodingActionBase
  {
    private Lifetime lifetime;
    private readonly DocumentTransactionManager documentTransactionManager;
    private IShellLocks locks;

    public PowerToys_ZenCodingWrapAction(Lifetime lifetime, DocumentTransactionManager documentTransactionManager)
    {
      this.lifetime = lifetime;
      this.documentTransactionManager = documentTransactionManager;
      this.locks = Shell.Instance.GetComponent<IShellLocks>();
    }

    public override bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      return context.CheckAllNotNull(IDE.DataConstants.DOCUMENT_SELECTION) &&
        base.Update(context, presentation, nextUpdate);
    }

    public override void Execute(IDataContext context, DelegateExecute nextExecute)
    {
    //  var solution = context.GetData(IDE.DataConstants.SOLUTION);
    //  Assertion.AssertNotNull(solution, "solution == null");
    //  var textControl = context.GetData(IDE.DataConstants.TEXT_CONTROL);
    //  Assertion.AssertNotNull(textControl, "textControl == null");

      var windowContext = context.GetData(DataConstants.PopupWindowContextSource);

    //  // Layouter
    //  // Achtung! You MUST either pass the layouter to CreatePopupWindow or dispose of it, don't let it drift off
    //  IPopupWindowContext ctxToUse;
    //  IPopupLayouter layouterToUse;

    //  if (windowContext != null)
    //  {
    //    var ctxTextControl = windowContext as TextControlPopupWindowContext;
    //    if (ctxTextControl != null)
    //    {
    //      layouterToUse = new DockingLayouter(lifetime, 
    //        new TextControlAnchoringRect(lifetime, ctxTextControl.TextControl, ctxTextControl.TextControl.Caret.Offset(), locks), Anchoring2D.AnchorLeftOrRightOnly);
    //      ctxToUse = ctxTextControl;
    //    }
    //    else
    //    {
    //      layouterToUse = windowContext.CreateLayouter();
    //      ctxToUse = windowContext;
    //    }
    //  }
    //  else
    //  {
    //    ctxToUse = PopupWindowContext.Empty;
    //    layouterToUse = ctxToUse.CreateLayouter(lifetime);
    //  }

    //  var form = new ZenCodingWrapForm(lifetime);
    //  var window = PopupWindowManager.CreatePopupWindow(form, layouterToUse, ctxToUse, HideFlags.Escape, true);
    //  window.Closed += (sender, args) => ReentrancyGuard.Current.ExecuteOrQueue("ZenCodingWrap", () =>
    //  {
    //    try
    //    {
    //      if (form.DialogResult == DialogResult.Cancel)
    //        return;
    //      var abbr = form.TextBox.Text.Trim();
    //      if (abbr.IsEmpty())
    //      {
    //        Win32Declarations.MessageBeep(MessageBeepType.Error);
    //        return;
    //      }
    //      using (ReadLockCookie.Create())
    //      using (CommandCookie.Create("ZenCodingWrap"))
    //      {
    //        // use transaction manager?
    //        using (var cookie = documentTransactionManager.EnsureWritable(textControl.Document))
    //        {
    //          if (cookie.EnsureWritableResult != EnsureWritableResult.SUCCESS)
    //            return;

              
    //          var selection = textControl.Selection.UnionOfDocRanges();
    //          Assertion.Assert(selection.IsValid, "selection is not valid");

    //          int insertPoint;
    //          var expanded = GetEngine(solution).WrapWithAbbreviation(
    //            abbr, string.Join("", textControl.Selection.GetSelectedText().ToArray()), GetDocTypeForFile(GetProjectFile(context)), out insertPoint);
    //          CheckAndIndent(solution, textControl, selection, expanded, insertPoint);
    //        }
    //      }
    //    }
    //    finally
    //    {
    //      window.Dispose();
    //      form.Dispose();
    //    }
    //  });
    //  window.ShowWindow();
    }
  }
}
