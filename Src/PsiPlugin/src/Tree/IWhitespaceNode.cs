using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
    public interface IWhitespaceNode : ITokenNode
    {
        bool IsNewLine { get; }
    }  
}
