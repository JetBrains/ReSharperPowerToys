using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Lexer;
using JetBrains.ReSharper.PsiPlugin.Parsing;

namespace JetBrains.ReSharper.PsiPlugin.Grammar
{
    [Language(typeof(PsiLanguage))]
    public class PsiLanguageService : LanguageService
    {
        public PsiLanguageService(PsiLanguageType psiLanguageType, IConstantValueService constantValueService) : base(psiLanguageType, constantValueService)
        {
        }

        public override ILexerFactory GetPrimaryLexerFactory()
        {
            return PsiLexerFactory.Instance;
        }

        public override ILexer CreateFilteringLexer(ILexer lexer)
        {
            return new FilteringPsiLexer(lexer);
        }

        public override string GetTokenReprByTokenType(TokenNodeType token)
        {
            return null;
        }

        public override IParser CreateParser(ILexer lexer, IPsiModule module, IPsiSourceFile sourceFile)
        {
            return new Parser(LanguageType, lexer, sourceFile);
        }

        private class Parser : PsiParser, IParser
        {
            private readonly PsiLanguageType myLanguageType;

            public Parser(PsiLanguageType languageType, ILexer lexer, IPsiSourceFile sourceFile) : base(lexer)
            {
                myLanguageType = languageType;
                mySourceFile = sourceFile;
            }
        }

        public override bool IsFilteredNode(ITreeNode node)
        {
            var tokenNode = node as ITokenNode;
            return tokenNode != null && SimpleFilteringLexer.IS_WHITESPACE(tokenNode.GetTokenType());
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
            get { return null; }
        }
    }
}
