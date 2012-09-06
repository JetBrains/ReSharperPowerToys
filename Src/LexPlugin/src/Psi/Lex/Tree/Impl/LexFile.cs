using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.LexPlugin.Grammar;
using JetBrains.ReSharper.LexPlugin.Resolve;
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
    private ISymbolTable myStateSymbolTable;
    private readonly Dictionary<string, IDeclaredElement> myTokenDeclarations = new Dictionary<string, IDeclaredElement>();
    private readonly Dictionary<string, IDeclaredElement> myStateDeclarations = new Dictionary<string, IDeclaredElement>();

    protected override void ClearCachedData()
    {
      base.ClearCachedData();
      myTokenSymbolTable = null;
      myStateSymbolTable = null;
      myTokenDeclarations.Clear();
      myStateDeclarations.Clear();
    }

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
      CollectTokenDeclarations();
      if (GetSourceFile() != null)
      {
        IList<IDeclaredElement> elements = myTokenDeclarations.Values.ToList();
        myTokenSymbolTable = ResolveUtil.CreateSymbolTable(elements, 0);
      }
      else
      {
        myTokenSymbolTable = null;
      }
      return myTokenSymbolTable;
    }

    private void CollectTokenDeclarations()
    {
      CollectTokenDeclarations(this);
    }

    private void CollectTokenDeclarations(ITreeNode treeNode)
    {
      if(treeNode is ITokenDeclaration)
      {
        var declaration = treeNode as IDeclaration;
        string s = declaration.DeclaredName;
        myTokenDeclarations[s] = declaration.DeclaredElement;        
      } else
      {
        var child = treeNode.FirstChild;
        while(child != null)
        {
          CollectTokenDeclarations(child);
          child = child.NextSibling;
        }
      }
    }

    public ISymbolTable FileStateSymbolTable
    {
      get
      {
        if (myStateSymbolTable != null)
        {
          return myStateSymbolTable;
        }
        lock (this)
        {
          return myStateSymbolTable ?? (myStateSymbolTable = CreateStateSymbolTable());
        }
      }
    }

    private ISymbolTable CreateStateSymbolTable()
    {
      CollectStateDeclarations();
      if (GetSourceFile() != null)
      {
        IList<IDeclaredElement> elements = myStateDeclarations.Values.ToList();
        elements.Add(new InitialStateDeclaredElement(GetSourceFile(), GetPsiServices()));
        myStateSymbolTable = ResolveUtil.CreateSymbolTable(elements, 0);
      }
      else
      {
        myStateSymbolTable = null;
      }
      return myStateSymbolTable;
    }

    private void CollectStateDeclarations()
    {
      CollectStateDeclarations(this);
    }

    private void CollectStateDeclarations(ITreeNode treeNode)
    {
      if (treeNode is IStateDeclaration)
      {
        var declaration = treeNode as IDeclaration;
        string s = declaration.DeclaredName;
        myStateDeclarations[s] = declaration.DeclaredElement;
      }
      else
      {
        var child = treeNode.FirstChild;
        while (child != null)
        {
          CollectStateDeclarations(child);
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
