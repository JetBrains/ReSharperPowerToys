using JetBrains.Application;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.PsiPlugin.Feature.Services.Bulbs;
using JetBrains.TextControl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.ResearchFormatter.Psi
{
  //[ContextAction(Group = "PSI", Name = "formatPSI", Description = "format psi.", Priority = -1)]
  public class PsiFormattingAction : IContextAction, IBulbAction
  {
    private readonly PsiContextActionDataProvider myProvider;

    #region Overrides of ContextActionBase

    public PsiFormattingAction(PsiContextActionDataProvider provider)
    {
      myProvider = provider;
    }

    public void CreateBulbItems(BulbMenu menu)
    {
      menu.ArrangeContextAction(this);
    }

    public bool IsAvailable(IUserDataHolder cache)
    {
      //todo
      return myProvider.Selection.Length > 0;
    }

    #endregion

    #region Implementation of IBulbAction

    public void Execute(ISolution solution, ITextControl textControl)
    {
      var formatter = PsiResearchFormatter.Instance;
      var startOffset = myProvider.Selection.StartOffset;
      var endOffset = myProvider.Selection.EndOffset;
      var nodeFirst = myProvider.PsiFile.FindTokenAt(new TreeOffset(startOffset));
      var nodeLast = myProvider.PsiFile.FindTokenAt(new TreeOffset(endOffset - 1));
      var psiServices = myProvider.PsiServices;
      using (new DisableCodeFormatter())
      {
        using (PsiTransactionCookie.CreateAutoCommitCookieWithCachesUpdate(psiServices, "Format code"))
        {
          using (WriteLockCookie.Create())
          {
            formatter.Format(nodeFirst, nodeLast, CodeFormatProfile.DEFAULT);
          }
        }
      }
    }

    public string Text { get { return "format psi"; } }

    #endregion
  }
}
