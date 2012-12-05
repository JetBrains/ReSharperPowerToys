using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.ResearchFormatter.Psi
{
  internal class PsiFormattingStageResearch : FormattingStageResearchBase
  {
    private readonly CodeFormattingContext myContext;

    public PsiFormattingStageResearch(CodeFormattingContext context, FormatterResearchBase formatter) : base(formatter)
    {
      myContext = context;
    }

    protected override CodeFormattingContext Context
    {
      get { return myContext; }
    }

    protected override ITreeNode[] CreateWhitespaces(IEnumerable<string> wsTexts)
    {
      if (wsTexts == null)
        throw new ArgumentNullException("wsTexts");

      return wsTexts.Where(text => !text.IsEmpty()).Select(text =>
        {
          if (text.IsNewLine())
            return CreateNewLine();
          // consistency check (remove in release?)
          if (!PsiLexer.IsWhitespace(text))
            throw new ApplicationException("Inconsistent space structure");
          return CreateSpace(text);
        }).ToArray();
    }

    protected override ILexer GetLexer(string text)
    {
      return new PsiLexer(new StringBuffer(text));
    }

    public static IWhitespaceNode CreateNewLine()
    {
      var buf = FormatterImplHelper.NewLineBuffer;
      return (IWhitespaceNode)TreeElementFactory.CreateLeafElement(PsiTokenType.NEW_LINE, buf, 0, buf.Length);
    }

    [NotNull]
    public static IWhitespaceNode CreateSpace(string spaceText)
    {
      return (IWhitespaceNode)TreeElementFactory.CreateLeafElement(PsiTokenType.WHITE_SPACE, FormatterImplHelper.GetPooledWhitespace(spaceText), 0, spaceText.Length);
    }

    public override ITreeNode AsWhitespaceNode(ITreeNode node)
    {
      return node as IWhitespaceNode;
    }

    public override TokenNodeType NewLineType
    {
      get { return PsiTokenType.NEW_LINE; }
    }

    public override TokenNodeType WhiteSpaceType
    {
      get { return PsiTokenType.WHITE_SPACE; }
    }
  }
}