using System.Collections.Generic;
using System.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.ReSharper.PsiPlugin.Util;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  internal partial class RuleDeclaration : IDeclaredElement
  {
    private readonly IList<IDeclaredElement> myDerivedDeclaredElements = new List<IDeclaredElement>();
    private readonly IList<IDeclaredElement> myDerivedClasses = new List<IDeclaredElement>();
    private readonly IList<IDeclaredElement> myDerivedInterfaces = new List<IDeclaredElement>();
    private readonly IList<IDeclaredElement> myDerivedVisitorMethods = new List<IDeclaredElement>();

    private IClass myParserClass;
    private ICollection<ITypeElement> myVisitorClasses; 
    private string myTreeInterfacesPackageName;
    private string myTreeClassesPackageName;
    private string myVisitorMethodPrefix;
    private string myVisitorMethodSuffix;
    private string myInterfacePrefix;

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

    public IList<IDeclaration> GetDeclarations()
    {
      var list = new List<IDeclaration> {this};
      return list;
    }

    public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
    {
      var list = new List<IDeclaration> {this};
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

    public IDeclaredElement DeclaredElement
    {
      get { return this; }
    }

    public string DeclaredName
    {
      get { return GetDeclaredName(); }
    }

    private string GetDeclaredName()
    {
      return RuleName.GetText();
    }

    public PsiLanguageType PresentationLanguage
    {
      get { return PsiLanguage.Instance; }
    }

    public IEnumerable<IDeclaredElement> DerivedDeclaredElements
    {
      get { return myDerivedDeclaredElements; }
    }

    public IEnumerable<IDeclaredElement> DerivedClasses
    {
      get { return myDerivedClasses; }
    }

    public IEnumerable<IDeclaredElement> DerivedInterfaces
    {
      get { return myDerivedInterfaces; }
    }

    public IEnumerable<IDeclaredElement> DerivedVisitorMethods
    {
      get { return myDerivedVisitorMethods; }
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

    public void UpdateDerivedDeclaredElements(){
      myDerivedDeclaredElements.Clear();
      myDerivedClasses.Clear();
      myDerivedInterfaces.Clear();
      myDerivedVisitorMethods.Clear();
      if (myParserClass != null)
      {
        UpdateParserMethods();
      }
      if(myVisitorClasses != null)
      {
        UpdateVisitorClasses();
      }

      UpdateClasses();

      UpdateInterfaces();
    }

    private void UpdateInterfaces()
    {
      var interfaces = GetPsiServices().CacheManager.GetDeclarationsCache(GetPsiModule(), false, true).GetTypeElementsByCLRName(myTreeInterfacesPackageName + "." + myInterfacePrefix + NameToCamelCase());
      foreach (ITypeElement declaredElement in interfaces)
      {
        myDerivedInterfaces.Add(declaredElement);
      }
    }

    private void UpdateClasses()
    {
      var classes = GetPsiServices().CacheManager.GetDeclarationsCache(GetPsiModule(), false, true).GetTypeElementsByCLRName(myTreeClassesPackageName + "." + NameToCamelCase());
      foreach (ITypeElement declaredElement in classes)
      {
        myDerivedClasses.Add(declaredElement);
      }
    }

    private void UpdateVisitorClasses()
    {
      foreach (var visitorClass in myVisitorClasses)
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
          myDerivedDeclaredElements.Add(method);
        }
      }
    }

    private string NameToCamelCase()
    {
      string s = DeclaredName;
      string firstLetter = s.Substring(0, 1);
      firstLetter = firstLetter.ToUpper();
      s = firstLetter + s.Substring(1,s.Length - 1);
      return s;
    }

    public IChameleonNode ReSync(CachingLexer cachingLexer, TreeTextRange changedRange, int insertedTextLen)
    {
      TreeOffset currStartOffset = GetTreeStartOffset();
      int currLength = GetTextLength();

      Logger.Assert(changedRange.StartOffset >= currStartOffset && changedRange.EndOffset <= (currStartOffset + currLength),
        "changedRange.StartOffset >= currStartOffset && changedRange.EndOffset <= (currStartOffset+currLength)");

      int newLength = currLength - changedRange.Length + insertedTextLen;

      var languageService = Language.LanguageService();
      if (languageService != null)
      {
        var parser = (IPsiParser)languageService.CreateParser(new ProjectedLexer(cachingLexer, new TextRange(currStartOffset.Offset, currStartOffset.Offset + newLength)), GetPsiModule(), GetSourceFile());
        TreeElement newElement = parser.ParseStatement();
        if((newElement.GetTextLength() == newLength) && (";".Equals(newElement.GetText().Substring(newElement.GetTextLength() - 1))))
        {
          var psiFile = GetContainingNode<PsiFile>();
          if (psiFile != null) psiFile.ClearTables();
          return newElement as IRuleDeclaration;
        }
      }
      return null;
    }

    public bool IsOpened
    {
      get { return false;  }
    }

    public override IChameleonNode FindChameleonWhichCoversRange(TreeTextRange textRange)
    {
      if (textRange.ContainedIn(TreeTextRange.FromLength(GetTextLength())))
      {
        return base.FindChameleonWhichCoversRange(textRange) ?? this;
      }

      return null;
    }

    public ISymbolTable VariableSymbolTable
    {
      get { IList<VariableDeclaration> elements = PsiTreeUtil.GetAllChildren<VariableDeclaration>(this).AsIList();
        if(Parameters != null)
        {
          ITreeNode child = Parameters.FirstChild;
          while(child != null)
          {
            var variableDeclaration = child as VariableDeclaration;
            if (variableDeclaration != null)
            {
              elements.Add(variableDeclaration);
            }
            child = child.NextSibling;
          }
        }
        return ResolveUtil.CreateSymbolTable(elements, 0);
      }
    }
  }
}
