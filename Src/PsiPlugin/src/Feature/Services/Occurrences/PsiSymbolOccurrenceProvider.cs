using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Occurences;
using JetBrains.ReSharper.Feature.Services.Search;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.PsiPlugin.Cache;
using JetBrains.ReSharper.PsiPlugin.Feature.Finding;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services.Occurrences
{
  [OccurenceProvider(Priority = 3)]
  public class PsiSymbolOccurrenceProvider : IOccurenceProvider
  {
    public IOccurence MakeOccurence(FindResult findResult)
    {
      var symbolFindResult = findResult as FindResultPsiSymbol;
      if (symbolFindResult != null)
        return MakeOccurence(symbolFindResult.Symbol);
      return null;
    }

    public static IOccurence MakeOccurence(IPsiSymbol symbol)
    {
      return MakeOccurence(symbol, new OccurencePresentationOptions{TextDisplayStyle = TextDisplayStyle.ContainingFile});
    }

    public static IOccurence MakeOccurence(IPsiSymbol symbol, OccurencePresentationOptions occurencePresentationOptions)
    {
      var projectFile = symbol.SourceFile.ToProjectFile();
      if (projectFile != null)
      {
        //var psiFile = symbol.SourceFile.GetPsiFile<JavaScriptLanguage>();
        //Assertion.Assert(psiFile != null, "psiFile != null");
        var offset = symbol.Offset;
        var documentRange = new DocumentRange(symbol.SourceFile.Document, new TextRange(offset, offset + symbol.Name.Length));
        {
          return new PsiSymbolOccurrence(symbol.SourceFile.GetSolution(), documentRange, occurencePresentationOptions);
        }
      }
      return null;
    }
  }

  public class PsiSymbolOccurrence : RangeOccurence
  {
    public PsiSymbolOccurrence(ISolution solution, DocumentRange documentRange, OccurencePresentationOptions options) : base(solution, documentRange, options)
    {
    }

  }
}
