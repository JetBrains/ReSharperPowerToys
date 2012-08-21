using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Util;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  public class PsiVariableReference : PsiReferenceBase
  {
    public PsiVariableReference(ITreeNode node)
      : base(node)
    {
    }

    public override ISymbolTable GetReferenceSymbolTable(bool useReferenceName)
    {
      ITreeNode node = TreeNode;
      while (node != null)
      {
        if (node is IRuleDeclaration)
        {
          break;
        }
        node = node.Parent;
      }
      var ruleDeclaration = ((IRuleDeclaration)node);
      if (ruleDeclaration == null)
      {
        return EmptySymbolTable.INSTANCE;
      }
      return ruleDeclaration.VariableSymbolTable;
    }

    public override IReference BindTo(IDeclaredElement element)
    {
      var variableName = (IVariableName)GetTreeNode();
      if (variableName.Parent != null)
      {
        PsiTreeUtil.ReplaceChild(variableName, variableName.FirstChild, element.ShortName);
        variableName.SetName(element.ShortName);
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
