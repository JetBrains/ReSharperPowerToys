using JetBrains.ReSharper.LexPlugin.Lexer.Lex;
using JetBrains.ReSharper.LexPlugin.Psi.Lex.Parsing;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Parsing;

namespace JetBrains.ReSharper.LexPlugin.Grammar
{
  [Language(typeof (LexLanguage))]
  public class LexLanguageService : LanguageService
  {
    public LexLanguageService(
      PsiLanguageType lexLanguageType, IConstantValueService constantValueService)
      : base(lexLanguageType, constantValueService) { }

    public override bool IsCaseSensitive
    {
      get { return true; }
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

    public override ILexerFactory GetPrimaryLexerFactory()
    {
      return LexLexerFactory.Instance;
    }

    public override ILexer CreateFilteringLexer(ILexer lexer)
    {
      return new FilteringLexLexer(lexer);
    }

    public override IParser CreateParser(
      ILexer lexer, IPsiModule module, IPsiSourceFile sourceFile)
    {
      return new Parser(lexer, sourceFile);
    }

    private class Parser : LexParser
    {
      public Parser(ILexer lexer, IPsiSourceFile sourceFile)
        : base(lexer)
      {
        SourceFile = sourceFile;
      }
    }
  }
}
