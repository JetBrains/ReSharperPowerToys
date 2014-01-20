using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.TextControl;

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services.Bulbs
{
  public class PsiContextActionDataProvider : CachedContextActionDataProviderBase, IContextActionDataProvider<IPsiFile>
  {
    public PsiContextActionDataProvider([NotNull] ISolution solution, [NotNull] ITextControl textControl, [NotNull] IFile psiFile) : base(solution, textControl, psiFile)
    {
    }

    public IPsiFile PsiFile { get { return (IPsiFile)base.PsiFile; } }
  }
}
