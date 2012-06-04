using System;
using System.Text;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.Text;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  public abstract class PsiTokenBase : LeafElementBase, IPsiTreeNode, ITokenNode
  {
    #region IPsiTreeNode Members

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

    public override StringBuilder GetText(StringBuilder to)
    {
      to.Append(GetText());
      return to;
    }

    public override IBuffer GetTextAsBuffer()
    {
      return new StringBuffer(GetText());
    }

    public override PsiLanguageType Language
    {
      get { return PsiLanguage.Instance; }
    }

    #endregion

    #region ITokenNode Members

    public TokenNodeType GetTokenType()
    {
      return (TokenNodeType)NodeType;
    }

    #endregion

    public override String ToString()
    {
      return base.ToString() + "(type:" + NodeType + ", text:" + GetText() + ")";
    }
  }
}
