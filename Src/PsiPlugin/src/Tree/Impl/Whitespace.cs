using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.PsiPlugin.Parsing;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
    internal abstract class WhitespaceBase : PsiTokenBase, IWhitespaceNode
    {
        protected readonly string myText;

        protected WhitespaceBase(string text)
        {
            myText = text;
        }

        public override int GetTextLength()
        {
            return myText.Length;
        }

        public override string GetText()
        {
            return myText;
        }

        public override String ToString()
        {
            return base.ToString() + " spaces:" + "\"" + GetText() + "\"";
        }

        public abstract bool IsNewLine { get; }
    }

    internal class Whitespace : WhitespaceBase
    {
        public Whitespace(string text)
            : base(text)
        {
        }

        public override NodeType NodeType
        {
            get { return PsiTokenType.WHITE_SPACE; }
        }

        public override bool IsNewLine
        {
            get { return false; }
        }
    }

    internal class NewLine : WhitespaceBase
    {
        public NewLine(string text)
            : base(text)
        {
        }

        public override NodeType NodeType
        {
            get { return PsiTokenType.NEW_LINE; }
        }

        public override bool IsNewLine
        {
            get { return true; }
        }
    }  
}
