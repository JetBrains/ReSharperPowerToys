using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Finder;
using JetBrains.ReSharper.Psi.Impl.Search;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services.FindUsges
{
  internal class PsiReferenceSearcher : IDomainSpecificSearcher
  {
    private readonly HashSet<IDeclaredElement> myElements;
    private readonly HashSet<string> myNames;
    private readonly bool mySearchForLateBound;
    private readonly bool myHasUnnamedElement;

    public PsiReferenceSearcher(IDomainSpecificSearcherFactory searchWordsProvider, IEnumerable<IDeclaredElement> elements, bool findCandidates, bool searchForLateBound)
    {
      mySearchForLateBound = searchForLateBound;
      myElements = new HashSet<IDeclaredElement>(elements);

      myNames = new HashSet<string>();
      foreach (var element in myElements)
      {
        foreach (var name in searchWordsProvider.GetAllPossibleWordsInFile(element))
        {
          if (string.IsNullOrEmpty(name))
          {
            myHasUnnamedElement = true;
            continue;
          }
          myNames.Add(name);
        }

        var shortName = element.ShortName;
        if (!string.IsNullOrEmpty(shortName))
          myNames.Add(shortName);
      }
    }

    public bool ProcessProjectItem<TResult>(IPsiSourceFile sourceFile, IFindResultConsumer<TResult> consumer)
    {
      if (myElements.All(element => !CanContainReferencesTo(sourceFile, element)))
        return false;

      IFile psiFile = sourceFile.GetPsiFile<PsiLanguage>();
      return psiFile != null && ProcessElement(psiFile, consumer);
    }

    public bool ProcessElement<TResult>(ITreeNode element, IFindResultConsumer<TResult> consumer)
    {

      Assertion.Assert(element != null, "The condition (element != null) is false.");

      var names = new JetHashSet<string>(myNames);

      FindExecution result;

      if (mySearchForLateBound)
      result = new LateBoundReferenceSourceFileProcessor<TResult>(element, consumer, myElements, myHasUnnamedElement ? null : names, names).Run();
      else
      result = new ReferenceSearchSourceFileProcessor<TResult>(element, true, consumer, myElements, myHasUnnamedElement ? null : names, names).Run();
      return result == FindExecution.Stop;
    }

    private bool CanContainReferencesTo(IPsiSourceFile sourceFile, IDeclaredElement element)
    {
      var wordIndex = element.GetPsiServices().CacheManager.WordIndex;

      var field = element as IField;
      if (field != null && field.IsEnumMember)
      {
        if (!wordIndex.CanContainWord(sourceFile, field.ShortName))
          return false;

        var containingType = field.GetContainingType();
        if (containingType != null && !wordIndex.CanContainWord(sourceFile, containingType.ShortName))
          return false;

        return true;
      }

      var typeMember = element as ITypeMember;
      if (typeMember != null && typeMember.IsStatic && !typeMember.IsExtensionMethod() && !(typeMember is IOperator)
        /* && sourceFile.PrimaryPsiLanguage.Is<PsiLanguage>() or && !typeMember.GetSourceFiles().Contains(sourceFile)*/)
      {
        var containingType = typeMember.GetContainingType();
        if (containingType == null)
          return true;

        if (!wordIndex.CanContainWord(sourceFile, typeMember.ShortName))
          return false;

        var typeClrName = containingType.GetClrName();
        if (typeClrName.Equals(PredefinedType.OBJECT_FQN)) 
          return true;


        if (((IModifiersOwner) containingType).IsSealed)
          return wordIndex.CanContainWord(sourceFile, containingType.ShortName);

        return FinderUtil.GetInheritorsClosure(containingType).First.Any(name => wordIndex.CanContainWord(sourceFile, name));
      }

      return true;
    }
  }  
}
