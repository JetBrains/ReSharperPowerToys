using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Goto;
using JetBrains.ReSharper.Feature.Services.Navigation.Search;
using JetBrains.ReSharper.Feature.Services.Occurences;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.PsiPlugin.Cache;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Feature.Finding.GotoMember
{
  [ShellFeaturePart]
  public class PsiGotoFileMemberProvider : IGotoFileMemberProvider
  {
    public IEnumerable<MatchingInfo> FindMatchingInfos(IdentifierMatcher matcher, INavigationScope scope, GotoContext gotoContext, Func<bool> checkForInterrupt)
    {
      var fileMemberScope = scope as FileMemberNavigationScope;
      if (fileMemberScope == null)
        return EmptyList<MatchingInfo>.InstanceList;

      var primaryMembersData = GetPrimaryMembers(fileMemberScope);

      var psiFileMembersMap = new PsiFileMembersMap();

      var result = new Collection<MatchingInfo>();
      foreach (var data in primaryMembersData)
      {
        var quickSearchTexts = GetQuickSearchTexts(data.Element);
        var matchedText = quickSearchTexts.FirstOrDefault(tuple => matcher.Matches(tuple.A));
        if (matchedText == null)
          continue;

        psiFileMembersMap.Add(matchedText.A, data);

        var matchingIndicies = matchedText.B ? matcher.MatchingIndicies(matchedText.A) : EmptyArray<IdentifierMatch>.Instance;
        result.Add(new MatchingInfo(matchedText.A, matcher.Filter == "*" ? EmptyList<IdentifierMatch>.InstanceList : matchingIndicies, matchedText.B));
      }

      gotoContext.PutData(PsiFileMembersMap.PsiFileMembersMapKey, psiFileMembersMap);
      return result;
    }

    protected virtual bool IsSourceFileAvailable(IPsiSourceFile sourceFile)
    {
      return sourceFile.IsValid();
    }

    public bool IsApplicable(INavigationScope scope, GotoContext gotoContext, IdentifierMatcher matcher)
    {
      return true;
    }

    public IEnumerable<IOccurence> GetOccurencesByMatchingInfo(MatchingInfo navigationInfo, INavigationScope scope, GotoContext gotoContext, Func<bool> checkForInterrupt)
    {
      var fileMembersMap = gotoContext.GetData(PsiFileMembersMap.PsiFileMembersMapKey);
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
    protected IOccurence CreateOccurence(PsiFileMemberData psiFileMemberData)
    {
        return new DeclaredElementOccurence(psiFileMemberData.Element,
          new OccurencePresentationOptions
          {
            ContainerStyle = !(psiFileMemberData.Element is ITypeElement) ? psiFileMemberData.ContainerDisplayStyle : ContainerDisplayStyle.NoContainer,
            LocationStyle = GlobalLocationStyle.None
          });
    }

    private IEnumerable<PsiFileMemberData> GetPrimaryMembers(FileMemberNavigationScope fileMemberScope)
    {
      var primarySourceFile = fileMemberScope.GetPrimarySourceFile();
      if (!IsSourceFileAvailable(primarySourceFile))
        return EmptyList<PsiFileMemberData>.InstanceList;

      var psiSymbols = GetPsiSourceFileTypeElements(primarySourceFile);

      var services = primarySourceFile.GetPsiServices();
      var psiFile = services.Files.GetPrimaryPsiFile(primarySourceFile) as PsiFile;
      var primaryMembers = new LinkedList<PsiFileMemberData>();
      foreach(var symbol in psiSymbols)
      {
        if (psiFile != null)
        {
          var declaredElements = psiFile.GetDeclaredElements(symbol.Name);
          foreach (var declaredElement in declaredElements)
          {
            primaryMembers.AddFirst(new PsiFileMemberData(declaredElement, ContainerDisplayStyle.NoContainer));
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

    private IEnumerable<JetTuple<string, bool>> GetQuickSearchTexts(IDeclaredElement declaredElement)
    {

      return new[] { JetTuple.Of(declaredElement.ShortName, true) };
    }

    protected class PsiFileMemberData
    {
      private readonly IDeclaredElement myElement;
      private readonly ContainerDisplayStyle myDisambigStyle;

      public PsiFileMemberData(IDeclaredElement element, ContainerDisplayStyle disambigStyle)
      {
        myElement = element;
        myDisambigStyle = disambigStyle;
      }

      public IDeclaredElement Element
      {
        get { return myElement; }
      }

      public ContainerDisplayStyle ContainerDisplayStyle
      {
        get { return myDisambigStyle; }
      }
    }

    private class PsiFileMembersMap : OneToSetMap<string, PsiFileMemberData>
    {
      public static readonly Key<PsiFileMembersMap> PsiFileMembersMapKey = new Key<PsiFileMembersMap>("PsiFileMembersMap");
    }
  }
}
