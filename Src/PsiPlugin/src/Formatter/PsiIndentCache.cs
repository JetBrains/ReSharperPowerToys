using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiIndentCache : CustomizableIndentCacheBase
  {
    public PsiIndentCache() : base(null)
    {}

    public PsiIndentCache([CanBeNull] Func<ITreeNode, CustomIndentType, string> customLineIndenter) : base(customLineIndenter)
    {

    }

    protected override string CalcLineIndent(ITreeNode node)
    {
      throw new NotImplementedException();
    }

    protected override string CalcNodeIndent(ITreeNode node)
    {
      throw new NotImplementedException();
    }
  }
}