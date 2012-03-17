using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.ReSharper.PsiPlugin.Util;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  public abstract class PsiReferenceBase : TreeReferenceBase<ITreeNode>, ICompleteableReference
  {
    protected ITreeNode myTreeNode;
    protected string myName;

    public PsiReferenceBase(ITreeNode node) : base(node)
    {
      myTreeNode = node;
      myName = node.GetText();
    }

    public override ResolveResultWithInfo ResolveWithoutCache()
    {
      ISymbolTable table = GetReferenceSymbolTable(true);
      IList<DeclaredElementInstance> elements = new List<DeclaredElementInstance>();
      if (table != null)
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
      return myTreeNode;
    }

    public override string GetName()
    {
      return myName;
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
      return new TreeTextRange(new TreeOffset(myTreeNode.GetNavigationRange().TextRange.StartOffset), myTreeNode.GetText().Length);
    }


  }
}
