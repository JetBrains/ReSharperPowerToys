using JetBrains.ReSharper.LexPlugin.Lexer.Lex;
using JetBrains.ReSharper.LexPlugin.Psi.Lex.Parsing;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.LexPlugin.Grammar
{
  [Language(typeof (LexLanguage))]
  public class LexLanguageService : LanguageService
  {
    //private readonly LexCodeFormatter myFormatter;

    public LexLanguageService(PsiLanguageType LexLanguageType, IConstantValueService constantValueService)
      : base(LexLanguageType, constantValueService)

    {
      //myFormatter = formatter;
    }

    public override IWordIndexLanguageProvider WordIndexLanguageProvider
    {
      get { return new LexWordIndexProvider(); }
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

   /*public override ICodeFormatter CodeFormatter
    {
      get { return myFormatter; }
    }*/

    public override ILexerFactory GetPrimaryLexerFactory()
    {
      return LexLexerFactory.Instance;
    }

    public override ILexer CreateFilteringLexer(ILexer lexer)
    {
      return new FilteringLexLexer(lexer);
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

    private class Parser : LexParser
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
