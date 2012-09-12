using System;
using JetBrains.Application;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  public abstract class JamExpressionBase : JamCompositeElement, IJamExpression
  {
    public IJamExpression ReplaceBy<TExpression>(TExpression expression) where TExpression : class, IJamExpression
    {
      Assertion.Assert(expression != null, "expression != null");

      using (WriteLockCookie.Create(IsPhysical()))
      {
        // special check - replace to sub-expressein
        if (Contains(expression))
          expression = expression.Copy(Parent);
        return ModificationUtil.ReplaceChild(this, expression);
      }
    }

    public ExpressionAccessType GetAccessType()
    {
      throw new NotImplementedException();
    }
  }
}