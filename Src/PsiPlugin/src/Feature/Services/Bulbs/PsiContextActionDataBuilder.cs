using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.TextControl;

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services.Bulbs
{
  [ContextActionDataBuilder(typeof(PsiContextActionDataProvider))]
  public class PsiContextActionDataBuilder : IContextActionDataBuilder
  {
    #region Implementation of IContextActionDataBuilder

    public PsiContextActionDataBuilder()
    {
    }

    public IContextActionDataProvider Build(ISolution solution, ITextControl textControl)
    {
      if (!solution.GetPsiServices().CacheManager.IsIdle)
        return null;

      var projectFile = textControl.Document.GetPsiSourceFile(solution);
      if (projectFile == null || !projectFile.IsValid())
        return null;
      var psiFile = projectFile.GetPsiFile<PsiLanguage>(new DocumentRange(textControl.Document, textControl.Caret.Offset())) as IPsiFile;
      if (psiFile == null || !psiFile.IsValid() || !psiFile.Language.Is<PsiLanguage>())
        return null;

      return new PsiContextActionDataProvider(solution, textControl, psiFile);
    }

    #endregion
  }
}
