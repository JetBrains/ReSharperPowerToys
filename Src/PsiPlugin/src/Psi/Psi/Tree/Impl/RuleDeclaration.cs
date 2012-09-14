using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.ReSharper.PsiPlugin.Util;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl
{
  internal partial class RuleDeclaration : IDeclaredElement
  {
    private readonly IList<IDeclaredElement> myDerivedClasses = new List<IDeclaredElement>();
    private readonly IList<IDeclaredElement> myDerivedParserMethods = new List<IDeclaredElement>();
    private readonly IList<IDeclaredElement> myDerivedInterfaces = new List<IDeclaredElement>();
    private readonly IList<IDeclaredElement> myDerivedVisitorMethods = new List<IDeclaredElement>();
    private string myInterfacePrefix;

    private IClass myParserClass;
    private string myTreeClassesPackageName;
    private string myTreeInterfacesPackageName;
    private ICollection<ITypeElement> myVisitorClasses;
    private string myVisitorMethodPrefix;
    private string myVisitorMethodSuffix;

    public IEnumerable<IDeclaredElement> DerivedParserMethods
    {
      get
      {
        if(myDerivedParserMethods.Count == 0)
        {
          CollectDerivedDeclaredElements();
        }
        return myDerivedParserMethods;
      }
    }

    public IEnumerable<IDeclaredElement> DerivedClasses
    {
      get
      {
        if(myDerivedClasses.Count == 0)
        {
          CollectDerivedDeclaredElements();
        }
        return myDerivedClasses;
      }
    }

    public IEnumerable<IDeclaredElement> DerivedInterfaces
    {
      get
      {
        if(myDerivedInterfaces.Count == 0)
        {
          CollectDerivedDeclaredElements();
        }
        return myDerivedInterfaces;
      }
    }

    public IEnumerable<IDeclaredElement> DerivedVisitorMethods
    {
      get
      {
        if(myDerivedVisitorMethods.Count == 0)
        {
          CollectDerivedDeclaredElements();
        }
        return myDerivedVisitorMethods;
      }
    }

    public string VisitorMethodPrefix
    {
      get { return myVisitorMethodPrefix; }
    }

    public string VisitorMethodSuffix
    {
      get { return myVisitorMethodSuffix; }
    }

    public string InterfacePrefix
    {
      get { return myInterfacePrefix; }
    }

    #region IDeclaredElement Members

    public IList<IDeclaration> GetDeclarations()
    {
      var list = new List<IDeclaration> { this };
      return list;
    }

    public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
    {
      var list = new List<IDeclaration> { this };
      return list;
    }

    public DeclaredElementType GetElementType()
    {
      return PsiDeclaredElementType.Rule;
    }

    public XmlNode GetXMLDoc(bool inherit)
    {
      return null;
    }

    public XmlNode GetXMLDescriptionSummary(bool inherit)
    {
      return null;
    }

    public bool IsSynthetic()
    {
      return false;
    }

    public string ShortName
    {
      get { return RuleName.GetText(); }
    }

    public bool CaseSensistiveName
    {
      get { return true; }
    }

    public PsiLanguageType PresentationLanguage
    {
      get { return PsiLanguage.Instance; }
    }

    #endregion

    #region IRuleDeclaration Members

    public void SetName(string name)
    {
      PsiTreeUtil.ReplaceChild(RuleName, RuleName.FirstChild, name);
    }

    public TreeTextRange GetNameRange()
    {
      ITreeNode ruleName = RuleName;
      int offset = ruleName.GetNavigationRange().TextRange.StartOffset;
      return new TreeTextRange(new TreeOffset(offset), ruleName.GetText().Length);
    }

    public IDeclaredElement DeclaredElement
    {
      get { return this; }
    }

    public string DeclaredName
    {
      get { return GetDeclaredName(); }
    }

    public IChameleonNode ReSync(CachingLexer cachingLexer, TreeTextRange changedRange, int insertedTextLen)
    {
      TreeOffset currStartOffset = GetTreeStartOffset();
      int currLength = GetTextLength();

      Logger.Assert(changedRange.StartOffset >= currStartOffset && changedRange.EndOffset <= (currStartOffset + currLength),
        "changedRange.StartOffset >= currStartOffset && changedRange.EndOffset <= (currStartOffset+currLength)");

      int newLength = currLength - changedRange.Length + insertedTextLen;

      LanguageService languageService = Language.LanguageService();
      if (languageService != null)
      {
        var parser = (IPsiParser)languageService.CreateParser(new ProjectedLexer(cachingLexer, new TextRange(currStartOffset.Offset, currStartOffset.Offset + newLength)), GetPsiModule(), GetSourceFile());
        TreeElement newElement = parser.ParseStatement();
        if (newElement.GetTextLength() == 0)
        {
          return null;
        }
        if ((newElement.GetTextLength() == newLength) && (";".Equals(newElement.GetText().Substring(newElement.GetTextLength() - 1))))
        {
          var psiFile = GetContainingNode<PsiFile>();
          if (psiFile != null)
          {
            psiFile.ClearTables();
          }
          return newElement as IRuleDeclaration;
        }
      }
      return null;
    }

    public bool IsOpened
    {
      get { return false; }
    }

    public ISymbolTable VariableSymbolTable
    {
      get
      {
        IList<VariableDeclaration> elements = PsiTreeUtil.GetAllChildren<VariableDeclaration>(this).AsIList();
        return ResolveUtil.CreateSymbolTable(elements, 0);
      }
    }

    #endregion

    private string GetDeclaredName()
    {
      return RuleName.GetText();
    }

    public void CollectDerivedDeclaredElements()
    {
      var psiFile = GetContainingNode<PsiFile>();
      if(psiFile != null)
      {
        psiFile.CollectDerivedElements(this);
      }
    }

    public void CollectDerivedDeclaredElements(IClass parserClass, ICollection<ITypeElement> visitorClasses, string treeInterfacesPackageName, string treeClassesPackageName, string interfacePrefix, string visitorMethodPrefix, string visitorMethodSuffix)
    {
      myParserClass = parserClass;
      myVisitorClasses = visitorClasses;
      myTreeInterfacesPackageName = treeInterfacesPackageName;
      myTreeClassesPackageName = treeClassesPackageName;
      myInterfacePrefix = interfacePrefix;
      myVisitorMethodPrefix = visitorMethodPrefix;
      myVisitorMethodSuffix = visitorMethodSuffix;
      UpdateDerivedDeclaredElements();
    }

    public void UpdateDerivedDeclaredElements()
    {
      myDerivedParserMethods.Clear();
      myDerivedClasses.Clear();
      myDerivedInterfaces.Clear();
      myDerivedVisitorMethods.Clear();
      if (myParserClass != null)
      {
        UpdateParserMethods();
      }
      if (myVisitorClasses != null)
      {
        UpdateVisitorClasses();
      }

      UpdateClasses();

      UpdateInterfaces();
    }

    private void UpdateInterfaces()
    {
      ICollection<ITypeElement> interfaces = GetPsiServices().CacheManager.GetDeclarationsCache(GetPsiModule(), false, true).GetTypeElementsByCLRName(myTreeInterfacesPackageName + "." + myInterfacePrefix + NameToCamelCase());
      foreach (ITypeElement declaredElement in interfaces)
      {
        myDerivedInterfaces.Add(declaredElement);
      }
    }

    private void UpdateClasses()
    {
      ICollection<ITypeElement> classes = GetPsiServices().CacheManager.GetDeclarationsCache(GetPsiModule(), false, true).GetTypeElementsByCLRName(myTreeClassesPackageName + "." + NameToCamelCase());
      foreach (ITypeElement declaredElement in classes)
      {
        myDerivedClasses.Add(declaredElement);
      }
    }

    private void UpdateVisitorClasses()
    {
      foreach (ITypeElement visitorClass in myVisitorClasses)
      {
        IEnumerable<IMethod> methods = visitorClass.Methods;
        foreach (IMethod method in methods)
        {
          string name = myVisitorMethodPrefix + NameToCamelCase() + myVisitorMethodSuffix;
          if ((name).Equals(method.ShortName) || ((name + "`").Equals(method.ShortName)) || ((name + "``").Equals(method.ShortName)))
          {
            myDerivedVisitorMethods.Add(method);
          }
        }
      }
    }

    private void UpdateParserMethods()
    {
      IEnumerable<IMethod> methods = myParserClass.Methods;
      foreach (IMethod method in methods)
      {
        if (("parse" + NameToCamelCase()).Equals(method.ShortName))
        {
          myDerivedParserMethods.Add(method);
        }
      }
    }

    private string NameToCamelCase()
    {
      string s = DeclaredName;
      string firstLetter = s.Substring(0, 1);
      firstLetter = firstLetter.ToUpper();
      s = firstLetter + s.Substring(1, s.Length - 1);
      return s;
    }

    public override IChameleonNode FindChameleonWhichCoversRange(TreeTextRange textRange)
    {
      if (textRange.ContainedIn(TreeTextRange.FromLength(GetTextLength())))
      {
        return base.FindChameleonWhichCoversRange(textRange) ?? this;
      }

      return null;
    }

    public IEnumerable<Pair<IDeclaredElement, Predicate<FindResult>>> GetRelatedDeclaredElements()
    {
      UpdateDerivedDeclaredElements();
      foreach (var element in DerivedClasses)
      {
        yield return new Pair<IDeclaredElement, Predicate<FindResult>>(element, JetPredicate<FindResult>.True);
      }
      foreach (var element in DerivedInterfaces)
      {
        yield return new Pair<IDeclaredElement, Predicate<FindResult>>(element, JetPredicate<FindResult>.True);
      }
      foreach (var element in DerivedParserMethods)
      {
        yield return new Pair<IDeclaredElement, Predicate<FindResult>>(element, JetPredicate<FindResult>.True);
      }
      foreach (var element in DerivedVisitorMethods)
      {
        yield return new Pair<IDeclaredElement, Predicate<FindResult>>(element, JetPredicate<FindResult>.True);
      }
    }
  }
}
