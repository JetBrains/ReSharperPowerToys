using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.ReSharper.PsiPlugin.Util;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  public class PsiRuleReference : PsiReferenceBase
  {
    public PsiRuleReference(ITreeNode node)
      : base(node)
    {
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
      return new ResolveResultWithInfo(ResolveResultFactory.CreateResolveResultFinaly(elements),
        ResolveErrorType.OK);
    }

    public override ISymbolTable GetReferenceSymbolTable(bool useReferenceName)
    {
      var file = TreeNode.GetContainingFile() as PsiFile;
      if (file == null)
      {
        return EmptySymbolTable.INSTANCE;
      }
      return file.FileRuleSymbolTable;
    }

    public override IReference BindTo(IDeclaredElement element)
    {
      var ruleName = (IRuleName)GetTreeNode();
      if (ruleName.Parent != null)
      {
        PsiTreeUtil.ReplaceChild(ruleName, ruleName.FirstChild, element.ShortName);
        ruleName.SetName(element.ShortName);
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
