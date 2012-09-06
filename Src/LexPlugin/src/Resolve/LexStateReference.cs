using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree;
using JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree.Impl;
using JetBrains.ReSharper.LexPlugin.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.LexPlugin.Resolve
{
  public class LexStateReference : LexReferenceBase
  {
    public LexStateReference(ITreeNode node) : base(node)
    {
    }

    public override ISymbolTable GetReferenceSymbolTable(bool useReferenceName)
    {
      var file = TreeNode.GetContainingFile() as LexFile;
      if (file == null)
      {
        return EmptySymbolTable.INSTANCE;
      }
      return file.FileStateSymbolTable;
    }

    public override IReference BindTo(IDeclaredElement element)
    {
      var stateName = GetTreeNode() as StateName;
      if (stateName.Parent != null)
      {
        LexTreeUtil.ReplaceChild(stateName, stateName.FirstChild, element.ShortName);
        stateName.SetName(element.ShortName);
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
