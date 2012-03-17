using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve.Managed;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Resolve.Managed;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections
{
  public class SmartResolverProcess : IDaemonStageProcess
  {
    private readonly IDaemonProcess myDaemonProcess;
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
                          fibers.EnqueueJob(() => new ScopeResolver(fileStructureCollector, scope, context).Process(scope));
                        };
      }

      MergeContextResults(allContexts);
    }

    private void MergeContextResults(IEnumerable<KeyValuePair<IScope, ScopeContext>> allContexts)
    {
      foreach (var context in allContexts)
      {
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
    }

    #region ReferenceResolver

    private class ScopeResolver : NonQualifiedReferencesResolveBase
    {
      private readonly PsiFileStructure myFileStructure;
      private readonly IScope myScope;
      private readonly ScopeContext myContext;

      /// <summary>
      /// Indicates wether resolve for redundant qualifier check should be done
      /// If daemon incrementally rehighlights method body, then only this body should be checked
      /// </summary>
      private bool myCheckRedundantQualifiers;

      public ScopeResolver(PsiFileStructure fileStructure, IScope scope, ScopeContext context)
        : base(SymbolTableMode.FULL)
      {
        myResolveContext = new UniversalContext(scope.GetPsiModule());

        myFileStructure = fileStructure;
        myScope = scope;
        myContext = context;
        myCheckRedundantQualifiers = myContext.CheckRedundantQualifiers;
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
          var qualifiableReference = reference as IQualifiableReference;
          if (qualifiableReference == null || qualifiableReference.IsQualified)
          {
            ResolveResultWithInfo resolveResult = reference.ResolveWithInfo(myResolveContext);
            if (reference is IManagedReference)
              reference.CurrentResolveResult = resolveResult;
          }
        }

        var scope = element as IScope;
        if (scope != null)
        {
          if (scope == myScope)
            myContext.VarTypeVisibility = IsVarTypeVisible();
        }

      }

      private bool IsVarTypeVisible()
      {
        return false;
      }
    }

    #endregion
  }
}