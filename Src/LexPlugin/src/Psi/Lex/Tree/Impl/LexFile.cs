using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.LexPlugin.Grammar;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree.Impl
{
  internal partial class LexFile
  {
    private string myNamespace;

    #region Overrides of TreeElement

    public override PsiLanguageType Language
    {
      get { return LexLanguage.Instance; }
    }

    public string Namespace
    {
      get { return myNamespace; }
    }

    public override bool IsValid()
    {
      //todo!!!!!!!!!!!
      return true;
    }
    #endregion

    public void CollectOptions()
    {
    }
  }
}
