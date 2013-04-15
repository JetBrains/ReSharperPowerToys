using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.LexPlugin.Grammar;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Text;

namespace JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree.Impl
{
  public abstract class LexTokenBase : LeafElementBase, ILexTreeNode, ITokenNode
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
      get { return LexLanguage.Instance; }
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
