using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Goto;
using JetBrains.ReSharper.Feature.Services.Goto.GotoProviders;
using JetBrains.ReSharper.Feature.Services.Occurences;
using JetBrains.ReSharper.Feature.Services.Search;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.PsiPlugin.Cache;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Feature.Finding.GotoMember
{
  [FeaturePart]
  public class PsiGotoFileMemberProvider : IGotoFileMemberProvider
  {

    public IEnumerable<MatchingInfo> FindMatchingInfos(IdentifierMatcher matcher, INavigationScope scope, CheckForInterrupt checkCancelled, GotoContext gotoContext)
    {
      var fileMemberScope = scope as FileMemberNavigationScope;
      if (fileMemberScope == null)
        return EmptyList<MatchingInfo>.InstanceList;

      var primaryMembersData = GetPrimaryMembers(fileMemberScope);

      ICollection<ClrFileMemberData> secondaryMembersData = new Collection<ClrFileMemberData>();
      if (scope.ExtendedSearchFlag == LibrariesFlag.SolutionAndLibraries)
      {
        var secondaryFilesGetter = fileMemberScope.GetSecondaryFilesGetter();
        secondaryMembersData = GetSecondaryMembers(secondaryFilesGetter);
      }

      var clrFileMembersMap = new ClrFileMembersMap();

      var result = new Collection<MatchingInfo>();
      foreach (var data in primaryMembersData.Concat(secondaryMembersData))
      {
        var quickSearchTexts = GetQuickSearchTexts(data.Element);
        var matchedText = quickSearchTexts.FirstOrDefault(tuple => matcher.Matches(tuple.A));
        if (matchedText == null)
          continue;

        clrFileMembersMap.Add(matchedText.A, data);

        var matchingIndicies = matchedText.B ? matcher.MatchingIndicies(matchedText.A) : EmptyArray<IdentifierMatch>.Instance;
        result.Add(new MatchingInfo(matchedText.A, matcher.Filter == "*" ? EmptyList<IdentifierMatch>.InstanceList : matchingIndicies, matchedText.B));
      }

      gotoContext.PutData(ClrFileMembersMap.ClrFileMembersMapKey, clrFileMembersMap);
      return result;
    }

    protected virtual bool IsSourceFileAvailable(IPsiSourceFile sourceFile)
    {
      //return true;
      return sourceFile.IsValid();
    }

    public virtual bool IsApplicable(INavigationScope scope, GotoContext gotoContext)
    {
      return true;
    }

    public IEnumerable<IOccurence> GetOccurencesByMatchingInfo(MatchingInfo navigationInfo, INavigationScope scope, GotoContext gotoContext)
    {
      var fileMembersMap = gotoContext.GetData(ClrFileMembersMap.ClrFileMembersMapKey);
      if (fileMembersMap == null)
        yield break;

      var membersData = fileMembersMap[navigationInfo.Identifier];
      foreach (var clrFileMemberData in membersData)
      {
        var occurence = CreateOccurence(clrFileMemberData);
        if (occurence != null)
          yield return occurence;
      }
    }

    [CanBeNull]
    protected IOccurence CreateOccurence(ClrFileMemberData clrFileMemberData)
    {
      if (!clrFileMemberData.IsInRelatedFile)
      {
        return new DeclaredElementOccurence(clrFileMemberData.Element,
          new OccurencePresentationOptions
          {
            ContainerStyle = !(clrFileMemberData.Element is ITypeElement) ? clrFileMemberData.ContainerDisplayStyle : ContainerDisplayStyle.NoContainer,
            LocationStyle = GlobalLocationStyle.None
          });
      }
      return new DeclaredElementOccurence(clrFileMemberData.Element,
        new OccurencePresentationOptions
          {
            ContainerStyle = !(clrFileMemberData.Element is ITypeElement) ? clrFileMemberData.ContainerDisplayStyle : ContainerDisplayStyle.NoContainer,
            LocationStyle = GlobalLocationStyle.RelatedFile
          });
    }

    private IEnumerable<ClrFileMemberData> GetPrimaryMembers(FileMemberNavigationScope fileMemberScope)
    {
      var primarySourceFile = fileMemberScope.GetPrimarySourceFile();
      if (!IsSourceFileAvailable(primarySourceFile))
        return EmptyList<ClrFileMemberData>.InstanceList;

      var psiSymbols = GetPsiSourceFileTypeElements(primarySourceFile);

      var psiManager = primarySourceFile.GetSolution().GetComponent<PsiManager>();
      var psiFile = psiManager.GetPrimaryPsiFile(primarySourceFile) as PsiFile;
      var primaryMembers = new LinkedList<ClrFileMemberData>();
      foreach(var symbol in psiSymbols)
      {
        if (psiFile != null)
        {
          var declaredElements = psiFile.GetDeclaredElements(symbol.Name);
          foreach (var declaredElement in declaredElements)
          {
            primaryMembers.AddFirst(new ClrFileMemberData(declaredElement, false, ContainerDisplayStyle.NoContainer));
          }
        }
      }

      return primaryMembers;
    }

    private IEnumerable<IPsiSymbol> GetPsiSourceFileTypeElements(IPsiSourceFile primarySourceFile)
    {
      var solution = primarySourceFile.GetSolution();
      var typeElements = solution.GetComponent<PsiCache>().GetSymbolsDeclaredInFile(primarySourceFile);
      return typeElements.ToList();
    }

    protected ICollection<ClrFileMemberData> GetSecondaryMembers(Func<ICollection<IProjectFile>> secondaryProjectFilesGetter)
    {
      var secondaryProjectFiles = secondaryProjectFilesGetter();
      ICollection<ClrFileMemberData> secondaryMembersData = new Collection<ClrFileMemberData>();
      foreach (var sourceFile in secondaryProjectFiles.SelectNotNull(file => file.ToSourceFile()))
      {
        if (!IsSourceFileAvailable(sourceFile))
          continue;

        var sourceFileTypeElements = TypeMemberNavigationUtil.GetPsiSourceFileTypeElements(sourceFile).ToList();
        var containerDisplayStyle = sourceFileTypeElements.OfType<IClass>().HasMoreThan(1) ? ContainerDisplayStyle.ContainingType : ContainerDisplayStyle.NoContainer;
        ICollection<IDeclaredElement> sourceFileMembers = new Collection<IDeclaredElement>();
        foreach (var sourceFileTypeElement in sourceFileTypeElements)
        {
          IPsiSourceFile file = sourceFile;
          sourceFileMembers = TypeMemberNavigationUtil.GetFilteredTypeMembers(sourceFileTypeElement,
            (member, typeElement) => IsValidMemberOfSourceFile(file, member), null)
          .Cast<IDeclaredElement>().ToList();
        }

        var elementsAndMembers = sourceFileMembers.Concat(sourceFileTypeElements);
        secondaryMembersData.AddRange(elementsAndMembers.Select(element => new ClrFileMemberData(element, true, containerDisplayStyle)));
      }

      return secondaryMembersData;
    }

    private bool IsIndexer()
    {
      return false;
    }

    private bool IsValidMemberOfSourceFile(IPsiSourceFile sourceFile, IDeclaredElement typeMember)
    {
      if (typeMember is ICompiledElement)
        return true;

      if (!typeMember.HasDeclarationsIn(sourceFile))
        return false;

      if (typeMember.GetDeclarationsIn(sourceFile)
            .Any(declaration => declaration.GetDocumentRange().IsValid() && declaration.GetNavigationRange().IsValid()))
        return true;

      return false;
    }

    private IEnumerable<JetTuple<string, bool>> GetQuickSearchTexts(IDeclaredElement declaredElement)
    {
      var ctor = declaredElement as IConstructor;
      if (ctor != null)
      {
        var cDotName = ctor.IsStatic ? JetTuple.Of(".cctor", false) : JetTuple.Of(".ctor", false);
        var quickSearchTexts = new Collection<JetTuple<string, bool>> { cDotName, JetTuple.Of("new", false) };

        var containingType = ctor.GetContainingType();
        if (containingType != null)
          quickSearchTexts.Add(JetTuple.Of(containingType.ShortName, true));

        var cName = ctor.IsStatic ? JetTuple.Of("cctor", false) : JetTuple.Of("ctor", false);
        quickSearchTexts.Add(cName);

        return quickSearchTexts;
      }

      if (IsIndexer())
        return new[] { JetTuple.Of("this", false) };

      if ((declaredElement as IMethod).IsOverridesObjectFinalize())
        return new[] { JetTuple.Of(declaredElement.ShortName, false), JetTuple.Of("~", false) };

      var oper = declaredElement as IOperator;
      if (oper != null)
        return new[] { JetTuple.Of(oper.ShortName, true), JetTuple.Of("operator", false) };

      var evt = declaredElement as IEvent;
      if (evt != null)
        return new[] { JetTuple.Of(evt.ShortName, true), JetTuple.Of("event", false) };

      return new[] { JetTuple.Of(declaredElement.ShortName, true) };
    }

    protected class ClrFileMemberData
    {
      private readonly IDeclaredElement myElement;
      private readonly bool myIsInRelatedFile;
      private readonly ContainerDisplayStyle myDisambigStyle;

      public ClrFileMemberData(IDeclaredElement element, bool isInRelatedFile, ContainerDisplayStyle disambigStyle)
      {
        myElement = element;
        myIsInRelatedFile = isInRelatedFile;
        myDisambigStyle = disambigStyle;
      }

      public IDeclaredElement Element
      {
        get { return myElement; }
      }

      public bool IsInRelatedFile
      {
        get { return myIsInRelatedFile; }
      }

      public ContainerDisplayStyle ContainerDisplayStyle
      {
        get { return myDisambigStyle; }
      }
    }

    private class ClrFileMembersMap : OneToSetMap<string, ClrFileMemberData>
    {
      public static readonly Key<ClrFileMembersMap> ClrFileMembersMapKey = new Key<ClrFileMembersMap>("ClrFileMembersMap");
    }
  }
}
