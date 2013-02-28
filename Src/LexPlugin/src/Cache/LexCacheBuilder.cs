using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.LexPlugin.Cache;
using JetBrains.ReSharper.LexPlugin.Grammar;
using JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.LexPlugin.Cache
{
  class LexCacheBuilder : IRecursiveElementProcessor
  {
    private readonly List<ILexSymbol> mySymbols = new List<ILexSymbol>();

    public bool InteriorShouldBeProcessed(ITreeNode element)
    {
      return true;
    }

    public void ProcessBeforeInterior(ITreeNode element)
    {
      var optionDefinition = element as IStateDeclaration;
      if (optionDefinition != null)
      {
        VisitStateDefinition(optionDefinition);
        return;
      }
      var ruleDeclaration = element as ITokenDeclaration;
      if (ruleDeclaration != null)
      {
        VisitTokenDeclaration(ruleDeclaration);
      }
      var includeStatement = element as IIncludeStatement;
      if(includeStatement != null)
      {
        VisitIncludeStatement(includeStatement);
      }
    }

    public void ProcessAfterInterior(ITreeNode element)
    {
    }

    public bool ProcessingIsFinished
    {
      get { return false; }
    }

    private void VisitStateDefinition(IStateDeclaration stateDeclaration)
    {
      string name = stateDeclaration.StateName.GetText();
      int offset = stateDeclaration.GetNavigationRange().TextRange.StartOffset;
      IPsiSourceFile psiSourceFile = stateDeclaration.GetSourceFile();

      mySymbols.Add(new LexStateSymbol(name, offset, psiSourceFile));
    }

    private void VisitTokenDeclaration(ITokenDeclaration tokenDeclaration)
    {
      string name = tokenDeclaration.DeclaredName;
      int offset = tokenDeclaration.GetNavigationRange().TextRange.StartOffset;
      IPsiSourceFile psiSourceFile = tokenDeclaration.GetSourceFile();
      mySymbols.Add(new LexTokenSymbol(name, offset, psiSourceFile));
    }

    private void VisitIncludeStatement(IIncludeStatement includeStatement)
    {
      var pathId = includeStatement.PathId;
      var child = pathId.LastChild;
      var fileExt = child as IFileExt;
      string name = "";
      int offset = -1;
      if (fileExt != null)
      {
        var sibling = fileExt.PrevSibling.PrevSibling;
        string fileName = sibling.GetText() + "." + fileExt.GetText();
        name = fileName;
        offset = sibling.GetNavigationRange().TextRange.StartOffset;
      }
      IPsiSourceFile psiSourceFile = includeStatement.GetSourceFile();

      mySymbols.Add(new LexIncludeFileSymbol(name, offset, psiSourceFile));
    }

    private ICollection<ILexSymbol> GetSymbols()
    {
      return mySymbols;
    }

    public static CacheData Read(BinaryReader reader, IPsiSourceFile sourceFile)
    {
      IList<LexTokenSymbol> tokenData = ReadRules(reader, sourceFile);
      IList<LexStateSymbol> stateData = ReadOptions(reader, sourceFile);
      IList<LexIncludeFileSymbol> includeFileData = ReadIncludeFile(reader, sourceFile);

      return new CacheData(tokenData, stateData, includeFileData);
    }

    private static IList<LexTokenSymbol> ReadRules(BinaryReader reader, IPsiSourceFile sourceFile)
    {
      int count = reader.ReadInt32();
      var ret = new List<LexTokenSymbol>();

      for (int i = 0; i < count; i++)
      {
        var symbol = new LexTokenSymbol(sourceFile);
        symbol.Read(reader);
        ret.Add(symbol);
      }

      return ret;
    }

    private static IList<LexStateSymbol> ReadOptions(BinaryReader reader, IPsiSourceFile sourceFile)
    {
      int count = reader.ReadInt32();
      var ret = new List<LexStateSymbol>();

      for (int i = 0; i < count; i++)
      {
        var symbol = new LexStateSymbol(sourceFile);
        symbol.Read(reader);
        ret.Add(symbol);
      }

      return ret;
    }

    private static IList<LexIncludeFileSymbol> ReadIncludeFile(BinaryReader reader, IPsiSourceFile sourceFile)
    {
      int count = reader.ReadInt32();
      var ret = new List<LexIncludeFileSymbol>();

      for (int i = 0; i < count; i++)
      {
        var symbol = new LexIncludeFileSymbol(sourceFile);
        symbol.Read(reader);
        ret.Add(symbol);
      }

      return ret;
    }

    public static void Write(CacheData data, BinaryWriter writer)
    {
      IList<LexTokenSymbol> ruleItems = data.Tokens;
      writer.Write(ruleItems.Count);

      foreach (LexTokenSymbol ruleItem in ruleItems)
      {
        ruleItem.Write(writer);
      }

      IList<LexStateSymbol> optionItems = data.States;
      writer.Write(optionItems.Count);

      foreach (LexStateSymbol optionItem in optionItems)
      {
        optionItem.Write(writer);
      }

      IList<LexIncludeFileSymbol> includeFileItems = data.IncludeFiles;
      writer.Write(optionItems.Count);

      foreach (LexIncludeFileSymbol optionItem in includeFileItems)
      {
        optionItem.Write(writer);
      }
    }

    [CanBeNull]
    public static ICollection<ILexSymbol> Build(IPsiSourceFile sourceFile)
    {
      var file = sourceFile.GetPsiFile<LexLanguage>(new DocumentRange(sourceFile.Document, 0)) as ILexFile;
      if (file == null)
      {
        return null;
      }
      return Build(file);
    }

    [CanBeNull]
    private static ICollection<ILexSymbol> Build(ILexFile file)
    {
      var ret = new LexCacheBuilder();
      file.ProcessDescendants(ret);
      return ret.GetSymbols();
    }
  }
}
