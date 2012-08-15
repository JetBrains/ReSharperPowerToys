using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.DocumentManagers.Transactions;
using JetBrains.DocumentModel;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Intentions.Impl.LanguageSpecific;
using JetBrains.ReSharper.Feature.Services.Intentions.Impl.TemplateFieldHolders;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.LiveTemplates;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Intentions.CreateFromUsage
{
  public class PsiRuleBuilder
  {
    public static PsiIntentionResult Create(CreatePsiRuleContext context)
    {
      var declaration = context.Declaration;

      declaration = PsiIntentionsUtil.AddToTarget(declaration, context.Target);


      var holders = new List<ITemplateFieldHolder>();

      return new PsiIntentionResult(holders, declaration, context.Anchor, new DocumentRange(context.Document, context.Offset));
    }
  }

  public class PsiIntentionResult
  {
    private IDeclaration myDeclaration;
    private List<ITemplateFieldHolder> myHolders;
    private DocumentRange myPrefferedSelection;
    private ITreeNode myAnchor;

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

    public IList<ITemplateFieldHolder> Holders
    {
      get { return myHolders; }
    }
 
    public DocumentRange PrefferedSelection
    {
      get
      {
        return myPrefferedSelection == null 
          ? DocumentRange.InvalidRange 
          : myPrefferedSelection;
      }
    }

    public void ExecuteTemplate()
    {
      IDeclaration newDeclaration = myDeclaration;
      newDeclaration.AssertIsValid();

      ISolution solution = newDeclaration.GetPsiModule().GetSolution();

      Shell.Instance.Invocator.Dispatcher.AssertAccess();

      Assertion.Assert(!PsiManager.GetInstance(solution).HasActiveTransaction, "PSI transaction is active");
      solution.GetComponent<SolutionDocumentTransactionManager>().AssertNotUnderTransaction();

      IFile file = myAnchor.GetContainingFile();
      Assertion.Assert(file != null, "file!= null");
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

  [ShellComponent]
  public class PsiIntentionResultBehavior
  {
    protected HotspotSessionExecutor HotspotSessionExecutor { get; private set; }

    public PsiIntentionResultBehavior(HotspotSessionExecutor hotspotSessionExecutor)
    {
      HotspotSessionExecutor = hotspotSessionExecutor;
    }

    public void OnHotspotSessionExecutionStarted(PsiIntentionResult result, ITextControl textControl)
    {
      OnHotspotSessionExecutionStartedInternal(result, textControl);
    }

    protected virtual void OnHotspotSessionExecutionStartedInternal(PsiIntentionResult result, ITextControl textControl)
    {
      var hotspotSessionUi = HotspotSessionExecutor.CurrentSession;
      if (hotspotSessionUi == null)
        SetCaretPosition(textControl, result);
      else
        hotspotSessionUi.HotspotSession.Closed += (session, type) =>
        {
          if (type != TerminationType.Finished) return;
          SetCaretPosition(textControl, result);
        };
    }

    protected static void SetCaretPosition(ITextControl textControl, PsiIntentionResult result)
    {
      if (result.PrefferedSelection != DocumentRange.InvalidRange)
      {
        textControl.Selection.SetRange(result.PrefferedSelection.TextRange);
      }
    }
  }

}