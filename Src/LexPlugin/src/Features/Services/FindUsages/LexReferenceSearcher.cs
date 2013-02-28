using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.LexPlugin.Grammar;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Finder;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.LexPlugin.Features.Services.FindUsages
{
  internal class LexReferenceSearcher : IDomainSpecificSearcher
  {
    private readonly HashSet<IDeclaredElement> myElements;
    private readonly bool myHasUnnamedElement;
    private readonly HashSet<string> myNames;
    private readonly bool mySearchForLateBound;

    public LexReferenceSearcher(IDomainSpecificSearcherFactory searchWordsProvider, IEnumerable<IDeclaredElement> elements, bool searchForLateBound)
    {
      mySearchForLateBound = searchForLateBound;
      myElements = new HashSet<IDeclaredElement>(elements);

      myNames = new HashSet<string>();
      foreach (IDeclaredElement element in myElements)
      {
        foreach (string name in searchWordsProvider.GetAllPossibleWordsInFile(element))
        {
          if (string.IsNullOrEmpty(name))
          {
            myHasUnnamedElement = true;
            continue;
          }
          myNames.Add(name);
        }

        string shortName = element.ShortName;
        if (!string.IsNullOrEmpty(shortName))
        {
          myNames.Add(shortName);
        }
      }
    }

    #region IDomainSpecificSearcher Members

    public bool ProcessProjectItem<TResult>(IPsiSourceFile sourceFile, IFindResultConsumer<TResult> consumer)
    {
      if (!CanContainReferencesTo(sourceFile))
      {
        return false;
      }

      IFile psiFile = sourceFile.GetPsiFile<LexLanguage>(new DocumentRange(sourceFile.Document, 0));
      return psiFile != null && ProcessElement(psiFile, consumer);
    }

    public bool ProcessElement<TResult>(ITreeNode element, IFindResultConsumer<TResult> consumer)
    {
      Assertion.Assert(element != null, "The condition (element != null) is false.");

      var names = new JetHashSet<string>(myNames);

      FindExecution result;

      if (mySearchForLateBound)
      {
        result = new LateBoundReferenceSourceFileProcessor<TResult>(element, consumer, myElements, myHasUnnamedElement ? null : names, names).Run();
      }
      else
      {
        result = new ReferenceSearchSourceFileProcessor<TResult>(element, true, consumer, myElements, myHasUnnamedElement ? null : names, names).Run();
      }
      return result == FindExecution.Stop;
    }

    #endregion

    private bool CanContainReferencesTo(IPsiSourceFile sourceFile)
    {
      return ((Equals(sourceFile.PrimaryPsiLanguage, LexLanguage.Instance)) || (Equals(sourceFile.PrimaryPsiLanguage, CSharpLanguage.Instance)));
    }
  }
}
