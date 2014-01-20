using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.TextControl;

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services.Bulbs
{
  [ContextActionDataBuilder(typeof(PsiContextActionDataProvider))]
  public class PsiContextActionDataBuilder : IContextActionDataBuilder
  {
    public IContextActionDataProvider Build(ISolution solution, ITextControl textControl)
    {
      if (!solution.GetPsiServices().Caches.IsIdle.Value)
        return null;

      var projectFile = textControl.Document.GetPsiSourceFile(solution);
      if (projectFile == null || !projectFile.IsValid())
        return null;
      var psiFile = projectFile.GetPsiFile<PsiLanguage>(new DocumentRange(textControl.Document, textControl.Caret.Offset())) as IPsiFile;
      if (psiFile == null || !psiFile.IsValid() || !psiFile.Language.Is<PsiLanguage>())
        return null;

      return new PsiContextActionDataProvider(solution, textControl, psiFile);
    }
  }
}
