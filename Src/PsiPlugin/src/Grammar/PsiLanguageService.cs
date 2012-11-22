using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Formatter;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Lexer.Psi;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing;

namespace JetBrains.ReSharper.PsiPlugin.Grammar
{
  [Language(typeof (PsiLanguage))]
  public class PsiLanguageService : LanguageService
  {
    private readonly PsiCodeFormatter myFormatter;
    //private readonly PsiResearchFormatter myFormatter;

    public PsiLanguageService(PsiLanguageType psiLanguageType, IConstantValueService constantValueService, PsiCodeFormatter formatter)
      : base(psiLanguageType, constantValueService)

    {
      myFormatter = formatter;
    }

    public override IWordIndexLanguageProvider WordIndexLanguageProvider
    {
      get { return new PsiWordIndexProvider(); }
    }

    public override ILanguageCacheProvider CacheProvider
    {
      get { return null; }
    }

    public override bool SupportTypeMemberCache
    {
      get { return true; }
    }

    public override ITypePresenter TypePresenter
    {
      get { return DefaultTypePresenter.Instance; }
    }

    public override ICodeFormatter CodeFormatter
    {
      get { return myFormatter; }
    }

    public override ILexerFactory GetPrimaryLexerFactory()
    {
      return PsiLexerFactory.Instance;
    }

    public override ILexer CreateFilteringLexer(ILexer lexer)
    {
      var tokenBuffer = new TokenBuffer(lexer);
      return new FilteringPsiLexer(tokenBuffer.CreateLexer());
    }

    public override IParser CreateParser(ILexer lexer, IPsiModule module, IPsiSourceFile sourceFile)
    {
      return new Parser(lexer, sourceFile);
    }

    public override bool IsFilteredNode(ITreeNode node)
    {
      var tokenNode = node as ITokenNode;
      return tokenNode != null && SimpleFilteringLexer.IS_WHITESPACE(tokenNode.GetTokenType());
    }

    #region Nested type: Parser

    private class Parser : PsiParser
    {
      public Parser(ILexer lexer, IPsiSourceFile sourceFile)
        : base(lexer)
      {
        SourceFile = sourceFile;
      }
    }

    #endregion
  }
}
