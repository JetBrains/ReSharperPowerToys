using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.SelectEmbracingConstruct;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.SelectEmbracingConstruct
{
  [ProjectFileType(typeof(PsiProjectFileType))]
  class PsiEmbracingConstructProvider : ISelectEmbracingConstructProvider
  {
    public bool IsAvailable(IPsiSourceFile sourceFile)
    {
      return sourceFile.Properties.ShouldBuildPsi;
    }

    public ISelectedRange GetSelectedRange(IPsiSourceFile sourceFile, DocumentRange documentRange)
    {
      var file = (PsiFile)sourceFile.GetPsiFile<PsiLanguage>(documentRange);
      if (file == null)
        return null;

      return GetSelectedRange(file, documentRange);
    }

    private ISelectedRange GetSelectedRange(PsiFile file, DocumentRange documentRange)
    {
      var fileNode = file;
      var translatedRange = fileNode.Translate(documentRange);
      if (!translatedRange.IsValid())
        return null;

      return new PsiSelection(fileNode, translatedRange.StartOffset);
    }

  }
}
