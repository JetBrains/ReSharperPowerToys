using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.ReSharper.Psi.Impl.PsiManagerImpl;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  internal partial class PsiFile
  {
    private ISymbolTable myRuleSymbolTable = null;
    private ISymbolTable myOptionSymbolTable = null;
    private ISymbolTable myRoleSymbolTable = null;
    private ISymbolTable myPathSymbolTable = null;
    private IPsiModule myModule;
    private string tokenTypeClassFQName = "";
    private string parserClassName = "";
    private string parserPackageName = "";
    private string treeInterfacesPackageName = "";
    private string treeClassesPackageName = "";

    protected Dictionary<string, IDeclaredElement> Declarations = new Dictionary<string, IDeclaredElement>();

    protected override void ClearCachedData()
    {
      base.ClearCachedData();
      tokenTypeClassFQName = "";
      myRuleSymbolTable = null;
      myOptionSymbolTable = null;
      myRoleSymbolTable = null;
      myPathSymbolTable = null;
      Declarations.Clear();
    }

    public void ClearTables()
    {
      myRuleSymbolTable = null;
      myOptionSymbolTable = null;
      myRoleSymbolTable = null;
      myPathSymbolTable = null;
      Declarations.Clear();
    }

    public void CollectDeclarations()
    {
      ITreeNode child = firstChild;
      while (child != null)
      {
        var declaration = child as IDeclaration;
        if (declaration != null)
        {
          string s = declaration.DeclaredName;
          Declarations[s] = declaration.DeclaredElement;
        }
        child = child.NextSibling;
      }
      child = Interfaces as ITreeNode;
      if(child != null)
      {
        child = child.FirstChild;
        while (child != null)
        {
          var declaration = child as IDeclaration;
          if (declaration != null)
          {
            string s = declaration.DeclaredName;
            Declarations[s] = declaration.DeclaredElement;
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
        var elements = Declarations.Values;
        myRuleSymbolTable = ResolveUtil.CreateSymbolTable(elements, 0);
      }
      else
      {
        myRuleSymbolTable = null;
      }

      IOptionsDefinition optionsDefinition = FirstChild as IOptionsDefinition;
      if (optionsDefinition != null)
      {
        IOptionDefinition optionDefinition;
        ITreeNode child = optionsDefinition.FirstChild;
        ITreeNode tokenTypeClassFQNameNode = null;
        ITreeNode parserClassNameNode = null;
        ITreeNode parserPackageNode = null;
        ITreeNode treeInterfacesPackageNode = null;
        ITreeNode treeClassesPackageNode = null;
        while (child != null)
        {
          optionDefinition = child as IOptionDefinition;
          if (optionDefinition != null)
          {
            IOptionName optionName = optionDefinition.OptionName;
            PsiTokenBase token = (optionName.FirstChild as PsiTokenBase);
            if (token.NodeType.Equals(PsiTokenType.STRING_LITERAL))
            {
              if ("\"tokenTypeClassFQName\"".Equals(token.GetText()))
              {
                tokenTypeClassFQNameNode = optionDefinition.OptionStringValue;
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
          }
          child = child.NextSibling;
        }
        if (tokenTypeClassFQNameNode != null)
        {
          tokenTypeClassFQName = tokenTypeClassFQNameNode.GetText();
          tokenTypeClassFQName = tokenTypeClassFQName.Substring(1, tokenTypeClassFQName.Length - 2);
          var classes =
            GetPsiServices().CacheManager.GetDeclarationsCache(GetPsiModule(), false, true).GetTypeElementsByCLRName(
              tokenTypeClassFQName);
          IEnumerator<ITypeElement> enumerator = classes.GetEnumerator();
          if (enumerator.MoveNext())
          {
            IClass tokenTypeClass = enumerator.Current as IClass;
            if (tokenTypeClass != null)
            {
              IEnumerable<IField> fields = tokenTypeClass.Fields;
              IList<IDeclaredElement> elements = new List<IDeclaredElement>();
              foreach (IField field in fields)
              {
                if (field.IsReadonly && field.IsStatic)
                {
                  elements.Add(field);
                }
              }
              ISymbolTable tokenSymbolTable = ResolveUtil.CreateSymbolTable(elements, 0);
              myRuleSymbolTable = myRuleSymbolTable.Merge(tokenSymbolTable);
            }
          }
        }

        if ((parserPackageNode != null) && (parserClassNameNode != null))
        {
          parserClassName = parserClassNameNode.GetText();
          parserClassName = parserClassName.Substring(1, parserClassName.Length - 2);
          parserPackageName = parserPackageNode.GetText();
          parserPackageName = parserPackageName.Substring(1, parserPackageName.Length - 2);
          treeInterfacesPackageName = treeInterfacesPackageNode.GetText();
          treeInterfacesPackageName = treeInterfacesPackageName.Substring(1, treeInterfacesPackageName.Length - 2);
          treeClassesPackageName = treeClassesPackageNode.GetText();
          treeClassesPackageName = treeClassesPackageName.Substring(1, treeClassesPackageName.Length - 2);
          var classes =
            GetPsiServices().CacheManager.GetDeclarationsCache(GetPsiModule(), false, true).GetTypeElementsByCLRName(
              parserPackageName + "." + parserClassName);
          var elements = Declarations.Values;
          foreach (IDeclaredElement declaredElement in elements)
          {
            IEnumerator<ITypeElement> enumerator = classes.GetEnumerator();
            if (enumerator.MoveNext())
            {
              ((RuleDeclaration) declaredElement).CollectDerivedDeclaredElements((IClass) enumerator.Current, treeInterfacesPackageName, treeClassesPackageName);
            }
          }
        }
      }

      return myRuleSymbolTable;
    }

    public ISymbolTable CreateOptionSymbolTable(bool ifFileReal)
    {
      if (ifFileReal)
      {
        IList<IDeclaredElement> optionDeclaredElements = new List<IDeclaredElement>();
        foreach (string name in OptionDeclaredElements.FileOptionNames)
        {
          IDeclaredElement element = new OptionPropertyDeclaredElement(this.GetSourceFile(), name,
                                                                       this.GetSourceFile().GetPsiServices());
          optionDeclaredElements.Add(element);
        }

        if (optionDeclaredElements.Count > 0)
        {
          myOptionSymbolTable = ResolveUtil.CreateSymbolTable(optionDeclaredElements, 0);
        }

        optionDeclaredElements = new List<IDeclaredElement>();
        foreach (string name in OptionDeclaredElements.RuleOptionNames)
        {
          IDeclaredElement element = new OptionPropertyDeclaredElement(this.GetSourceFile(), name,
                                                                       this.GetSourceFile().GetPsiServices());
          optionDeclaredElements.Add(element);
        }

        if (optionDeclaredElements.Count > 0)
        {
          myOptionSymbolTable = myOptionSymbolTable.Merge(ResolveUtil.CreateSymbolTable(optionDeclaredElements, 0));
        }
        return myOptionSymbolTable;
      }
      else
      {
        return EmptySymbolTable.INSTANCE;
      }
    }

    public ISymbolTable CreateRoleSymbolTable()
    {
      IList<IDeclaredElement> roleDeclaredElements = new List<IDeclaredElement>();
      IList<string> roles = new List<string>();
      ITreeNode child = FirstChild;
      while (child != null)
      {
        if (child is IRuleDeclaration)
        {
          IExtrasDefinition extras = ((IRuleDeclaration) child).Extras;
          if (extras != null)
          {
            ITreeNode extrasChild = extras.FirstChild;
            while (extrasChild != null)
            {
              extrasChild = extrasChild.NextSibling;
              if (extrasChild is IExtraDefinition)
              {
                IPathValue pathValue = ((IExtraDefinition) extrasChild).PathValue;
                if (pathValue != null)
                {
                  ITreeNode pathElement = pathValue.FirstChild;
                  while (pathElement != null)
                  {
                    if (pathElement is IPathElement)
                    {
                      IRoleName roleName = ((IPathElement) pathElement).RoleName;
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

    public ISymbolTable CreatePathSymbolTable()
    {
      IList<IDeclaredElement> pathDeclaredElements = new List<IDeclaredElement>();
      ITreeNode child = Paths as ITreeNode;
      if (child != null)
      {
        child = child.FirstChild;
        while (child != null)
        {
          if (child is PathDeclaration)
          {
            pathDeclaredElements.Add((PathDeclaration) child);
          }
          child = child.NextSibling;
        }
      }

      myPathSymbolTable = ResolveUtil.CreateSymbolTable(pathDeclaredElements, 0);
      myPathSymbolTable = myPathSymbolTable.Merge(FileRuleSymbolTable);

      return myPathSymbolTable;
    }

    public ISymbolTable FileRuleSymbolTable
    {
      get
      {
        if (myRuleSymbolTable != null)
          return myRuleSymbolTable;
        lock (this)
        {
          if (myRuleSymbolTable == null)
            myRuleSymbolTable = CreateRulesSymbolTable();
          return myRuleSymbolTable;
        }
      }
    }

    public ISymbolTable FileOptionSymbolTable
    {
      get
      {
        if (myOptionSymbolTable != null)
          return myOptionSymbolTable;
        lock (this)
        {
          if (myOptionSymbolTable == null)
            myOptionSymbolTable = CreateOptionSymbolTable(true);
          return myOptionSymbolTable;
        }
      }
    }

    public ISymbolTable FileRoleSymbolTable
    {
      get
      {
        if (myRoleSymbolTable != null)
          return myRoleSymbolTable;
        lock (this)
        {
          if (myRoleSymbolTable == null)
            myRoleSymbolTable = CreateRoleSymbolTable();
          return myRoleSymbolTable;
        }
      }
    }

    public ISymbolTable FilePathSymbolTable
    {
      get
      {
        if (myPathSymbolTable != null)
          return myPathSymbolTable;
        lock (this)
        {
          if (myPathSymbolTable == null)
            myPathSymbolTable = CreatePathSymbolTable();
          return myPathSymbolTable;
        }
      }
    }

  }
}
