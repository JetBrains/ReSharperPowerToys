using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Cache
{
  internal class PsiCacheBuilder : IRecursiveElementProcessor
  {
    private readonly List<IPsiSymbol> mySymbols = new List<IPsiSymbol>();
    private readonly IPsiSourceFile mySourceFile;

    private PsiCacheBuilder(IPsiSourceFile sourceFile)
    {
      mySourceFile = sourceFile;
    }

    public void VisitOptionDefinition(IOptionDefinition optionDefinition)
    {
      var name = optionDefinition.OptionName.GetText();
      var offset = optionDefinition.GetNavigationRange().TextRange.StartOffset;
      var psiSourceFile = optionDefinition.GetSourceFile();
      string value = "";
      var valueNode = optionDefinition.OptionStringValue;
      if(valueNode != null)
      {
        value = valueNode.GetText();
        if("\"".Equals(value.Substring(0,1)))
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
      var name = ruleDeclaration.DeclaredName;
      var offset = ruleDeclaration.GetNavigationRange().TextRange.StartOffset;
      var psiSourceFile = ruleDeclaration.GetSourceFile();
      mySymbols.Add(new PsiRuleSymbol(name, offset, psiSourceFile));
    }

    private ICollection<IPsiSymbol> GetSymbols()
    {
      return mySymbols;
    }

    public static CachePair Read(BinaryReader reader, IPsiSourceFile sourceFile)
    {
      var ruleData = ReadRules(reader, sourceFile);
      var optionData = ReadOptions(reader, sourceFile);

      return new CachePair(ruleData, optionData);
    }

    private static IList<PsiRuleSymbol> ReadRules(BinaryReader reader, IPsiSourceFile sourceFile)
    {
      var count = reader.ReadInt32();
      var ret = new List<PsiRuleSymbol>();

      for (int i = 0; i < count; i++)
      {
        var symbol = new PsiRuleSymbol(sourceFile);
        symbol.Read(reader);
        ret.Add(symbol);
      }

      return ret;
    }

    private static IList<PsiOptionSymbol> ReadOptions(BinaryReader reader, IPsiSourceFile sourceFile)
    {
      var count = reader.ReadInt32();
      var ret = new List<PsiOptionSymbol>();

      for (int i = 0; i < count; i++)
      {
        var symbol = new PsiOptionSymbol(sourceFile);
        symbol.Read(reader);
        ret.Add(symbol);
      }

      return ret;
    }

    public static void Write(CachePair pair, BinaryWriter writer)
    {
      var ruleItems = pair.Rules;
      writer.Write(ruleItems.Count);

      foreach (var ruleItem in ruleItems)
      {
        ruleItem.Write(writer);
      }

      var optionItems = pair.Options;
      writer.Write(optionItems.Count);

      foreach (var optionItem in optionItems)
      {
        optionItem.Write(writer);
      }
    }

    [CanBeNull]
    public static ICollection<IPsiSymbol> Build(IPsiSourceFile sourceFile)
    {
      var file = sourceFile.GetPsiFile<PsiLanguage>() as IPsiFile;
      if (file == null)
        return null;
      return Build(sourceFile, file);
    }

    [CanBeNull]
    private static ICollection<IPsiSymbol> Build(IPsiSourceFile sourceFile, IPsiFile file)
    {
      var ret = new PsiCacheBuilder(sourceFile);
      file.ProcessDescendants(ret);
      return ret.GetSymbols();
    }

    public bool InteriorShouldBeProcessed(ITreeNode element)
    {
      return true;
    }

    public void ProcessBeforeInterior(ITreeNode element)
    {
      var optionDefinition = element as IOptionDefinition;
      if(optionDefinition != null)
      {
        VisitOptionDefinition(optionDefinition);
        return;
      }
      var ruleDeclaration = element as IRuleDeclaration;
      if(ruleDeclaration != null)
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
  }
}
