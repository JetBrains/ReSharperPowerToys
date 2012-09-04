using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.ReSharper.PsiPlugin.Util;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  internal class PsiPathReference : PsiReferenceBase
  {
    public PsiPathReference(ITreeNode node)
      : base(node)
    {
    }

    public override ISymbolTable GetReferenceSymbolTable(bool useReferenceName)
    {
      var file = TreeNode.GetContainingFile() as PsiFile;
      if (file == null)
      {
        return EmptySymbolTable.INSTANCE;
      }
      return file.FilePathSymbolTable;
    }

    public override IReference BindTo(IDeclaredElement element)
    {
      var pathName = (IPathName)GetTreeNode();
      if (pathName.Parent != null)
      {
        PsiTreeUtil.ReplaceChild(pathName, pathName.FirstChild, element.ShortName);
      }
      IReference reference = new PsiPathReference(pathName);
      pathName.SetReference(reference);
      return reference;
    }

    public override IReference BindTo(IDeclaredElement element, ISubstitution substitution)
    {
      return BindTo(element);
    }
  }
}
