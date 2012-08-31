using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Cache
{
  internal class PsiCacheBuilder : IRecursiveElementProcessor
  {
    private readonly List<IPsiSymbol> mySymbols = new List<IPsiSymbol>();
    
    public bool InteriorShouldBeProcessed(ITreeNode element)
    {
      return true;
    }

    public void ProcessBeforeInterior(ITreeNode element)
    {
      var optionDefinition = element as IOptionDefinition;
      if (optionDefinition != null)
      {
        VisitOptionDefinition(optionDefinition);
        return;
      }
      var ruleDeclaration = element as IRuleDeclaration;
      if (ruleDeclaration != null)
      {
        VisitRuleDeclaration(ruleDeclaration);
      }
    }

    public void ProcessAfterInterior(ITreeNode element)
    {
    }

    public bool ProcessingIsFinished
    {
      get { return false; }
    }

    private void VisitOptionDefinition(IOptionDefinition optionDefinition)
    {
      string name = optionDefinition.OptionName.GetText();
      int offset = optionDefinition.GetNavigationRange().TextRange.StartOffset;
      IPsiSourceFile psiSourceFile = optionDefinition.GetSourceFile();
      string value = "";
      IOptionStringValue valueNode = optionDefinition.OptionStringValue;
      if (valueNode != null)
      {
        value = valueNode.GetText();
        if ("\"".Equals(value.Substring(0, 1)))
        {
          value = value.Substring(1, value.Length - 1);
        }
        if ("\"".Equals(value.Substring(value.Length - 1, 1)))
        {
          value = value.Substring(0, value.Length - 1);
        }
      }
      mySymbols.Add(new PsiOptionSymbol(name, offset, value, psiSourceFile));
    }

    private void VisitRuleDeclaration(IRuleDeclaration ruleDeclaration)
    {
      string name = ruleDeclaration.DeclaredName;
      int offset = ruleDeclaration.GetNavigationRange().TextRange.StartOffset;
      IPsiSourceFile psiSourceFile = ruleDeclaration.GetSourceFile();
      mySymbols.Add(new PsiRuleSymbol(name, offset, psiSourceFile));
    }

    private ICollection<IPsiSymbol> GetSymbols()
    {
      return mySymbols;
    }

    public static CachePair Read(BinaryReader reader, IPsiSourceFile sourceFile)
    {
      IList<PsiRuleSymbol> ruleData = ReadRules(reader, sourceFile);
      IList<PsiOptionSymbol> optionData = ReadOptions(reader, sourceFile);

      return new CachePair(ruleData, optionData);
    }

    private static IList<PsiRuleSymbol> ReadRules(BinaryReader reader, IPsiSourceFile sourceFile)
    {
      int count = reader.ReadInt32();
      var ret = new List<PsiRuleSymbol>();

      for (int i = 0 ; i < count ; i++)
      {
        var symbol = new PsiRuleSymbol(sourceFile);
        symbol.Read(reader);
        ret.Add(symbol);
      }

      return ret;
    }

    private static IList<PsiOptionSymbol> ReadOptions(BinaryReader reader, IPsiSourceFile sourceFile)
    {
      int count = reader.ReadInt32();
      var ret = new List<PsiOptionSymbol>();

      for (int i = 0 ; i < count ; i++)
      {
        var symbol = new PsiOptionSymbol(sourceFile);
        symbol.Read(reader);
        ret.Add(symbol);
      }

      return ret;
    }

    public static void Write(CachePair pair, BinaryWriter writer)
    {
      IList<PsiRuleSymbol> ruleItems = pair.Rules;
      writer.Write(ruleItems.Count);

      foreach (PsiRuleSymbol ruleItem in ruleItems)
      {
        ruleItem.Write(writer);
      }

      IList<PsiOptionSymbol> optionItems = pair.Options;
      writer.Write(optionItems.Count);

      foreach (PsiOptionSymbol optionItem in optionItems)
      {
        optionItem.Write(writer);
      }
    }

    [CanBeNull]
    public static ICollection<IPsiSymbol> Build(IPsiSourceFile sourceFile)
    {
      var file = sourceFile.GetPsiFile<PsiLanguage>(new DocumentRange(sourceFile.Document, 0)) as IPsiFile;
      if (file == null)
      {
        return null;
      }
      return Build(file);
    }

    [CanBeNull]
    private static ICollection<IPsiSymbol> Build(IPsiFile file)
    {
      var ret = new PsiCacheBuilder();
      file.ProcessDescendants(ret);
      return ret.GetSymbols();
    }
  }
}
