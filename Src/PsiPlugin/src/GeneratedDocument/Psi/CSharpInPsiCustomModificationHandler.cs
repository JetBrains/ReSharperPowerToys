using System;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Impl.CustomHandlers;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;

namespace JetBrains.ReSharper.PsiPlugin.GeneratedDocument.Psi
{
  [ProjectFileType(typeof(PsiProjectFileType))]
  class CSharpInPsiCustomModificationHandler : ICSharpCustomModificationHandler
  {
    public bool IsToAddImportsToDeepestScope(ITreeNode context)
    {
      return false;
    }

    public bool CanRemoveUsing(IDocument document, IUsingDirective usingDirective)
    {
      return false;
    }

    public bool CanUseAliases()
    {
      return true;
    }

    public void HandleRemoveStatementsRange(IPsiServices psiServices, ITreeRange treeRange, Action action)
    {
      action();
    }

    public ICSharpStatementsRange HandleAddStatementsRange(IPsiServices psiServices, Func<ITreeNode, ICSharpStatementsRange> addAction,
      IBlock block, ITreeNode anchor, bool before, bool strict)
    {
      return addAction(anchor);
    }

    public void HandleChangeExpressionInStatement(IPsiServices psiServices, IStatement statement, Action changeAction)
    {
      changeAction();
    }

    public void HandleRemoveImport(IPsiServices psiServices, ICSharpTypeAndNamespaceHolderDeclaration scope, IUsingDirective usingDirective, Action action)
    {
      action();
    }

    public IUsingDirective HandleAddImport(IPsiServices psiServices, Func<IUsingDirective> action, ITreeNode generatedAnchor, bool before, IFile generatedFile)
    {
      return null;
    }

    public IUsingDirective HandleAddImport(IPsiServices psiServices, Func<IUsingDirective> action, IUsingDirective generatedAnchor, bool before, IFile generatedFile)
    {
      return action();
    }

    public ITreeNode HandleAddTypeMember(IPsiServices psiServices, Func<ITreeNode> action, IFile generatedFile)
    {
      return action();
    }

    public void HandleRemoveTypeMember(IPsiServices psiServices, ITreeNode node, Action action)
    {
      action();
    }

    public ITreeNode FixupAnchorForAddingTypeMember(IPsiServices psiServices, IFile generatedFile, ITreeNode anchor, bool willInsertBefore,
                                 ITreeNode classDeclaration)
    {
      return anchor;
    }

    public ITreeNode HandleSuperClassChange(IPsiServices psiServices, Func<ITreeNode> action, ITreeNode classDeclaration)
    {
      return action();
    }

    public IBlock GetMethodBodyVisibleForUser(ICSharpFunctionDeclaration method)
    {
      return method.Body;
    }


    public bool PreferQualifiedReference(IQualifiableReferenceWithGlobalSymbolTable reference)
    {
      return true;
    }

    public string GetSpecialMethodType(DeclaredElementPresenterStyle presenter, IMethod method, ISubstitution substitution)
    {
      return null;
    }

    public ITreeRange HandleChangeStatements(IPsiServices psiServices, ITreeRange rangeBeforeChange, Func<ITreeRange> changeAction,
      bool strict)
    {
      return changeAction();
    }
  }
}
