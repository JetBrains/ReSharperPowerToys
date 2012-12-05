using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.ResearchFormatter
{
  public abstract class FormattingStageResearchBase
  {
    protected readonly FormatterResearchBase myFormatter;

    public FormattingStageResearchBase(FormatterResearchBase formatter)
    {
      myFormatter = formatter;
    }

    protected virtual CodeFormattingContext Context { get; private set; }

    public void DoFormat(IProgressIndicator pi)
    {
      if (Context.FirstNode == Context.LastNode)
        return;

      var nodePairs = Context.HierarchicalEnumNodes().Where(p => Context.CanModifyInsideNodeRange(p.First, p.Last)).ToList();
      //var nodePairs = Context.HierarchicalEnumNodes().ToList();

      var spaces = nodePairs.Select(
        range => new FormatResult<IEnumerable<string>>(range, CalcSpaces(new FormattingStageContext(range))));

      FormatterImplHelper.ForeachResult(spaces, pi, res => MakeFormat(res.Range, res.ResultValue));
    }

    private void MakeFormat(FormattingRange range, IEnumerable<string> space)
    {
      ReplaceSpaces(range.First, range.Last, space);
    }

    private void ReplaceSpaces(ITreeNode leftNode, ITreeNode rightNode, IEnumerable<string> wsTexts)
    {
      if (wsTexts == null)
        return;
      FormatterImplHelper.ReplaceSpaces(leftNode, rightNode, CreateWhitespaces(wsTexts));
    }

    protected abstract ITreeNode[] CreateWhitespaces(IEnumerable<string> wsTexts);

    private IEnumerable<string> CalcSpaces(FormattingStageContext formattingStageContext)
    {
      IFormattingRule currentRule = null;
      foreach (var formattingRule in myFormatter.FormattingRules)
      {
        if (formattingRule.Match(formattingStageContext))
        {
          if (currentRule == null)
          {
            if (formattingRule.GetPriority() > 0)
            {
              currentRule = formattingRule;
            }
          }
          else
          {
            if (formattingRule.GetPriority() > currentRule.GetPriority())
            {
              currentRule = formattingRule;
            }
          }
        }
      }

      if (currentRule != null)
      {
        return currentRule.Space(formattingStageContext, this);
      }
      if (IsTokensGlued(formattingStageContext))
      {
        return new[] {" "};
      } else
      {
        return new[] {""};
      }
    }

    private bool IsTokensGlued(FormattingStageContext formattingStageContext)
    {
      if(formattingStageContext.LeftChild.GetText() == "")
      {
        return false;
      }
      if (formattingStageContext.RightChild.GetText() == "")
      {
        return false;
      }
      var lexer = GetLexer(formattingStageContext);
      return lexer.LookaheadToken(1) == null;
    }

    protected  ILexer GetLexer(FormattingStageContext formattingStageContext)
    {
      string s = "";
      if (formattingStageContext.LeftChild.FirstChild == null)
      {
        s = formattingStageContext.LeftChild.GetText();
      }
      else
      {
        s = formattingStageContext.LeftChild.GetLastTokenIn().GetText();
      }
      if (formattingStageContext.RightChild.FirstChild == null)
      {
        s += formattingStageContext.RightChild.GetText();
      }
      else
      {
        s += formattingStageContext.RightChild.GetFirstTokenIn().GetText();
      }
      return GetLexer(s);
    }

    protected abstract ILexer GetLexer(string text);

    public virtual TokenNodeType NewLineType { get; private set; }
    public virtual TokenNodeType WhiteSpaceType { get; private set; }
    public abstract ITreeNode AsWhitespaceNode(ITreeNode node);

    public bool HasLineFeedsTo(ITreeNode fromNode, ITreeNode toNode)
    {
      return GetLineFeedsTo(fromNode, toNode).Any();
    }

    private IEnumerable<ITreeNode> GetLineFeedsTo(ITreeNode fromNode, ITreeNode toNode)
    {
      return fromNode.GetWhitespacesTo(toNode).Where(wsNode => (wsNode.GetTokenType() == NewLineType) && (AsWhitespaceNode(wsNode) != null));
    }
  }
}
