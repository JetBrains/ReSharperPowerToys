using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve.Managed;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Resolve.Managed;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.DaemonStage
{
  [DaemonStage(StagesBefore = new[] { typeof(GlobalFileStructureCollectorStage) }, StagesAfter = new[] { typeof(IdentifierHighlightingStage) })]
  public class SmartResolverStage : PsiDaemonStageBase
  {
    public override ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
    {
      return ErrorStripeRequest.NONE;
    }

    public override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
    {
      if (!IsSupported(process.SourceFile))
        return null;
      return new SmartResolverProcess(process);
    }
  }

  public class SmartResolverProcess : IDaemonStageProcess
  {
    private readonly IDaemonProcess myDaemonProcess;
    //private readonly HashSet<IUsingDirective> myUsedUsings = new HashSet<IUsingDirective>();
    private readonly Dictionary<IReference, IResolveResult> myUnqualifiedResolve = new Dictionary<IReference, IResolveResult>();
    private readonly Dictionary<IScope, bool> myVarTypeVisibility = new Dictionary<IScope, bool>();
    private readonly OneToSetMap<ITypeParameter, ITypeElement> myHiddenByTypeParameter = new OneToSetMap<ITypeParameter, ITypeElement>();

    public SmartResolverProcess(IDaemonProcess daemonProcess)
    {
      myDaemonProcess = daemonProcess;
    }

    public IDaemonProcess DaemonProcess
    {
      get { return myDaemonProcess; }
    }

    public void Execute(Action<DaemonStageResult> commiter)
    {
      var file = PsiDaemonStageBase.GetPsiFile(myDaemonProcess.SourceFile);
      if (file == null)
        return;

      var fileStructureCollector = DaemonProcess.GetStageProcess<GlobalFileStructureCollectorStage.Process>().Get<PsiFileStructure>();
      Assertion.Assert(fileStructureCollector != null, "fileStructureCollector != null");

      var allContexts = new Dictionary<IScope, ScopeContext>();
      Action<IScope, ScopeContext> scopeAction = null;

      using (var fibers = DaemonProcess.CreateFibers())
      {
        scopeAction = (scope, context) =>
        {
          lock (allContexts)
            allContexts.Add(scope, context);
          fibers.EnqueueJob(() => new ScopeResolver(DaemonProcess, fileStructureCollector, scope, context, scopeAction).Process(scope));
        };

        var initialContext = new ScopeContext(EmptySymbolTable.INSTANCE, 0, new ElementAccessContext(file), myDaemonProcess.FullRehighlightingRequired);
        //scopeAction((IScope)file, initialContext);
      }

      MergeContextResults(allContexts);
    }

    private void MergeContextResults(IEnumerable<KeyValuePair<IScope, ScopeContext>> allContexts)
    {
      foreach (var context in allContexts)
      {
        //myUsedUsings.UnionWith(context.Value.UsedUsings);
        myVarTypeVisibility.Add(context.Key, context.Value.VarTypeVisibility);
        foreach (var pair in context.Value.UnqualifiedResolve)
          myUnqualifiedResolve.Add(pair.Key, pair.Value);
        foreach (var pair in context.Value.HiddenByTypeParameter)
          myHiddenByTypeParameter.AddRange(pair.Key, pair.Value);
      }
    }

    private class ScopeContext
    {
      public readonly ISymbolTable SymbolTable;
      public readonly int Level;
      public readonly IAccessContext AccessContext;
      public readonly bool CheckRedundantQualifiers;

      //private readonly HashSet<IUsingDirective> myUsedUsings = new HashSet<IUsingDirective>();
      private readonly Dictionary<IReference, IResolveResult> myUnqualifiedResolve = new Dictionary<IReference, IResolveResult>();
      private readonly Dictionary<ITypeParameter, IList<ITypeElement>> myHiddenByTypeParameter = new Dictionary<ITypeParameter, IList<ITypeElement>>();

      public ScopeContext(ISymbolTable symbolTable, int level, IAccessContext accessContext, bool checkRedundantQualifiers)
      {
        SymbolTable = symbolTable;
        Level = level;
        AccessContext = accessContext;
        CheckRedundantQualifiers = checkRedundantQualifiers;
      }

      public Dictionary<ITypeParameter, IList<ITypeElement>> HiddenByTypeParameter
      {
        get { return myHiddenByTypeParameter; }
      }

      public bool VarTypeVisibility { get; set; }

      public Dictionary<IReference, IResolveResult> UnqualifiedResolve
      {
        get { return myUnqualifiedResolve; }
      }

      /*public HashSet<IUsingDirective> UsedUsings
      {
        get { return myUsedUsings; }
      }*/
    }

    #region ReferenceResolver

    private class ScopeResolver : NonQualifiedReferencesResolveBase
    {
      private readonly IDaemonProcess myDaemonProcess;
      private readonly PsiFileStructure myFileStructure;
      private readonly IScope myScope;
      private readonly ScopeContext myContext;
      private readonly Action<IScope, ScopeContext> myScopeProcessor;

      /// <summary>
      /// Indicates wether resolve for redundant qualifier check should be done
      /// If daemon incrementally rehighlights method body, then only this body should be checked
      /// </summary>
      private bool myCheckRedundantQualifiers;

      public ScopeResolver(IDaemonProcess daemonProcess, PsiFileStructure fileStructure, IScope scope, ScopeContext context, Action<IScope, ScopeContext> scopeProcessor)
        : base(SymbolTableMode.FULL)
      {
        myResolveContext = new UniversalContext(scope.GetPsiModule());

        myDaemonProcess = daemonProcess;
        myFileStructure = fileStructure;
        myScope = scope;
        myContext = context;
        myScopeProcessor = scopeProcessor;
        myCheckRedundantQualifiers = myContext.CheckRedundantQualifiers;
      }

      protected override bool ScopeShouldBeVisited(IScope scope)
      {
        return scope == myScope || !ForkOnScope(scope);
      }

      private bool ForkOnScope(IScope scope)
      {
        //return PsiFunctionDeclarationNavigator.GetByBody(scope as IBlock) != null;
        throw new NotImplementedException();
      }

      protected override Pair<ISymbolTable, int> GetInitialSymbolTable(SymbolTableMode mode)
      {
        return new Pair<ISymbolTable, int>(myContext.SymbolTable, myContext.Level);
      }

      protected override IAccessContext GetInitialAccessContext()
      {
        return myContext.AccessContext;
      }

      public override void ProcessBeforeInterior(ITreeNode element)
      {
        base.ProcessBeforeInterior(element);

        var memberDeclaration = element as IPsiTypeMemberDeclaration;
        if (memberDeclaration != null && !myContext.CheckRedundantQualifiers)
        {
          Assertion.Assert(!myCheckRedundantQualifiers, "!myCheckRedundantQualifiers");
          if (myFileStructure.MembersToRehighlight.Contains(memberDeclaration))
            myCheckRedundantQualifiers = true;
        }
      }

      public override void ProcessAfterInterior(ITreeNode element)
      {
        base.ProcessAfterInterior(element);

        var typeMemberDeclaration = element as IPsiTypeMemberDeclaration;
        if (typeMemberDeclaration != null && !myContext.CheckRedundantQualifiers)
          myCheckRedundantQualifiers = false;
      }

      protected override void VisitElement(ITreeNode element)
      {
        foreach (var reference in element.GetFirstClassReferences())
        {
          ResolveResultWithInfo resolveResult;

          var qualifiableReference = reference as IQualifiableReference;
          if (qualifiableReference == null || qualifiableReference.IsQualified)
          {
            resolveResult = reference.ResolveWithInfo(myResolveContext);
            if (reference is IManagedReference)
              reference.CurrentResolveResult = resolveResult;
          }
          else
          {
            resolveResult = ResolveNonqualifiedQualifiableReference(myResolveContext, qualifiableReference, SymbolTable, AccessContext);

            // Check for redundant qualifiers. Do it only for references in changed range
            /*if (myCheckRedundantQualifiers)
            {
              var referenceExpression = element as IReferenceExpression;
              if (referenceExpression != null)
                CheckRedundantQualifierForReferenceExpression(referenceExpression);

              var referenceName = element as IReferenceName;
              if (referenceName != null)
                CheckRedundantQualifierForReferenceName(referenceName);
            }*/
          }

          //var infoWithUsings = resolveResult.Info as IResolveInfoWithUsings;
          //if ( /*resolveResult.DeclaredElement != null && */infoWithUsings != null)
            //myContext.UsedUsings.AddRange(infoWithUsings.UsingDirectives);
        }

        if (myCheckRedundantQualifiers)
        {
          /*if ((element is IThisExpression) ||
              (element is IPredefinedTypeExpression) ||
              (element is IBaseExpression))
            CheckRedundantQualifierForReferenceExpression((IPsiExpression)element);*/
        }

        var scope = element as IScope;
        if (scope != null)
        {
          if (scope == myScope)
            myContext.VarTypeVisibility = IsVarTypeVisible();
          else if (ForkOnScope(scope))
            myScopeProcessor(scope, new ScopeContext(SymbolTable, Level, AccessContext, myCheckRedundantQualifiers));
        }

        //var typeParameterDeclaration = element as ITypeParameterDeclaration;
        /*if (typeParameterDeclaration != null)
        {
          var typeParameter = typeParameterDeclaration.DeclaredElement;
          if (typeParameter != null)
            myContext.HiddenByTypeParameter[typeParameter] = GetHiddenByTypeParameter(typeParameter);
        }*/
      }

      /*private IList<ITypeElement> GetHiddenByTypeParameter(ITypeParameter typeParameter)
      {
        var result = new LocalList<ITypeElement>();
        foreach (var info in mySymbolTable.GetAllSymbolInfos(typeParameter.ShortName))
        {
          var typeElement = info.GetDeclaredElement() as ITypeElement;
          if (typeElement != null && !Equals(typeElement, typeParameter))
          {
            if (!(typeElement is ITypeMember) || AccessUtil.IsSymbolAccessible((ITypeMember)typeElement, AccessContext))
              result.Add(typeElement);
          }
        }

        return result.ResultingList();
      }*/

      private bool IsVarTypeVisible()
      {
        // ReSharper disable LoopCanBeConvertedToQuery
        /*foreach (var info in mySymbolTable.GetAllSymbolInfos("var"))
        {
          var typeElement = info.GetDeclaredElement() as ITypeElement;
          if (typeElement != null && typeElement.TypeParameters.Count == 0)
            return true;
        }*/

        return false;
        // ReSharper restore LoopCanBeConvertedToQuery
      }

      /*private void CheckRedundantQualifierForReferenceExpression(IPsiExpression qualifier)
      {
        if (!IsPossiblyRedundantQualifierExpression(qualifier))
          return;

        // move up
        bool stopResolve = false;
        if (qualifier is IReferenceExpression)
          stopResolve = ShouldStopResolve(((IReferenceExpression)qualifier).Reference.Resolve().Result);
        var referenceExpression = ReferenceExpressionNavigator.GetByQualifierExpression(qualifier);

        while (referenceExpression != null)
        {
          if (ProcessingIsFinished)
            break;

          if (!stopResolve)
          {
            IReferenceExpression referenceExpressionNode = referenceExpression;
            if (referenceExpressionNode.NameIdentifier == null)
              goto Next;
            var referenceExpressionReference = referenceExpression.Reference;

            var result = referenceExpressionReference.ResolveAsUnqualified(new ResolveContext(referenceExpressionNode.GetPsiModule()), mySymbolTable);
            if (result.DeclaredElement != null && result.ResolveErrorType == ResolveErrorType.OK)
            {
              myContext.UnqualifiedResolve[referenceExpression.Reference] = result.Result;
              stopResolve = ShouldStopResolve(result.Result);
            }
            else
            {
              myContext.UnqualifiedResolve[referenceExpression.Reference] = null;
            }
          }
          else
          {
            myContext.UnqualifiedResolve[referenceExpression.Reference] = null;
          }

        Next:
          referenceExpression = ReferenceExpressionNavigator.GetByQualifierExpression(referenceExpression);
        }
      }*/

      /*private static bool ShouldStopResolve(IResolveResult resolveResult)
      {
        var declaredElement = resolveResult.DeclaredElement;
        return !(declaredElement is INamespace || declaredElement is ITypeElement || declaredElement is IExternAlias);
      }*/

      /*private bool IsPossiblyRedundantQualifierExpression(IPsiExpression qualifier)
      {
        var referenceExpression = qualifier as IReferenceExpression;
        if (referenceExpression != null)
        {
          if (referenceExpression.QualifierExpression != null)
            return false;

          if (!referenceExpression.Reference.QualifyingAliasName(myResolveContext).IsGlobalAlias())
          {
            IDeclaredElement resolveResult = referenceExpression.Reference.Resolve().DeclaredElement;
            if (resolveResult is IExternAlias)
              return false;
          }

          return true;
        }

        return (qualifier is IThisExpression) ||
               (qualifier is IPredefinedTypeExpression) ||
               (qualifier is IBaseExpression);
      }*/

      /*private void CheckRedundantQualifierForReferenceName(IReferenceName qualifier)
      {
        Assertion.Assert(qualifier.Qualifier == null, "qualifier.Qualifier == null");

        // Check for extern alias problems
        if (!qualifier.Reference.GetExternAliasName().IsGlobalAlias())
        {
          IDeclaredElement resolveResult = qualifier.Reference.Resolve().DeclaredElement;
          if (resolveResult is IExternAlias)
            return;
        }

        // move up
        var referenceName = ReferenceNameNavigator.GetByQualifier(qualifier);
        while (referenceName != null)
        {
          if (ProcessingIsFinished)
            break;

          if (referenceName.NameIdentifier != null)
          {
            var result = referenceName.Reference.ResolveAsUnqualified(mySymbolTable);
            myContext.UnqualifiedResolve[referenceName.Reference] = result.ResolveErrorType == ResolveErrorType.OK ? result.Result : null;
          }

          referenceName = ReferenceNameNavigator.GetByQualifier(referenceName);
        }
      }*/
    }

    #endregion

    /*public bool IsUsingDirectiveUsed(IUsingDirective directive)
    {
      return myUsedUsings.Contains(directive);
    }*/

    public bool IsVarTypeVisibleInScope(IScope scope)
    {
      bool value;
      return myVarTypeVisibility.TryGetValue(scope, out value) && value;
    }

    public IEnumerable<ITypeElement> GetTypesHiddenByTypeParameter(ITypeParameter typeParameter)
    {
      return myHiddenByTypeParameter[typeParameter];
    }

    public IResolveResult GetUnqualifiedResolve(IReference reference)
    {
      IResolveResult value;
      if (myUnqualifiedResolve.TryGetValue(reference, out value))
        return value;
      return null;
    }
  }
}
