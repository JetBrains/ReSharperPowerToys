using System;
using System.IO;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.CodeStructure;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.UI.PopupMenu;
using JetBrains.UI.TreeView;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Jam.CodeStructure
{
  internal class JamCodeStructureElementBase<T> : CodeStructureElement where T : class, IJamTreeNode
  {
    private readonly TextRange myTextRange;
    private readonly PsiLanguageType myLanguage;
    private readonly ITreeNodePointer<T> myNodePointer;

    protected JamCodeStructureElementBase([NotNull] T treeNode, [NotNull] CodeStructureElement root, [NotNull] PsiIconManager psiIconManager) : base(root)
    {
      myLanguage = treeNode.Language;
      myTextRange = treeNode.GetDocumentRange().TextRange;
      myNodePointer = treeNode.CreateTreeElementPointer();
      PsiIconManager = psiIconManager;
    }

    [NotNull]
    public PsiIconManager PsiIconManager { get; private set; }

    [CanBeNull]
    public T PsiElement
    {
      get { return myNodePointer.GetTreeNode(); }
    }

    public override ITreeNode TreeNode
    {
      get { return PsiElement; }
    }

    public override PsiLanguageType Language
    {
      get { return myLanguage; }
    }

    public override IFileStructureAspect GetFileStructureAspect()
    {
      throw new NotImplementedException();
    }

    public override IGotoFileMemberAspect GetGotoMemberAspect()
    {
      throw new NotImplementedException();
    }

    public override IMemberNavigationAspect GetMemberNavigationAspect()
    {
      throw new NotImplementedException();
    }

    public override TextRange GetTextRange()
    {
      return myTextRange;
    }

    protected override void DumpSelf(TextWriter builder)
    {
      var description = new MenuItemDescriptor(this);

      var aspect = GetGotoMemberAspect();
      if (aspect != null)
        aspect.Present(description, new PresentationState());

      builder.Write(description.Text);
    }
  }
}