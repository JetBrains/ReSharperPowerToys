using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.JavaScript.Parsing;
using JetBrains.ReSharper.Psi.JavaScript.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.ResearchFormatter.JavaScript
{
  public class JavaScriptFormattingStageResearch : FormattingStageResearchBase
  {
    private readonly CodeFormattingContext myContext;

    public JavaScriptFormattingStageResearch(CodeFormattingContext context, JavaScriptResearchFormatter javaScriptResearchFormatter) : base(javaScriptResearchFormatter)
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
        if (!JavaScriptLexer.IsWhitespace(text))
          throw new ApplicationException("Inconsistent space structure");
        return CreateSpace(text);
      }).ToArray();
    }

    protected override ILexer GetLexer(string text)
    {
      return new JavaScriptLexerImpl(new StringBuffer(text));
    }

    public static IWhitespaceNode CreateNewLine()
    {
      var buf = FormatterImplHelper.NewLineBuffer;
      return (IWhitespaceNode)TreeElementFactory.CreateLeafElement(JavaScriptTokenType.NEW_LINE, buf, 0, buf.Length);
    }

    [NotNull]
    public static IWhitespaceNode CreateSpace(string spaceText)
    {
      return (IWhitespaceNode)TreeElementFactory.CreateLeafElement(JavaScriptTokenType.WHITE_SPACE, FormatterImplHelper.GetPooledWhitespace(spaceText), 0, spaceText.Length);
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
  }
}