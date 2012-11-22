using System;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.JavaScript.Parsing;
using JetBrains.ReSharper.Psi.JavaScript.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.ResearchFormatter.JavaScript
{
  public class JavaScriptIndentingStageResearch : IndentingStageResearchBase
  {
    public JavaScriptIndentingStageResearch(JavaScriptResearchFormatter javaScriptResearchFormatter):base(javaScriptResearchFormatter)
    {
    }

    #region Overrides of IndentingStageResearchBase

    protected override ITreeNode[] CreateSpace(string indent)
    {
      return new[] {TreeElementFactory.CreateLeafElement(JavaScriptTokenType.WHITE_SPACE, FormatterImplHelper.GetPooledWhitespace(indent), 0, indent.Length)};
    }

    public override ITreeNode AsWhitespaceNode(ITreeNode node)
    {
      return node as IWhitespaceNode;
    }

    public override TokenNodeType NewLineType
    {
      get { return JavaScriptTokenType.NEW_LINE; }
    }

    public override TokenNodeType WhiteSpaceType
    {
      get { return JavaScriptTokenType.WHITE_SPACE; }
    }

    #endregion
  }
}