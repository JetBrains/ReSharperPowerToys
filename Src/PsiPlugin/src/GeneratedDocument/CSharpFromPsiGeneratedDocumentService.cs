﻿using System.Collections.Generic;
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
using JetBrains.ReSharper.Psi.Web.Generation;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.GeneratedDocument
{
  [GeneratedDocumentService(typeof(PsiProjectFileType))]
  class CSharpFromPsiGeneratedDocumentService : IGeneratedDocumentService
  {
    //private CSharpFromPsiGenerator myGenerator;

    public CSharpFromPsiGeneratedDocumentService(PsiProjectFileType psiProjectFileType)
    {
      //myGenerator = new CSharpFromPsiGenerator();
    }

    public ICollection<PsiLanguageType> GetSecondaryPsiLanguageTypes(IProject project)
    {
      return new List<PsiLanguageType> { CSharpLanguage.Instance };
    }

    public bool CanHandle(ProjectFileType projectFileType)
    {
      return (projectFileType is PsiProjectFileType);
    }

    public ISecondaryDocumentGenerationResult Generate(PrimaryFileModificationInfo modificationInfo)
    {
      var sourceFile = modificationInfo.SourceFile;
      var psiFile = modificationInfo.NewPsiFile as IPsiFile;
      PsiLanguageType language = psiFile != null ? psiFile.Language : PsiLanguage.Instance;
      GenerationResults result;
      /*if (modificationInfo.NewElement is IRuleDeclaration)
      {
        result = myGenerator.Generate(psiFile, false);
      }
      else
      {*/
      var gen = new CSharpFromPsiGenerator();
      result = gen.Generate(psiFile);
      //}
      return new SecondaryDocumentGenerationResult(
        sourceFile,
        result.Text.ToString(),
        CSharpLanguage.Instance,
        new RangeTranslatorWithGeneratedRangeMap(result.GeneratedRangeMap),
        LexerFactoryWithPreprocessor(language)
        );
    }

    public ICollection<IPreCommitResult> ExecuteSecondaryDocumentCommitWork(PrimaryFileModificationInfo primaryFileModificationInfo, CachedPsiFile cachedPsiFile, TreeTextRange oldTreeRange, string newText)
    {
      return null;
    }

    public bool ProcessChangeFromGeneratedToPrimary(IPsiSourceFile sourceFile, TextRange range, string oldText, string newText, PsiLanguageType language)
    {
      return false;
    }

    public void ProcessChangeFromPrimaryToGenerated(TreeTextRange range, string oldText, string newText, ISecondaryRangeTranslator rangeTranslator, IFile file, IPsiTransactionAction transactionAction)
    {
    }

    public DocumentRange TryFindNavigationRangeInPrimaryDocument(ITreeNode element)
    {
      return element.GetNavigationRange();
    }

    public ISecondaryLexingProcess CreateSecondaryLexingService(ISolution solution, MixedLexer mixedLexer, IPsiSourceFile sourceFile = null)
    {
      return null;
    }

    public ILexerFactory LexerFactoryWithPreprocessor(PsiLanguageType primaryLanguage)
    {
      return new CSharpLexerFactory();
    }

    public bool CheckValid(IFile generatedFile)
    {
      return true;
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
