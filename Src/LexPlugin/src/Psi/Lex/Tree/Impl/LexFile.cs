using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.LexPlugin.Cache;
using JetBrains.ReSharper.LexPlugin.Grammar;
using JetBrains.ReSharper.LexPlugin.Resolve;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree.Impl
{
  internal partial class LexFile
  {
    private string myNamespace;
    private IList<string> myIncludedFiles = new List<string>();
    private ISymbolTable myTokenSymbolTable;
    private ISymbolTable myStateSymbolTable;
    private IList<IPsiSourceFile> myAllFiles; 
    private readonly Dictionary<string, IDeclaredElement> myTokenDeclarations = new Dictionary<string, IDeclaredElement>();
    private readonly Dictionary<string, IDeclaredElement> myStateDeclarations = new Dictionary<string, IDeclaredElement>();

    protected override void ClearCachedData()
    {
      base.ClearCachedData();
      myTokenSymbolTable = null;
      myStateSymbolTable = null;
      myTokenDeclarations.Clear();
      myStateDeclarations.Clear();
      myAllFiles = null;
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

    public IList<IPsiSourceFile> AllFiles
    {
      get
      {
        if (myAllFiles != null)
        {
          return myAllFiles;
        }
        lock (this)
        {
          return myAllFiles ?? (myAllFiles = CollectAllFiles());
        }
      }
    }

    private IList<IPsiSourceFile> CollectAllFiles()
    {
      var cache = GetPsiServices().Solution.GetComponent<LexCache>();
      myAllFiles = cache.GetAllFiles().ToList();
      return myAllFiles;
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
      var cache = GetPsiServices().Solution.GetComponent<LexCache>();
      IList<ILexSymbol> symbols = new List<ILexSymbol>();

      if (GetSourceFile() != null)
      {
        CollectTokensInRelatedFiles(cache, symbols);
        IList<IDeclaredElement> elements = myTokenDeclarations.Values.ToList();
        foreach (var lexSymbol in symbols)
        {
          if (lexSymbol.SourceFile != GetSourceFile())
          {
            ITreeNode element =
              lexSymbol.SourceFile.GetPsiFile<LexLanguage>(new DocumentRange(lexSymbol.SourceFile.Document, 0)).FindNodeAt(new TreeTextRange(new TreeOffset(lexSymbol.Offset), 1));
            var tokenDeclaration = element.GetContainingNode<ITokenDeclaration>();
            if (tokenDeclaration != null)
            {
              var declareedElement = tokenDeclaration.DeclaredElement;
              elements.Add(declareedElement);
            }
          }
        }
        myTokenSymbolTable = ResolveUtil.CreateSymbolTable(elements, 0);
      }
      else
      {
        myTokenSymbolTable = null;
      }
      return myTokenSymbolTable;
    }

    private void CollectTokensInRelatedFiles(LexCache cache, IList<ILexSymbol> symbols)
    {
      var includedFiles = GetRelatedFiles(cache);
      foreach (var lexIncludeFileSymbol in includedFiles)
      {
        var symbolsInFile = cache.GetTokenSymbolsDeclaredInFile(lexIncludeFileSymbol);
        foreach (var lexTokenSymbol in symbolsInFile)
        {
          symbols.Add(lexTokenSymbol);
        }
      }
    }

    private IEnumerable<IPsiSourceFile> GetRelatedFiles(LexCache cache)
    {
      IList<IPsiSourceFile> includedFiles = new List<IPsiSourceFile>();
      //GetIncludedFiles(includedFiles, cache, GetSourceFile());


      IList<IPsiSourceFile> usingFiles = new List<IPsiSourceFile>();
      GetFilesUsing(cache, GetSourceFile(), usingFiles);
      usingFiles.Add(GetSourceFile());
      foreach (var psiSourceFile in usingFiles)
      {
        GetIncludedFiles(includedFiles, cache, psiSourceFile);
      }
      return includedFiles;
    }

    private void GetFilesUsing(LexCache cache, IPsiSourceFile psiSourceFile, IList<IPsiSourceFile> usingFiles)
    {
      var allSymbols = cache.GetAllIncludeFileSymbols();
      List<IPsiSourceFile> files = new List<IPsiSourceFile>();
      foreach (var allIncludeFileSymbol in allSymbols)
      {
        if(allIncludeFileSymbol.Name == psiSourceFile.Name)
        {
          if(!(files.Contains(allIncludeFileSymbol.SourceFile)))
          {
            files.Add(allIncludeFileSymbol.SourceFile);
          }
          if(!(usingFiles.Contains(allIncludeFileSymbol.SourceFile)))
          {
            usingFiles.Add(allIncludeFileSymbol.SourceFile);
          }
        }
      }
      if (files.Count() > 0)
      {
        GetFilesUsing(cache, files, usingFiles);
      }
    }

    private void GetFilesUsing(LexCache cache, List<IPsiSourceFile> psiSourceFiles, IList<IPsiSourceFile> usingFiles )
    {
      var allSymbols = cache.GetAllSymbols();
      List<IPsiSourceFile> files = new List<IPsiSourceFile>();
      foreach (var psiSourceFile in psiSourceFiles)
      {
        foreach (var allIncludeFileSymbol in allSymbols)
        {
          if (allIncludeFileSymbol.Name == psiSourceFile.Name)
          {
            if (!(files.Contains(allIncludeFileSymbol.SourceFile)))
            {
              files.Add(allIncludeFileSymbol.SourceFile);
            }
            if(!(usingFiles.Contains(allIncludeFileSymbol.SourceFile)))
            {
              usingFiles.Add(allIncludeFileSymbol.SourceFile);
            }
          }
        }
      }

      if (files.Count > 0)
      {
        GetFilesUsing(cache, files, usingFiles);
      }
    }

    private void GetIncludedFiles(IList<IPsiSourceFile> includedFiles, LexCache cache, IPsiSourceFile sourceFile)
    {
      var firstIncludedFiles = cache.GetIncludeFileSymbolsDeclaredInFile(sourceFile);
      var allFiles = cache.GetAllFiles();
      IList<IPsiSourceFile> sourceFiles = new List<IPsiSourceFile>();
      foreach (var lexIncludeFileSymbol in firstIncludedFiles)
      {
        foreach (var file in allFiles)
        {
          if(lexIncludeFileSymbol.Name == file.Name)
          {
            if (!(sourceFiles.Contains(file)) && !(includedFiles.Contains(file)))
            {
              sourceFiles.Add(file);
              includedFiles.Add(file);
            }
          }
        }
      }

      foreach (var psiSourceFile in sourceFiles)
      {
        GetIncludedFiles(includedFiles, cache, psiSourceFile);
      }

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
      var cache = GetPsiServices().Solution.GetComponent<LexCache>();

      IList<ILexSymbol> symbols = new List<ILexSymbol>();
      if (GetSourceFile() != null)
      {
        CollectStatesInRelatedFiles(cache, symbols);

        IList<IDeclaredElement> elements = myStateDeclarations.Values.ToList();
        foreach (var lexSymbol in symbols)
        {
          if (lexSymbol.SourceFile != GetSourceFile())
          {
            ITreeNode element =
              lexSymbol.SourceFile.GetPsiFile<LexLanguage>(new DocumentRange(lexSymbol.SourceFile.Document, 0)).FindNodeAt(new TreeTextRange(new TreeOffset(lexSymbol.Offset), 1));
            var stateDeclaration = element.GetContainingNode<IStateDeclaration>();
            if (stateDeclaration != null)
            {
              var declareedElement = stateDeclaration.DeclaredElement;
              elements.Add(declareedElement);
            }
          }
        }
        elements.Add(new InitialStateDeclaredElement(GetSourceFile(), GetPsiServices()));
        myStateSymbolTable = ResolveUtil.CreateSymbolTable(elements, 0);
      }
      else
      {
        myStateSymbolTable = null;
      }
      return myStateSymbolTable;
    }

    private void CollectStatesInRelatedFiles(LexCache cache, IList<ILexSymbol> symbols)
    {
      var includedFiles = GetRelatedFiles(cache);
      foreach (var lexIncludeFileSymbol in includedFiles)
      {
        var symbolsInFile = cache.GetStateSymbolsDeclaredInFile(lexIncludeFileSymbol);
        foreach (var lexStateSymbol in symbolsInFile)
        {
          symbols.Add(lexStateSymbol);
        }
      }
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

    public void CollectIncluded()
    {
      var child = FirstChild;
      while (child != null)
      {
        CollectIncluded(child);
        child = child.NextSibling;
      }
    }

    private void CollectIncluded(ITreeNode treeNode)
    {
      if(treeNode is IIncludeStatement)
      {
        var pathId = (treeNode as IIncludeStatement).PathId;
        var child = pathId.LastChild;
        var fileExt = child as IFileExt;
        if(fileExt != null)
        {
          var sibling = fileExt.PrevSibling.PrevSibling;
          string fileName = sibling.GetText() + "." + fileExt.GetText();
          myIncludedFiles.Add(fileName);
          //fileName = NameToCamelCase(fileName);
          //myIncludedFiles.Add(fileName);
        }
      } else
      {
        if ((treeNode is ILexingBlock) || (treeNode is IDefinitionBlock))
        {
          var child = treeNode.FirstChild;
          while(child != null)
          {
            CollectIncluded(child);
            child = child.NextSibling;
          }
        }
      }
    }

    private string NameToCamelCase(string s)
    {
      string firstLetter = s.Substring(0, 1);
      firstLetter = firstLetter.ToUpper();
      return firstLetter + s.Substring(1, s.Length - 1);
    }
  }
}
