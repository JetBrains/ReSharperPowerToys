using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    private IList<IDeclaredElement> myDerivedDeclaredElements = new List<IDeclaredElement>();
    private IList<IDeclaredElement> myDerivedClasses = new List<IDeclaredElement>();
    private IList<IDeclaredElement> myDerivedInterfaces = new List<IDeclaredElement>();

    private IClass myParserClass = null;
    private string myTreeInterfacesPackageName;
    private string myTreeClassesPackageName;

    public void SetName(string name)
    {
      PsiTreeUtil.ReplaceChild(RuleName, RuleName.FirstChild, name);
    }

    public TreeTextRange GetNameRange()
    {
      ITreeNode ruleName = RuleName;
      int offset = ruleName.GetNavigationRange().TextRange.StartOffset;
      return new TreeTextRange(new TreeOffset(offset), ruleName.GetText().Length); //ruleName.GetNavigationRange());
    }

    public IList<IDeclaration> GetDeclarations()
    {
      var list = new List<IDeclaration>();
      list.Add(this);
      return list;
    }

    public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
    {
      var list = new List<IDeclaration>();
      list.Add(this);
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
      get { return getDeclaredName(); }
    }

    private string getDeclaredName()
    {
      return RuleName.GetText();
    }

    public PsiLanguageType PresentationLanguage
    {
      get { return PsiLanguage.Instance; }
    }

    public IList<IDeclaredElement> DerivedDeclaredElements
    {
      get { return myDerivedDeclaredElements; }
    }

    public IList<IDeclaredElement> DerivedClasses
    {
      get { return myDerivedClasses; }
    }

    public IList<IDeclaredElement> DerivedInterfaces
    {
      get { return myDerivedInterfaces; }
    }
 
    public void CollectDerivedDeclaredElements(IClass parserClass, string treeInterfacesPackageName, string treeClassesPackageName)
    {
      myParserClass = parserClass;
      myTreeInterfacesPackageName = treeInterfacesPackageName;
      myTreeClassesPackageName = treeClassesPackageName;
      CollectDerivedDeclaredElements();
    }

    public void CollectDerivedDeclaredElements(){
      myDerivedDeclaredElements.Clear();
      myDerivedClasses.Clear();
      myDerivedInterfaces.Clear();
      if (myParserClass != null)
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
      var classes = GetPsiServices().CacheManager.GetDeclarationsCache(GetPsiModule(), false, true).GetTypeElementsByCLRName(myTreeClassesPackageName + "." + NameToCamelCase());
      foreach (IDeclaredElement declaredElement in classes)
      {
        myDerivedClasses.Add(declaredElement);
      }
      classes = GetPsiServices().CacheManager.GetDeclarationsCache(GetPsiModule(), false, true).GetTypeElementsByCLRName(myTreeInterfacesPackageName + "." + "I" + NameToCamelCase());
      foreach (IDeclaredElement declaredElement in classes)
      {
        myDerivedInterfaces.Add(declaredElement);
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

      var parser = (IPsiParser)Language.LanguageService().CreateParser(new ProjectedLexer(cachingLexer, new TextRange(currStartOffset.Offset, currStartOffset.Offset + newLength)), GetPsiModule(), GetSourceFile());
      TreeElement newElement = parser.ParseStatement();
      if((newElement.GetTextLength() == newLength) && (";".Equals(newElement.GetText().Substring(newElement.GetTextLength() - 1))))
      {
        GetContainingNode<PsiFile>(false).ClearTables();
        return newElement as IRuleDeclaration;
      }
      else
      {
        return null;
      }
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
      get { IList<VariableDeclaration> elements = PsiTreeUtil.getAllChildren<VariableDeclaration>(this).AsIList();
        ISymbolTable table;
        if(Parameters != null)
        {
          ITreeNode child = Parameters.FirstChild;
          while(child != null)
          {
            if(child is VariableDeclaration)
            {
              elements.Add((VariableDeclaration) child);
            }
            child = child.NextSibling;
          }
        }
        return ResolveUtil.CreateSymbolTable(elements, 0);
      }
    }
  }
}
