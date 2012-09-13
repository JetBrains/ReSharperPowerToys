using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl
{
  internal partial class PsiFile
  {
    private readonly Dictionary<string, IDeclaredElement> myDeclarations = new Dictionary<string, IDeclaredElement>();

    private ISymbolTable myOptionSymbolTable;
    private ISymbolTable myPathSymbolTable;
    private ISymbolTable myRoleSymbolTable;
    private ISymbolTable myRuleSymbolTable;

    private string myInterfacePrefix = "";
    private string myParserClassName = "";
    private string myParserPackageName = "";
    private string myTokenTypeClassFqName = "";
    private string myTreeClassesPackageName = "";
    private string myTreeInterfacesPackageName = "";
    private string myVisitorClassName = "";
    private string myVisitorMethodPrefix = "";
    private string myVisitorMethodSuffix = "";

    public ISymbolTable FileRuleSymbolTable
    {
      get
      {
        if (myRuleSymbolTable != null)
        {
          return myRuleSymbolTable;
        }
        lock (this)
        {
          return myRuleSymbolTable ?? (myRuleSymbolTable = CreateRulesSymbolTable());
        }
      }
    }

    public ISymbolTable FileOptionSymbolTable
    {
      get
      {
        if (myOptionSymbolTable != null)
        {
          return myOptionSymbolTable;
        }
        lock (this)
        {
          return myOptionSymbolTable ?? (myOptionSymbolTable = CreateOptionSymbolTable(true));
        }
      }
    }

    public ISymbolTable FileRoleSymbolTable
    {
      get
      {
        if (myRoleSymbolTable != null)
        {
          return myRoleSymbolTable;
        }
        lock (this)
        {
          return myRoleSymbolTable ?? (myRoleSymbolTable = CreateRoleSymbolTable());
        }
      }
    }

    public ISymbolTable FilePathSymbolTable
    {
      get
      {
        if (myPathSymbolTable != null)
        {
          return myPathSymbolTable;
        }
        lock (this)
        {
          return myPathSymbolTable ?? (myPathSymbolTable = CreatePathSymbolTable());
        }
      }
    }

    #region IPsiFile Members

    public override PsiLanguageType Language
    {
      get { return PsiLanguage.Instance; }
    }

    #endregion

    protected override void ClearCachedData()
    {
      base.ClearCachedData();
      myTokenTypeClassFqName = "";
      myRuleSymbolTable = null;
      myOptionSymbolTable = null;
      myRoleSymbolTable = null;
      myPathSymbolTable = null;
      myDeclarations.Clear();
    }

    public void ClearTables()
    {
      myRuleSymbolTable = null;
      myOptionSymbolTable = null;
      myRoleSymbolTable = null;
      myPathSymbolTable = null;
      myDeclarations.Clear();
    }

    private void CollectDeclarations()
    {
      ITreeNode child = firstChild;
      while (child != null)
      {
        var declaration = child as IDeclaration;
        if (declaration != null)
        {
          string s = declaration.DeclaredName;
          myDeclarations[s] = declaration.DeclaredElement;
        }
        child = child.NextSibling;
      }
      child = Interfaces;
      if (child != null)
      {
        child = child.FirstChild;
        while (child != null)
        {
          var declaration = child as IDeclaration;
          if (declaration != null)
          {
            string s = declaration.DeclaredName;
            myDeclarations[s] = declaration.DeclaredElement;
          }
          child = child.NextSibling;
        }
      }
    }

    public ISymbolTable CreateRulesSymbolTable()
    {
      CollectDeclarations();
      if (GetSourceFile() != null)
      {
        Dictionary<string, IDeclaredElement>.ValueCollection elements = myDeclarations.Values;
        myRuleSymbolTable = ResolveUtil.CreateSymbolTable(elements, 0);
      }
      else
      {
        myRuleSymbolTable = null;
      }

      var optionsDefinition = FirstChild as IOptionsDefinition;
      if (optionsDefinition != null)
      {
        ITreeNode child = optionsDefinition.FirstChild;
        ITreeNode tokenTypeClassFQNNode = null;
        ITreeNode parserClassNameNode = null;
        ITreeNode parserPackageNode = null;
        ITreeNode treeInterfacesPackageNode = null;
        ITreeNode treeClassesPackageNode = null;
        ITreeNode visitorClassNameNode = null;
        ITreeNode interfacePrefixNode = null;
        ITreeNode visitorMethodSuffixNode = null;
        ITreeNode visitorMethodPrefixNode = null;
        while (child != null)
        {
          var optionDefinition = child as IOptionDefinition;
          if (optionDefinition != null)
          {
            IOptionName optionName = optionDefinition.OptionName;
            var token = (optionName.FirstChild as PsiTokenBase);
            if (token.NodeType.Equals(PsiTokenType.STRING_LITERAL))
            {
              if ("\"tokenTypeClassFQName\"".Equals(token.GetText()))
              {
                tokenTypeClassFQNNode = optionDefinition.OptionStringValue;
              }
              if ("\"visitMethodPrefix\"".Equals(token.GetText()))
              {
                visitorMethodPrefixNode = optionDefinition.OptionStringValue;
              }
            }
            if ("parserClassName".Equals(optionName.GetText()))
            {
              parserClassNameNode = optionDefinition.OptionStringValue;
            }
            if ("parserPackage".Equals(optionName.GetText()))
            {
              parserPackageNode = optionDefinition.OptionStringValue;
            }
            if ("psiInterfacePackageName".Equals(optionName.GetText()))
            {
              treeInterfacesPackageNode = optionDefinition.OptionStringValue;
            }
            if ("psiStubsPackageName".Equals(optionName.GetText()))
            {
              treeClassesPackageNode = optionDefinition.OptionStringValue;
            }
            if ("visitorClassName".Equals(optionName.GetText()))
            {
              visitorClassNameNode = optionDefinition.OptionStringValue;
            }
            if ("visitorMethodSuffix".Equals(optionName.GetText()))
            {
              visitorMethodSuffixNode = optionDefinition.OptionStringValue;
            }
            if ("\"interfaceNamePrefix\"".Equals(optionName.GetText()))
            {
              interfacePrefixNode = optionDefinition.OptionStringValue;
            }
          }
          child = child.NextSibling;
        }
        if (tokenTypeClassFQNNode != null)
        {
          AddTokensToSymbolTable(tokenTypeClassFQNNode);
        }

        if ((parserPackageNode != null) && (parserClassNameNode != null))
        {
          AddDerivedElementsToSymbolTable(visitorMethodSuffixNode, treeInterfacesPackageNode, parserPackageNode, parserClassNameNode, treeClassesPackageNode, interfacePrefixNode, visitorMethodPrefixNode, visitorClassNameNode);
        }
      }

      return myRuleSymbolTable;
    }

    private void AddDerivedElementsToSymbolTable(ITreeNode visitorMethodSuffixNode, ITreeNode treeInterfacesPackageNode, ITreeNode parserPackageNode, ITreeNode parserClassNameNode, ITreeNode treeClassesPackageNode, ITreeNode interfacePrefixNode, ITreeNode visitorMethodPrefixNode, ITreeNode visitorClassNameNode)
    {
      if (parserClassNameNode != null)
      {
        myParserClassName = parserClassNameNode.GetText();
        myParserClassName = myParserClassName.Substring(1, myParserClassName.Length - 2);
      }
      else
      {
        myParserClassName = "";
      }
      if (myParserPackageName != null)
      {
        myParserPackageName = parserPackageNode.GetText();
        myParserPackageName = myParserPackageName.Substring(1, myParserPackageName.Length - 2);
      }
      else
      {
        myParserPackageName = "";
      }
      if (treeInterfacesPackageNode != null)
      {
        myTreeInterfacesPackageName = treeInterfacesPackageNode.GetText();
        myTreeInterfacesPackageName = myTreeInterfacesPackageName.Substring(1, myTreeInterfacesPackageName.Length - 2);
      }
      else
      {
        myTreeInterfacesPackageName = "";
      }
      if (treeClassesPackageNode != null)
      {
        myTreeClassesPackageName = treeClassesPackageNode.GetText();
        myTreeClassesPackageName = myTreeClassesPackageName.Substring(1, myTreeClassesPackageName.Length - 2);
      }
      else
      {
        myTreeClassesPackageName = "";
      }

      if (visitorClassNameNode != null)
      {
        myVisitorClassName = visitorClassNameNode.GetText();
        myVisitorClassName = myVisitorClassName.Substring(1, myVisitorClassName.Length - 2);
      }
      else
      {
        myVisitorClassName = "";
      }

      if (visitorMethodPrefixNode != null)
      {
        myVisitorMethodPrefix = visitorMethodPrefixNode.GetText();
        myVisitorMethodPrefix = myVisitorMethodPrefix.Substring(1, myVisitorMethodPrefix.Length - 2);
      }
      else
      {
        myVisitorMethodPrefix = "";
      }
      if (visitorMethodSuffixNode != null)
      {
        myVisitorMethodSuffix = visitorMethodSuffixNode.GetText();
        myVisitorMethodSuffix = myVisitorMethodSuffix.Substring(1, myVisitorMethodSuffix.Length - 2);
      }
      else
      {
        myVisitorMethodSuffix = "";
      }
      if (interfacePrefixNode != null)
      {
        myInterfacePrefix = interfacePrefixNode.GetText();
        myInterfacePrefix = myInterfacePrefix.Substring(1, myInterfacePrefix.Length - 2);
      }
      else
      {
        myInterfacePrefix = "_";
      }
    }


    public void CollectDerivedElements(RuleDeclaration ruleDeclaration)
    {
      ICollection<ITypeElement> classes =
        GetPsiServices().CacheManager.GetDeclarationsCache(GetPsiModule(), false, true).GetTypeElementsByCLRName(
          myParserPackageName + "." + myParserClassName);
      ICollection<ITypeElement> visitorClasses = CollectVisitorClasses();
      IEnumerator<ITypeElement> enumerator = classes.GetEnumerator();
      if (enumerator.MoveNext())
      {
        ruleDeclaration.CollectDerivedDeclaredElements((IClass)enumerator.Current, visitorClasses, myTreeInterfacesPackageName, myTreeClassesPackageName, myInterfacePrefix, myVisitorMethodPrefix, myVisitorMethodSuffix);
      }
    }

    private ICollection<ITypeElement> CollectVisitorClasses()
    {
      ICollection<ITypeElement> visitorClasses = new List<ITypeElement>();
      foreach (ITypeElement typeElement in GetPsiServices().CacheManager.GetDeclarationsCache(GetPsiModule(), false, true).GetTypeElementsByCLRName(
        myTreeInterfacesPackageName + "." + myVisitorClassName))
      {
        visitorClasses.Add(typeElement);
      }
      ICollection<ITypeElement> visitorGenericClasses =
        GetPsiServices().CacheManager.GetDeclarationsCache(GetPsiModule(), false, true).GetTypeElementsByCLRName(
          myTreeInterfacesPackageName + "." + myVisitorClassName + "`1");
      foreach (ITypeElement visitorGenericClass in visitorGenericClasses)
      {
        visitorClasses.Add(visitorGenericClass);
      }
      visitorGenericClasses =
        GetPsiServices().CacheManager.GetDeclarationsCache(GetPsiModule(), false, true).GetTypeElementsByCLRName(
          myTreeInterfacesPackageName + "." + myVisitorClassName + "`2");
      foreach (ITypeElement visitorGenericClass in visitorGenericClasses)
      {
        visitorClasses.Add(visitorGenericClass);
      }
      return visitorClasses;
    }

    private void AddTokensToSymbolTable(ITreeNode tokenTypeClassFQNameNode)
    {
      myTokenTypeClassFqName = tokenTypeClassFQNameNode.GetText();
      myTokenTypeClassFqName = myTokenTypeClassFqName.Substring(1, myTokenTypeClassFqName.Length - 2);
      ICollection<ITypeElement> classes =
        GetPsiServices().CacheManager.GetDeclarationsCache(GetPsiModule(), false, true).GetTypeElementsByCLRName(
          myTokenTypeClassFqName);
      IEnumerator<ITypeElement> enumerator = classes.GetEnumerator();
      if (enumerator.MoveNext())
      {
        var tokenTypeClass = enumerator.Current as IClass;
        if (tokenTypeClass != null)
        {
          IEnumerable<IField> fields = tokenTypeClass.Fields;
          IList<IDeclaredElement> elements = new List<IDeclaredElement>();
          foreach (IField field in fields)
          {
            if(field.IsReadonly)
            {
              elements.Add(field);
            }
          }
          ISymbolTable tokenSymbolTable = ResolveUtil.CreateSymbolTable(elements, 0);
          myRuleSymbolTable = myRuleSymbolTable.Merge(tokenSymbolTable);
        }
      }
    }

    public ISymbolTable CreateOptionSymbolTable(bool ifFileReal)
    {
      if (ifFileReal)
      {
        IList<IDeclaredElement> optionDeclaredElements = new List<IDeclaredElement>();
        foreach (string name in OptionDeclaredElements.FileOptionNames)
        {
          IDeclaredElement element = new OptionPropertyDeclaredElement(GetSourceFile(), name,
            GetSourceFile().GetPsiServices());
          optionDeclaredElements.Add(element);
        }

        if (optionDeclaredElements.Count > 0)
        {
          myOptionSymbolTable = ResolveUtil.CreateSymbolTable(optionDeclaredElements, 0);
        }

        optionDeclaredElements = new List<IDeclaredElement>();
        foreach (string name in OptionDeclaredElements.RuleOptionNames)
        {
          IDeclaredElement element = new OptionPropertyDeclaredElement(GetSourceFile(), name,
            GetSourceFile().GetPsiServices());
          optionDeclaredElements.Add(element);
        }

        if (optionDeclaredElements.Count > 0)
        {
          myOptionSymbolTable = myOptionSymbolTable.Merge(ResolveUtil.CreateSymbolTable(optionDeclaredElements, 0));
        }
        return myOptionSymbolTable;
      }
      return EmptySymbolTable.INSTANCE;
    }

    private ISymbolTable CreateRoleSymbolTable()
    {
      IList<IDeclaredElement> roleDeclaredElements = new List<IDeclaredElement>();
      IList<string> roles = new List<string>();
      ITreeNode child = FirstChild;
      while (child != null)
      {
        var ruleDeclaration = child as IRuleDeclaration;
        if (ruleDeclaration != null)
        {
          IExtrasDefinition extras = (ruleDeclaration).Extras;
          if (extras != null)
          {
            ITreeNode extrasChild = extras.FirstChild;
            while (extrasChild != null)
            {
              extrasChild = extrasChild.NextSibling;
              var extraDefinition = extrasChild as IExtraDefinition;
              if (extraDefinition != null)
              {
                IPathValue pathValue = (extraDefinition).PathValue;
                if (pathValue != null)
                {
                  ITreeNode pathElement = pathValue.FirstChild;
                  while (pathElement != null)
                  {
                    var element = pathElement as IPathElement;
                    if (element != null)
                    {
                      IRoleName roleName = (element).RoleName;
                      if ((roleName != null) && (!roles.Contains(roleName.GetText())))
                      {
                        roles.Add(roleName.GetText());
                      }
                    }
                    pathElement = pathElement.NextSibling;
                  }
                }
              }
            }
          }
        }
        child = child.NextSibling;
      }
      foreach (string role in roles)
      {
        IDeclaredElement element = new RoleDeclaredElement(this, role, GetPsiServices());
        roleDeclaredElements.Add(element);
      }
      myRoleSymbolTable = ResolveUtil.CreateSymbolTable(roleDeclaredElements, 0);
      return myRoleSymbolTable;
    }

    private ISymbolTable CreatePathSymbolTable()
    {
      IList<IDeclaredElement> pathDeclaredElements = new List<IDeclaredElement>();
      var child = Paths as ITreeNode;
      if (child != null)
      {
        child = child.FirstChild;
        while (child != null)
        {
          var pathDeclaration = child as PathDeclaration;
          if (pathDeclaration != null)
          {
            pathDeclaredElements.Add(pathDeclaration);
          }
          child = child.NextSibling;
        }
      }

      myPathSymbolTable = ResolveUtil.CreateSymbolTable(pathDeclaredElements, 0);
      myPathSymbolTable = myPathSymbolTable.Merge(FileRuleSymbolTable);

      return myPathSymbolTable;
    }

    public IEnumerable<IDeclaredElement> GetDeclaredElements(string name)
    {
      var list = new LinkedList<IDeclaredElement>();
      if(myDeclarations.ContainsKey(name))
      {
        var declaredElement = myDeclarations.GetValue(name); 
        list.AddFirst(declaredElement);
      }
      return list;
    } 
  }
}
