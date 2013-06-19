using JetBrains.ReSharper.LexPlugin.Lexer.Lex;
using JetBrains.ReSharper.LexPlugin.Psi.Lex.Parsing;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Util;

namespace JetBrains.ReSharper.LexPlugin.Grammar
{
  [Language(typeof (LexLanguage))]
  public class LexLanguageService : LanguageService
  {
    private CommonIdentifierIntern myCommonIdentifierIntern;

    public LexLanguageService(
      PsiLanguageType lexLanguageType, IConstantValueService constantValueService, CommonIdentifierIntern commonIdentifierIntern)
      : base(lexLanguageType, constantValueService)
    {
      myCommonIdentifierIntern = commonIdentifierIntern;
    }

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
      return new Parser(lexer, sourceFile, myCommonIdentifierIntern);
    }

    private class Parser : LexParser
    {
      public Parser(ILexer lexer, IPsiSourceFile sourceFile, CommonIdentifierIntern commonIdentifierIntern)
        : base(lexer, commonIdentifierIntern)
      {
        SourceFile = sourceFile;
      }
    }
  }
}
