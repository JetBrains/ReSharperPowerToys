using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Application;
using JetBrains.DocumentManagers.Transactions;
using JetBrains.DocumentModel;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Intentions.Impl.TemplateFieldHolders;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.LiveTemplates;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Intentions.CreateFromUsage
{
  public class PsiIntentionResult
  {
    private readonly IDeclaration myDeclaration;
    private readonly List<ITemplateFieldHolder> myHolders;
    private readonly DocumentRange myPrefferedSelection;
    private readonly ITreeNode myAnchor;

    public PsiIntentionResult(List<ITemplateFieldHolder> holders, IDeclaration declaration, ITreeNode anchor, DocumentRange range)
    {
      myDeclaration = declaration;
      myHolders = holders;
      myPrefferedSelection = range;
      myAnchor = anchor;
    }

    public IDeclaration ResultDeclaration
    {
      get { return myDeclaration; }
    }

    public DocumentRange PrefferedSelection
    {
      get
      {
        return myPrefferedSelection;
      }
    }

    public void ExecuteTemplate()
    {
      IDeclaration newDeclaration = myDeclaration;
      newDeclaration.AssertIsValid();

      ISolution solution = newDeclaration.GetPsiModule().GetSolution();

      Debug.Assert(Shell.Instance.Invocator != null, "Shell.Instance.Invocator != null");
      Shell.Instance.Invocator.Dispatcher.AssertAccess();

      Assertion.Assert(!PsiManager.GetInstance(solution).HasActiveTransaction, "PSI transaction is active");
      solution.GetComponent<SolutionDocumentTransactionManager>().AssertNotUnderTransaction();

      IFile file = myAnchor.GetContainingFile();
      Assertion.Assert(file != null, "fileFullName!= null");
      var item = file.GetSourceFile().ToProjectFile();

      var infos = GetFieldInfos(newDeclaration, myHolders);

      var textControl = EditorManager.GetInstance(solution).OpenProjectFile(item, true, TabOptions.DefaultTab);
      if (textControl == null)
      {
        if (Shell.Instance.IsInInternalMode || Shell.Instance.IsTestShell) Logger.Fail("textControl != null");
        return;
      }

      if (infos.Length > 0)
      {
        HotspotSession hotspotSession = LiveTemplatesManager.Instance.CreateHotspotSessionAtopExistingText(solution,
          TextRange.InvalidRange, textControl, LiveTemplatesManager.EscapeAction.LeaveTextAndCaret, infos);
        hotspotSession.Execute();
      }

      Shell.Instance.GetComponent<PsiIntentionResultBehavior>().OnHotspotSessionExecutionStarted(this, textControl);
    }

    private static HotspotInfo[] GetFieldInfos(IDeclaration declaration, IEnumerable<ITemplateFieldHolder> templateArguments)
    {
      return
        templateArguments.Select(t => t.GetInfo(declaration)).Where(hotspotInfo => hotspotInfo.Ranges.Any()).ToArray();
    }
  }
}