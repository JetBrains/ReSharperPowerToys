using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.LexPlugin.Grammar;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree.Impl
{
  internal partial class LexFile
  {
    private string myNamespace;
    private ISymbolTable myTokenSymbolTable;
    private readonly Dictionary<string, IDeclaredElement> myDeclarations = new Dictionary<string, IDeclaredElement>();

    #region Overrides of TreeElement

    public override PsiLanguageType Language
    {
      get { return LexLanguage.Instance; }
    }

    public string Namespace
    {
      get { return myNamespace; }
    }

    public ISymbolTable FileTokenSymbolTable
    {
      get
      {
        if (myTokenSymbolTable != null)
        {
          return myTokenSymbolTable;
        }
        lock (this)
        {
          return myTokenSymbolTable ?? (myTokenSymbolTable = CreateTokenSymbolTable());
        }
      }
    }

    private ISymbolTable CreateTokenSymbolTable()
    {
      CollectDeclarations();
      if (GetSourceFile() != null)
      {
        Dictionary<string, IDeclaredElement>.ValueCollection elements = myDeclarations.Values;
        myTokenSymbolTable = ResolveUtil.CreateSymbolTable(elements, 0);
      }
      else
      {
        myTokenSymbolTable = null;
      }
      return myTokenSymbolTable;
    }

    private void CollectDeclarations()
    {
      CollectDeclarations(this);
    }

    private void CollectDeclarations(ITreeNode treeNode)
    {
      if(treeNode is ITokenDeclaration)
      {
        var declaration = treeNode as IDeclaration;
        string s = declaration.DeclaredName;
        myDeclarations[s] = declaration.DeclaredElement;        
      } else
      {
        var child = treeNode.FirstChild;
        while(child != null)
        {
          CollectDeclarations(child);
          child = child.NextSibling;
        }
      }
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
