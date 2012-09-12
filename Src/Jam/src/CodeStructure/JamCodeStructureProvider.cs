using System.Collections.Generic;
using JetBrains.Application;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeStructure;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using System.Linq;

namespace JetBrains.ReSharper.Psi.Jam.CodeStructure
{
  [Language(typeof (JamLanguage))]
  public class JamCodeStructureProvider : IPsiFileCodeStructureProvider
  {
    private readonly IShellLocks myLocks;

    public JamCodeStructureProvider(IShellLocks locks)
    {
      myLocks = locks;
    }

    public CodeStructureRootElement Build(IFile file, CodeStructureOptions options)
    {
      myLocks.AssertReadAccessAllowed();
      file.GetPsiServices().PsiManager.AssertAllDocumentAreCommited();

      var jamFile = file as IJamFile;
      if (jamFile == null) return null;

      var solution = jamFile.GetSolution();

      var rootElement = new JamCodeStructureRootElement(jamFile);
      jamFile.ProcessDescendants(new Visitor(solution.GetComponent<PsiIconManager>()), rootElement);
      return rootElement;
    }

    private class Visitor : TreeNodeVisitor<CodeStructureElement, CodeStructureElement>, IRecursiveElementProcessor<CodeStructureElement>
    {
      private readonly PsiIconManager myPsiIconManager;
      private readonly Stack<JetTuple<ITreeNode, CodeStructureElement>> myRoots = new Stack<JetTuple<ITreeNode, CodeStructureElement>>();

      public Visitor(PsiIconManager psiIconManager)
      {
        myPsiIconManager = psiIconManager;
      }

      #region Visitor methods

      public bool IsProcessingFinished(CodeStructureElement context)
      {
        InterruptableActivityCookie.CheckAndThrow();
        return false;
      }

      public bool InteriorShouldBeProcessed(ITreeNode element, CodeStructureElement context)
      {
        return true;
      }

      public void ProcessBeforeInterior(ITreeNode element, CodeStructureElement context)
      {
        var jamTreeNode = element as IJamTreeNode;
        if (jamTreeNode != null)
        {
          var tokenType = jamTreeNode.GetTokenType();
          if (tokenType == null || (!tokenType.IsComment && !tokenType.IsComment))
            SetCurrentRoot(element, jamTreeNode.Accept(this, GetCurrentRoot(context)));
        }
      }

      public void ProcessAfterInterior(ITreeNode element, CodeStructureElement context)
      {
        ResetCurrentRoot(element);
      }

      private void SetCurrentRoot(ITreeNode treeNode, CodeStructureElement root)
      {
        if (root != null)
          myRoots.Push(JetTuple.Of(treeNode, root));
      }

      private CodeStructureElement GetCurrentRoot(CodeStructureElement root)
      {
        return myRoots.Any() ? myRoots.Peek().B : root;
      }

      private void ResetCurrentRoot(ITreeNode treeNode)
      {
        if (myRoots.Any() && myRoots.Peek().A == treeNode)
          myRoots.Pop();
      }

      #endregion

      public override CodeStructureElement VisitGlobalVariableDeclaration(IGlobalVariableDeclaration globalVariableDeclarationParam, CodeStructureElement context)
      {
        return new JamGlobalVariableCodeElement(globalVariableDeclarationParam, context, myPsiIconManager);
      }

      public override CodeStructureElement VisitProcedureDeclaration(IProcedureDeclaration procedureDeclarationParam, CodeStructureElement context)
      {
        return new JamProcedureCodeElement(procedureDeclarationParam, context, myPsiIconManager);
      }
    }
  }
}