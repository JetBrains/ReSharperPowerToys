using System.Collections.Generic;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Impl.PsiManagerImpl;
using JetBrains.ReSharper.Psi.Impl.Shared;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.GeneratedDocument
{
  [GeneratedDocumentService(typeof(PsiProjectFileType))]
  class CSharpFromPsiGeneratedDocumentService : IGeneratedDocumentService
  {
    public CSharpFromPsiGeneratedDocumentService(PsiProjectFileType psiProjectFileType)
    {
    }

    public ICollection<PsiLanguageType> GetSecondaryPsiLanguageTypes()
    {
      return new List<PsiLanguageType> {CSharpLanguage.Instance};
    }

    public bool CanHandle(ProjectFileType projectFileType)
    {
      return (projectFileType is PsiProjectFileType);
    }

    public ISecondaryDocumentGenerationResult Generate(PrimaryFileModificationInfo modificationInfo)
    {
      var psiFile = modificationInfo.NewPsiFile as IPsiFile;
      var gen = new CSharpFromPsiGenerator(psiFile);
      var sourceFile = modificationInfo.SourceFile;
      var results = gen.Generate();
      PsiLanguageType language = psiFile != null ? psiFile.Language : PsiLanguage.Instance;
      return new SecondaryDocumentGenerationResult(
          sourceFile,
          results.Text,
          CSharpLanguage.Instance,
          new RangeTranslatorWithGeneratedRangeMap(results.GeneratedRangeMap),
          LexerFactoryWithPreprocessor(language)
          );
    }

    public ICollection<IPreCommitResult> ExecuteSecondaryDocumentCommitWork(PrimaryFileModificationInfo primaryFileModificationInfo, CachedPsiFile cachedPsiFile, TreeTextRange oldTreeRange, string newText)
    {
     /* var newElement = primaryFileModificationInfo.NewElement;
      var oldElement = primaryFileModificationInfo.OldElement;
      if (oldElement == null || newElement == null)
        return null;
      if (!HasProjectedInnerElements(newElement) && !HasProjectedInnerElements(oldElement) && !HasProjectedOuterElements(newElement))
      {
        // change is unrelated, just fix ranges...
        return CreateFixRangeTranslatorsCommitResult(cachedPsiFile, oldTreeRange, newText);
      }

      // try promote change to the secondary psi and reparse!
      if (IsUnderScriptBlock(newElement))
      {
        return CreatePromotedDocumentChangeCommitResult(cachedPsiFile, oldTreeRange, newText);
      }
      */
      return null;
    }

    public bool ProcessChangeFromGeneratedToPrimary(IPsiSourceFile sourceFile, TextRange range, string oldText, string newText, PsiLanguageType language)
    {
      //throw new NotImplementedException();
      return false;
    }

    public void ProcessChangeFromPrimaryToGenerated(TreeTextRange range, string oldText, string newText, ISecondaryRangeTranslator rangeTranslator, IFile file, IPsiTransactionAction transactionAction)
    {
      //throw new NotImplementedException();
    }

    public DocumentRange TryFindNavigationRangeInPrimaryDocument(ITreeNode element)
    {
      //throw new NotImplementedException();
      return element.GetNavigationRange();
    }

    public ISecondaryLexingProcess CreateSecondaryLexingService(ISolution solution, MixedLexer mixedLexer, IPsiSourceFile sourceFile = null)
    {
      //throw new NotImplementedException();
      return null;
    }

    public ILexerFactory LexerFactoryWithPreprocessor(PsiLanguageType primaryLanguage)
    {
      return new CSharpLexerFactory();
    }

    public bool CheckValid(IFile generatedFile)
    {
      //throw new NotImplementedException();
      return false;
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
