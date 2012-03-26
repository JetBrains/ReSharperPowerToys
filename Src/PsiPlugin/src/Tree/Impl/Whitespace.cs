using System;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.PsiPlugin.Parsing;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
    internal abstract class WhitespaceBase : PsiTokenBase, IWhitespaceNode
    {
        private readonly string myText;

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

        public virtual bool IsNewLine
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

        public virtual bool IsNewLine
        {
            get { return true; }
        }
    }  
}
