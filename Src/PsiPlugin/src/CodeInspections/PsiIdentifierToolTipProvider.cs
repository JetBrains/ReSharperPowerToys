using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Descriptions;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.PsiGrammar;
using JetBrains.TextControl.Markup;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections
{
  [SolutionComponent]
  internal class PsiIdentifierTooltipProvider : IdentifierTooltipProvider<PsiLanguage>
  {
    public PsiIdentifierTooltipProvider(ISolution solution, IDeclaredElementDescriptionPresenter presenter)
      : base(solution, presenter)
    {
    }

    protected override DeclaredElementInstance GetCustomElementInstance(ITreeNode element)
    {
      return null;
    }

    protected override bool ShouldShowTooltip(IHighlighter highlighter)
    {
      // suppress tooltip on mutable local variables, since there is already local variable tooltip
      return highlighter.AttributeId != HighlightingAttributeIds.MUTABLE_LOCAL_VARIABLE_IDENTIFIER_ATTRIBUTE;
    }
  }
}
