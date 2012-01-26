﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Text;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  public abstract class PsiTokenBase : LeafElementBase, IPsiTreeNode, ITokenNode
  {
    public virtual void Accept(TreeNodeVisitor visitor)
    {
      visitor.VisitNode(this);
    }

    public virtual void Accept<TContext>(TreeNodeVisitor<TContext> visitor, TContext context)
    {
      visitor.VisitNode(this, context);
    }

    public virtual TResult Accept<TContext, TResult>(TreeNodeVisitor<TContext, TResult> visitor, TContext context)
    {
      return visitor.VisitNode(this, context);
    }

    /*public IPsiNamespaceDeclaration GetContainingNamespaceDeclaration()
    {
      return GetContainingNode<IPsiNamespaceDeclaration>(false);
    }

    public IPsiTypeMemberDeclaration GetContainingTypeMemberDeclaration()
    {
      return GetContainingNode<IPsiTypeMemberDeclaration>(false);
    }

    public IPsiTypeDeclaration GetContainingTypeDeclaration()
    {
      return GetContainingNode<IPsiTypeDeclaration>(false);
    }*/

    public TokenNodeType GetTokenType()
    {
      return (TokenNodeType)NodeType;
    }

    public override String ToString()
    {
      return base.ToString() + "(type:" + NodeType + ", text:" + GetText() + ")";
    }

    public override StringBuilder GetText(StringBuilder to)
    {
      to.Append(GetText());
      return to;
    }

    public override IBuffer GetTextAsBuffer()
    {
      return new StringBuffer(GetText());
    }
  }
}