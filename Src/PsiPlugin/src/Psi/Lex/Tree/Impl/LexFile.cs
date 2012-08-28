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
      var definitions = DefinitionBlock;
      var options = definitions.Options;
      var child = options.FirstChild;
      while(child != null)
      {
        if(child is ILexerOption)
        {
          var node = child.FirstChild;
          while(node != null)
          {
            if(node is INamespaceLexerOption)
            {
              myNamespace = (node as INamespaceLexerOption).Namespace.GetText();
            }
            node = node.NextSibling;
          }
        }
        child = child.NextSibling;
      }
    }
  }
}
