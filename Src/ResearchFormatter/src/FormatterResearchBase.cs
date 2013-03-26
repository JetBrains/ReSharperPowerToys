using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.Util;

namespace JetBrains.ReSharper.ResearchFormatter
{
  public abstract class FormatterResearchBase : CodeFormatterBase
  {
    protected FormatterResearchBase(ISettingsStore settingsStore) : base(settingsStore)
    {
    }

    public virtual IEnumerable<IFormattingRule> FormattingRules { get; private set; }

    public virtual IEnumerable<IndentingRule> IndentingRules { get; private set; }

    public override ITreeRange Format(ITreeNode firstElement, ITreeNode lastElement, CodeFormatProfile profile, IProgressIndicator pi, IContextBoundSettingsStore overrideSettingsStore = null)
    {
      var firstNode = firstElement;
      var lastNode = lastElement;
      Assertion.Assert(firstNode != null || lastNode != null, "The condition (firstNode != null || lastNode != null) is false.");

      // if first node is null, then start from the file beginning
      if (firstNode == null)
      {
        firstNode = lastNode.PathToRoot().Last();
        foreach (var child in firstNode.FirstDescendants())
        {
          if (firstNode == lastNode)
            return new TreeRange(firstElement, lastElement);
          firstNode = child;
        }
      }

      var token = (firstNode as ITokenNode).SkipLeftWhitespaces();
      if (token != null)
        firstNode = token;

      //var solution = firstNode.GetSolution();
      //var globalSettings = GlobalFormatSettingsHelper.GetService(solution).GetSettingsForLanguage(LanguageType);
      //var contextBoundSettingsStore = GetProperContextBoundSettingsStore(overrideSettingsStore, firstNode);
      //var formatterSettings = new JavaScriptCodeFormattingSettings(contextBoundSettingsStore, mySettingsOptimization, globalSettings);

      using (firstNode.CreateWriteLock())
      using (pi.SafeTotal(4))
      {
        var context = new CodeFormattingContext(this, firstNode, lastNode, NullProgressIndicator.Instance);
        if (profile != CodeFormatProfile.INDENT)
        {
          /*using (var subPi = pi.CreateSubProgress(1))
          {
            //FormatterImplHelper.DecoratingIterateNodes(context, context.FirstNode, context.LastNode, new JavaScriptDecorationStage(formatterSettings, profile, subPi));
          }*/

          using (var subPi = pi.CreateSubProgress(1))
          {
            using (subPi.SafeTotal(2))
            {
              FormattingStage(context).DoFormat(subPi.CreateSubProgress(1));
              IndentingStage().DoIndent(context, subPi.CreateSubProgress(1));
            }
          }
        }
        else
        {
          using (var subPi = pi.CreateSubProgress(4))
          {
            IndentingStage().DoIndent(context, subPi, true);
          }
        }
      }
      return new TreeRange(firstElement, lastElement);
    }

    protected abstract IndentingStageResearchBase IndentingStage();

    protected abstract FormattingStageResearchBase FormattingStage(CodeFormattingContext context);

    public override void FormatInsertedNodes(ITreeNode nodeFirst, ITreeNode nodeLast, bool formatSurround)
    {
      Format(nodeFirst, nodeLast, CodeFormatProfile.GENERATOR, null);
    }

    public override ITreeRange FormatInsertedRange(ITreeNode nodeFirst, ITreeNode nodeLast, ITreeRange origin)
    {
      Format(nodeFirst, nodeLast, CodeFormatProfile.GENERATOR, null);
      return new TreeRange(nodeFirst, nodeLast);
    }

    public override void FormatReplacedNode(ITreeNode oldNode, ITreeNode newNode)
    {
      FormatInsertedNodes(newNode, newNode, false);
    }

    public override void FormatDeletedNodes(ITreeNode parent, ITreeNode prevNode, ITreeNode nextNode)
    {
      Format(
        prevNode,
        nextNode,
        CodeFormatProfile.GENERATOR,
        null);
    }
  }
}
