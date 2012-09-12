using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.ReSharper.Psi.Jam.Parsing;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam
{
  [Language(typeof (JamLanguage))]
  public class JamLanguageService : LanguageService
  {
    public JamLanguageService(PsiLanguageType psiLanguageType, IConstantValueService constantValueService) : base(psiLanguageType, constantValueService) {}

    public override IWordIndexLanguageProvider WordIndexLanguageProvider
    {
      get { return new JamWordIndexProvider(); }
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
      get { return null; }
    }

    public override ILexerFactory GetPrimaryLexerFactory()
    {
      return JamLexerFactory.Instance;
    }

    public override ILexer CreateFilteringLexer(ILexer lexer)
    {
      return new FilteringJamLexer(lexer);
    }

    public override IParser CreateParser(ILexer lexer, IPsiModule module, IPsiSourceFile sourceFile)
    {
      return new Parser(lexer, sourceFile);
    }

    public override bool IsFilteredNode(ITreeNode node)
    {
      var tokenType = node.GetTokenType();
      return tokenType != null && (tokenType.IsWhitespace || tokenType.IsComment);
    }

    #region Nested type: Parser

    private class Parser : JamParser
    {
      public Parser(ILexer lexer, IPsiSourceFile sourceFile) : base(lexer)
      {
        SourceFile = sourceFile;
      }
    }

    #endregion
  }
}