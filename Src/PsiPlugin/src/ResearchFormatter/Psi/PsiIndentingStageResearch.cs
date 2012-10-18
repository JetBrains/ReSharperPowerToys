using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.ResearchFormatter.Psi
{
  internal class PsiIndentingStageResearch : IndentingStageResearchBase
  {
    #region Overrides of IndentingStageResearchBase

    public PsiIndentingStageResearch(FormatterResearchBase formatter) : base(formatter)
    {
    }

    public override ITreeNode AsWhitespaceNode(ITreeNode node)
    {
      return node as IWhitespaceNode;
    }

    protected override ITreeNode[] CreateSpace(string indent)
    {
      return new[] {TreeElementFactory.CreateLeafElement(PsiTokenType.WHITE_SPACE, FormatterImplHelper.GetPooledWhitespace(indent), 0, indent.Length)};
    }

    public override TokenNodeType NewLineType
    {
      get { return PsiTokenType.NEW_LINE; }
    }

    public override TokenNodeType WhiteSpaceType
    {
      get { return PsiTokenType.WHITE_SPACE; }
    }

    #endregion
  }
}