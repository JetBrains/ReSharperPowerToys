using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.ReSharper.PsiPlugin.Util;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  class PsiRoleReference : PsiReferenceBase
  {
    public PsiRoleReference(ITreeNode node) : base(node)
    {
    }

    public override ISymbolTable GetReferenceSymbolTable(bool useReferenceName)
    {
      var file = TreeNode.GetContainingFile() as PsiFile;
      if (file == null)
        return EmptySymbolTable.INSTANCE;
      return file.FileRoleSymbolTable;
    }

    public override IReference BindTo(IDeclaredElement element)
    {
      var optionName = (IRoleName)GetTreeNode();
      if (optionName.Parent != null)
      {
        if(((RoleDeclaredElement)element).ChangeName)
        {
          PsiTreeUtil.ReplaceChild(optionName, optionName.FirstChild, ((RoleDeclaredElement)element).NewName);
        }
      }
      return this;
    }

    public override IReference BindTo(IDeclaredElement element, ISubstitution substitution)
    {
      return BindTo(element);
    }

    public void SetName(string shortName)
    {
      Name = shortName;
    }
  }
}
