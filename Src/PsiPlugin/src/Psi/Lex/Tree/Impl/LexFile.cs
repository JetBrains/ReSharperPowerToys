using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.LexGrammar;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Lex.Tree.Impl
{
  internal partial class LexFile
  {
    #region Overrides of TreeElement

    public override PsiLanguageType Language
    {
      get { return LexLanguage.Instance; }
    }

    #endregion
  }
}
