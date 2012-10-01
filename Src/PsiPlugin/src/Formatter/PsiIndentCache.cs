using System;
using System.Text;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiIndentCache : IndentCache<IPsiTreeNode>
  {
    public PsiIndentCache(ICodeFormatterImpl codeFormatter, Func<ITreeNode, CustomIndentType, string> customLineIndenter, AlignmentTabFillStyle tabFillStyle, GlobalFormatSettings formatSettings)
      : base(codeFormatter, customLineIndenter, tabFillStyle, formatSettings)
    {
    }
  }
}
