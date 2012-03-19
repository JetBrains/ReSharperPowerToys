using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  public abstract class PsiReferenceBase : TreeReferenceBase<ITreeNode>, ICompleteableReference
  {
    protected readonly ITreeNode TreeNode;
    protected string Name;

    public PsiReferenceBase(ITreeNode node) : base(node)
    {
      TreeNode = node;
      Name = node.GetText();
    }

    public override ResolveResultWithInfo ResolveWithoutCache()
    {
      ISymbolTable table = GetReferenceSymbolTable(true);
      IList<DeclaredElementInstance> elements = new List<DeclaredElementInstance>();
      {
        IList<ISymbolInfo> infos = table.GetSymbolInfos(GetName());
        foreach (ISymbolInfo info in infos)
        {
          var element = new DeclaredElementInstance(info.GetDeclaredElement(), EmptySubstitution.INSTANCE);
          elements.Add(element);
        }
      }
      return new  ResolveResultWithInfo(ResolveResultFactory.CreateResolveResultFinaly(elements),
                                                  ResolveErrorType.OK);
    }

    public ITreeNode GetTreeNode()
    {
      return TreeNode;
    }

    public override string GetName()
    {
      return Name;
    }

    public override IAccessContext GetAccessContext()
    {
     return new ElementAccessContext(myOwner);
    }

    public ISymbolTable GetCompletionSymbolTable()
    {
      return GetReferenceSymbolTable(false);
    }

    public override TreeTextRange GetTreeTextRange()
    {
      return new TreeTextRange(new TreeOffset(TreeNode.GetNavigationRange().TextRange.StartOffset), TreeNode.GetText().Length);
    }


  }
}
