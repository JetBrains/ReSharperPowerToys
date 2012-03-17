using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.ReSharper.PsiPlugin.Util;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  public class PsiOptionReference : PsiReferenceBase
  {
    public PsiOptionReference(ITreeNode node) : base(node)
    {
    }

    public override ISymbolTable GetReferenceSymbolTable(bool useReferenceName)
    {
      var file = myTreeNode.GetContainingFile() as PsiFile;
      if (file == null)
        return EmptySymbolTable.INSTANCE;
      return file.FileOptionSymbolTable;
    }

    public override IReference BindTo(IDeclaredElement element)
    {
      IOptionName optionName = (IOptionName)GetTreeNode();
      if (optionName.Parent != null)
      {
        if (!(element.ShortName.Equals(optionName.FirstChild.GetText())))
        {
          PsiTreeUtil.ReplaceChild(optionName, optionName.FirstChild, element.ShortName);
        }
      }
      return this;
    }

    public override IReference BindTo(IDeclaredElement element, ISubstitution substitution)
    {
      return BindTo(element);
    }
  }
}
