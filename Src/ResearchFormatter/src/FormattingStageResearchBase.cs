using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.ResearchFormatter
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
        return currentRule.Space;
      }
      return new[] {" "};
    }
  }
}
