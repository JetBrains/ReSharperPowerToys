using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Impl.Shared;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Web.Generation;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.Text;

namespace JetBrains.ReSharper.PsiPlugin.GeneratedDocument.Psi
{
  [GeneratedDocumentService(typeof(PsiProjectFileType))]
  class CSharpFromPsiGeneratedDocumentService : GeneratedDocumentServiceBase
  {

    public CSharpFromPsiGeneratedDocumentService(PsiProjectFileType psiProjectFileType)
    {
    }


    public override IEnumerable<PsiLanguageType> GetSecondaryPsiLanguageTypes(IProject project)
    {
      return new List<PsiLanguageType> { CSharpLanguage.Instance };
    }

    public override bool IsSecondaryPsiLanguageType(IProject project, PsiLanguageType language)
    {
      return language.Is<PsiLanguageType>();
    }

    public override ISecondaryDocumentGenerationResult Generate(PrimaryFileModificationInfo modificationInfo)
    {
      var psiFile = modificationInfo.NewPsiFile as IPsiFile;

      PsiLanguageType language = psiFile != null ? psiFile.Language : PsiLanguage.Instance;

      var gen = new CSharpFromPsiGenerator();
      GenerationResults result = gen.Generate(psiFile);
      return new SecondaryDocumentGenerationResult(
        result.Text.ToString(),
        CSharpLanguage.Instance,
        new RangeTranslatorWithGeneratedRangeMap(result.GeneratedRangeMap),
        LexerFactoryWithPreprocessor(language)
        );
    }

    public override ISecondaryLexingProcess CreateSecondaryLexingService(ISolution solution, MixedLexer mixedLexer, IPsiSourceFile sourceFile = null)
    {
      return null;
    }

    public override ILexerFactory LexerFactoryWithPreprocessor(PsiLanguageType primaryLanguage)
    {
      return new CSharpLexerFactory();
    }
  }

  internal class CSharpLexerFactory : ILexerFactory
  {
    public ILexer CreateLexer(IBuffer buffer)
    {
      return new CSharpLexer(buffer);
    }
  }
}
