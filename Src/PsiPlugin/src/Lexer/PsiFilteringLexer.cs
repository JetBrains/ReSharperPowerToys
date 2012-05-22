using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;

namespace JetBrains.ReSharper.PsiPlugin.Lexer
{
    public class PsiFilteringLexer : ILexer
    {
        private ILexer myLexer;

        public PsiFilteringLexer(ILexer lexer)
        {
            myLexer = lexer;
        }
        public void Start()
        {
            myLexer.Start();
        }

        public void Advance()
        {
            myLexer.Advance();
        }

        public object CurrentPosition
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public TokenNodeType TokenType
        {
            get { throw new NotImplementedException(); }
        }

        public int TokenStart
        {
            get { throw new NotImplementedException(); }
        }

        public int TokenEnd
        {
            get { throw new NotImplementedException(); }
        }

        public IBuffer Buffer
        {
            get { throw new NotImplementedException(); }
        }
    }
}
