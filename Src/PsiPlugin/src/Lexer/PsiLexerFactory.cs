using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.Text;

namespace JetBrains.ReSharper.PsiPlugin.Lexer
{
    class PsiLexerFactory : ILexerFactory
    {
        private readonly ILexerFactory myLexerFactory;
        private readonly IPsiSourceFile mySourceFile;
        private readonly ProjectFileType myProjectFileType;

        public static PsiLexerFactory Instance = new PsiLexerFactory();

        public PsiLexerFactory()
        {
            
        }

        public PsiLexerFactory(ILexerFactory lexerFactory, [NotNull] IPsiSourceFile sourceFile, ProjectFileType projectFileType)
        {
            myLexerFactory = lexerFactory;
            mySourceFile = sourceFile;
            myProjectFileType = projectFileType;
        }

        public ILexer CreateLexer(IBuffer buffer)
        {
            return new PsiLexer(buffer);
        }
    }
}
