using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
    internal class Identifier : PsiTokenBase, IPsiIdentifier
    {
        private readonly string myText = null;

        public Identifier(string text)
        {
            myText = text;
        }

        public string Name
        {
            get { return PsiResolveUtil.ReferenceName(myText); }
        }

        public override NodeType NodeType
        {
            get { return PsiTokenType.IDENTIFIER; }
        }

        public override int GetTextLength()
        {
            return myText.Length;
        }

        public override string GetText()
        {
            return myText;
        }
    } 
}
