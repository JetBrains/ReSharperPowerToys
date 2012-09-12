using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Descriptions;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.CodeInspections
{
  [SolutionComponent]
  internal class JamIdentifierTooltipProvider : IdentifierTooltipProvider<JamLanguage>
  {
    public JamIdentifierTooltipProvider(ISolution solution, IDeclaredElementDescriptionPresenter presenter)
      : base(solution, presenter) { }

    protected override DeclaredElementInstance GetCustomElementInstance(ITreeNode element)
    {
      return null;
    }
  }
}