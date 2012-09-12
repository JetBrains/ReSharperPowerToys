using System.Collections.Generic;
using System.Linq;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Cache;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
using JetBrains.ReSharper.PsiPlugin.Refactoring.Rename;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Refactoring
{
  public static class DerivedDeclaredElementUtil
  {
    public static IDeclaredElement GetPrimaryDeclaredElementForInterface(IInterface @interface)
    {
      string interfaceName = @interface.ShortName;
      var cache = @interface.GetPsiServices().Solution.GetComponent<PsiCache>();
      IEnumerable<PsiOptionSymbol> prefixes = cache.GetOptionSymbols("\"interfaceNamePrefix\"");
      IList<IPsiSymbol> symbols = new List<IPsiSymbol>();
      foreach (PsiOptionSymbol prefix in prefixes)
      {
        string prefixValue = prefix.Value;
        if ((prefixValue.Length < interfaceName.Length) && (prefixValue.Equals(interfaceName.Substring(0, prefixValue.Length))))
        {
          string shortInterfaceName = PsiRenamesFactory.NameFromCamelCase(interfaceName.Substring(prefixValue.Length, interfaceName.Length - prefixValue.Length));
          List<IPsiSymbol> allSymbols = cache.GetSymbols(PsiRenamesFactory.NameFromCamelCase(shortInterfaceName)).ToList();
          foreach (IPsiSymbol symbol in allSymbols)
          {
            if (symbol.SourceFile == prefix.SourceFile)
            {
              symbols.Add(symbol);
            }
          }
        }
      }

      if (symbols.Count == 0)
      {
        return null;
      }

      // TODO: move symbol binding logic to the cache, remove copy-paste.
      IPsiSymbol returnSymbol = Enumerable.ToArray(symbols)[0];
      ITreeNode element = returnSymbol.SourceFile.GetPsiFile<PsiLanguage>(new DocumentRange(returnSymbol.SourceFile.Document, 0)).FindNodeAt(new TreeTextRange(new TreeOffset(returnSymbol.Offset), 1));
      while (element != null)
      {
        var ret = element as IDeclaredElement;
        if (ret != null)
          return ret;

        element = element.Parent;
      }
      return null;
    }

    public static IDeclaredElement GetPrimaryDeclaredElementForClass(IClass @class)
    {
      string className = @class.ShortName;
      var cache = @class.GetPsiServices().Solution.GetComponent<PsiCache>();
      List<IPsiSymbol> symbols = cache.GetSymbols(PsiRenamesFactory.NameFromCamelCase(className)).ToList();
      if (symbols.Count > 0)
      {
        IPsiSymbol symbol = symbols.ToArray()[0];
        ITreeNode element =
          symbol.SourceFile.GetPsiFile<PsiLanguage>(new DocumentRange(symbol.SourceFile.Document, 0)).FindNodeAt(new TreeTextRange(new TreeOffset(symbol.Offset), 1));
        while (element != null)
        {
          if (element is IDeclaredElement)
          {
            {
              return (IDeclaredElement)element;
            }
          }
          element = element.Parent;
        }
        {
          return null;
        }
      }
      return null;
    }

    public static IDeclaredElement GetPrimaryDeclaredElementForMethod(IMethod declaredElement)
    {
      string methodName = declaredElement.ShortName;
      if ("parse".Equals(methodName.Substring(0, "parse".Length)))
      {
        string ruleName = methodName.Substring("parse".Length, methodName.Length - "parse".Length);
        var cache = declaredElement.GetPsiServices().Solution.GetComponent<PsiCache>();
        List<IPsiSymbol> symbols = cache.GetSymbols(PsiRenamesFactory.NameFromCamelCase(ruleName)).ToList();
        if (symbols.Count > 0)
        {
          IPsiSymbol symbol = symbols.ToArray()[0];
          ITreeNode element = symbol.SourceFile.GetPsiFile<PsiLanguage>(new DocumentRange(symbol.SourceFile.Document, 0)).FindNodeAt(new TreeTextRange(new TreeOffset(symbol.Offset), 1));
          List<PsiOptionSymbol> parserPackageName = cache.GetOptionSymbols("parserPackage").ToList();
          List<PsiOptionSymbol> parserClassName = cache.GetOptionSymbols("parserClassName").ToList();
          IList<IDeclaredElement> classes = new List<IDeclaredElement>();
          foreach (PsiOptionSymbol packageName in parserPackageName)
          {
            foreach (PsiOptionSymbol className in parserClassName)
            {
              if (packageName.SourceFile == className.SourceFile)
              {
                IPsiSourceFile sourceFile = packageName.SourceFile;
                CollectionUtil.AddRange(
                classes, sourceFile.PsiModule.GetPsiServices().CacheManager.GetDeclarationsCache(sourceFile.PsiModule, false, true).
                    GetTypeElementsByCLRName(packageName.Value + "." + className.Value));
              }
            }
          }

          var parentClass = declaredElement.GetContainingType() as IClass;
          if (parentClass != null)
          {
            if (classes.Contains(parentClass))
            {
              while (element != null)
              {
                if (element is IDeclaredElement)
                {
                  {
                    return (IDeclaredElement)element;
                  }
                }
                element = element.Parent;
              }
            }
          }
          {
            return null;
          }
        }
      }
      else
      {
        return GetPrimaryDeclaredElementForVisitorMethod(declaredElement, declaredElement);
      }
      return null;
    }

    private static IDeclaredElement GetPrimaryDeclaredElementForVisitorMethod(IDeclaredElement declaredElement, IMethod method)
    {
      var cache = declaredElement.GetPsiServices().Solution.GetComponent<PsiCache>();
      Dictionary<ITypeElement, IList<PsiOptionSymbol>> classesWithInfo = GetVisitorClasses(cache);
      if (classesWithInfo.Count > 0)
      {
        var parentClass = method.GetContainingType() as IClass;
        if (parentClass != null)
        {
          if (classesWithInfo.ContainsKey(parentClass))
          {
            IList<PsiOptionSymbol> list = classesWithInfo[parentClass];
            PsiOptionSymbol visitorName = list[0];
            PsiOptionSymbol methodSuffix = list[1];
            PsiOptionSymbol methodPrefix = list[2];
            IPsiSourceFile sourceFile = visitorName.SourceFile;
            string name = method.ShortName;
            if (name.Length > methodPrefix.Value.Length + methodSuffix.Value.Length)
            {
              if (methodPrefix.Value.Length > 0)
              {
                name = name.Substring(methodPrefix.Value.Length, name.Length - methodPrefix.Value.Length);
              }
              if (methodSuffix.Value.Length > 0)
              {
                name = name.Substring(0, name.Length - methodSuffix.Value.Length);
              }
              IEnumerable<IPsiSymbol> elements = cache.GetSymbols(NameFromCamelCase(name));
              foreach (IPsiSymbol psiSymbol in elements)
              {
                if (psiSymbol.SourceFile == sourceFile)
                {
                  var psiFile = sourceFile.GetPsiFile<PsiLanguage>(new DocumentRange(psiSymbol.SourceFile.Document, 0)) as PsiFile;
                  if (psiFile != null)
                  {
                    IList<ISymbolInfo> infos = psiFile.FileRuleSymbolTable.GetSymbolInfos(name);
                    foreach (ISymbolInfo info in infos)
                    {
                      IDeclaredElement element = info.GetDeclaredElement();
                      if (element is RuleDeclaration)
                      {
                        var ruleDeclaration = element as RuleDeclaration;
                        {
                          return ruleDeclaration;
                        }
                      }
                    }
                  }
                }
              }
            }
            else
            {
              {
                return null;
              }
            }
          }
        }
      }
      {
        return null;
      }
    }

    private static Dictionary<ITypeElement, IList<PsiOptionSymbol>> GetVisitorClasses(PsiCache cache)
    {
      List<PsiOptionSymbol> visitorClassName = cache.GetOptionSymbols("visitorClassName").ToList();
      List<PsiOptionSymbol> visitorMetodSuffix = cache.GetOptionSymbols("visitorMethodSuffix").ToList();
      List<PsiOptionSymbol> visitorMetodPrefix = cache.GetOptionSymbols("\"visitMethodPrefix\"").ToList();
      List<PsiOptionSymbol> interfacesPackageName = cache.GetOptionSymbols("psiInterfacePackageName").ToList();
      var classes = new Dictionary<ITypeElement, IList<PsiOptionSymbol>>();
      foreach (PsiOptionSymbol visitorName in visitorClassName)
      {
        foreach (PsiOptionSymbol methodSuffix in visitorMetodSuffix)
        {
          foreach (PsiOptionSymbol methodPrefix in visitorMetodPrefix)
          {
            foreach (PsiOptionSymbol packageName in interfacesPackageName)
            {
              if ((visitorName.SourceFile == methodSuffix.SourceFile) && (methodPrefix.SourceFile == packageName.SourceFile) && (packageName.SourceFile == methodSuffix.SourceFile))
              {
                IPsiSourceFile sourceFile = visitorName.SourceFile;
                ICollection<ITypeElement> visitorClasses = new List<ITypeElement>();
                ICollection<ITypeElement> visitorNotGenericClasses = sourceFile.GetPsiServices().CacheManager.GetDeclarationsCache(sourceFile.PsiModule, false, true).GetTypeElementsByCLRName(packageName.Value + "." + visitorName.Value);
                foreach (ITypeElement typeElement in visitorNotGenericClasses)
                {
                  visitorClasses.Add(typeElement);
                }
                ICollection<ITypeElement> visitorGenericClasses =
                  sourceFile.GetPsiServices().CacheManager.GetDeclarationsCache(sourceFile.PsiModule, false, true).GetTypeElementsByCLRName(
                    packageName.Value + "." + visitorName.Value + "`1");
                foreach (ITypeElement visitorGenericClass in visitorGenericClasses)
                {
                  visitorClasses.Add(visitorGenericClass);
                }
                visitorGenericClasses =
                  sourceFile.GetPsiServices().CacheManager.GetDeclarationsCache(sourceFile.PsiModule, false, true).GetTypeElementsByCLRName(
                    packageName.Value + "." + visitorName.Value + "`2");
                foreach (ITypeElement visitorGenericClass in visitorGenericClasses)
                {
                  visitorClasses.Add(visitorGenericClass);
                }
                foreach (ITypeElement typeElement in visitorClasses)
                {
                  classes.Add(typeElement, new List<PsiOptionSymbol> { visitorName, methodSuffix, methodPrefix, packageName });
                }
              }
            }
          }
        }
      }
      return classes;
    }


    private static string NameFromCamelCase(string s)
    {
      string firstLetter = s.Substring(0, 1);
      firstLetter = firstLetter.ToLower();
      s = firstLetter + s.Substring(1, s.Length - 1);
      return s;
    }
  }
}
