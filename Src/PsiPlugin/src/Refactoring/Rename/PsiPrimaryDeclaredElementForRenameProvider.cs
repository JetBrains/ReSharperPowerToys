using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.PsiPlugin.Cache;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.ReSharper.Refactorings.Rename;
using JetBrains.Util;
using System.Linq;

namespace JetBrains.ReSharper.PsiPlugin.Refactoring.Rename
{
  [RenamePart]
  internal class PsiPrimaryDeclaredElementForRenameProvider : IPrimaryDeclaredElementForRenameProvider
  {
    public IDeclaredElement GetPrimaryDeclaredElement(IDeclaredElement declaredElement, IReference reference)
    {
      if (declaredElement is IMethod)
      {
        return GetPrimaryDeclaredElementForMethod(declaredElement);
      }

      if(declaredElement is IClass)
      {
        return PrimaryDeclaredElementForClass(declaredElement);
      }

      if (declaredElement is IInterface)
      {
        return PrimaryDeclaredElementForInterface(declaredElement);
      }
      return declaredElement;
    }

    private static IDeclaredElement PrimaryDeclaredElementForInterface(IDeclaredElement declaredElement)
    {
      var interfaceElement = declaredElement as IInterface;
      string interfaceName = interfaceElement.ShortName;
      var cache = declaredElement.GetPsiServices().Solution.GetComponent<PsiCache>();
      var prefixes = cache.GetOptionSymbols("\"interfaceNamePrefix\"");
      IList<IPsiSymbol> symbols = new List<IPsiSymbol>();
      foreach (var prefix in prefixes)
      {
        var prefixValue = prefix.Value;
        if((prefixValue.Length < interfaceName.Length)&&(prefixValue.Equals(interfaceName.Substring(0,prefixValue.Length))))
        {
          var shortInterfaceName = PsiRenamesFactory.NameFromCamelCase(interfaceName.Substring(prefixValue.Length, interfaceName.Length - prefixValue.Length));
          var allSymbols = cache.GetSymbols(PsiRenamesFactory.NameFromCamelCase(shortInterfaceName)).ToList();
          foreach (var symbol in allSymbols)
          {
            if(symbol.SourceFile == prefix.SourceFile)
            {
              symbols.Add(symbol);
            }
          }
        }
      }

      if(symbols.Count == 0)
      {
        return declaredElement;
      }


      IPsiSymbol returnSymbol = symbols.ToArray()[0];
      var element =
        returnSymbol.SourceFile.GetPsiFile<PsiLanguage>().FindNodeAt(new TreeTextRange(new TreeOffset(returnSymbol.Offset), 1));
      while (element != null)
      {
        if (element is IDeclaredElement)
        {
          return (IDeclaredElement) element;
        }
        element = element.Parent;
      }
      return declaredElement;

    }

    private static IDeclaredElement PrimaryDeclaredElementForClass(IDeclaredElement declaredElement)
    {
      var classElement = declaredElement as IClass;
      string className = classElement.ShortName;
      var cache = declaredElement.GetPsiServices().Solution.GetComponent<PsiCache>();
      var symbols = cache.GetSymbols(PsiRenamesFactory.NameFromCamelCase(className)).ToList();
      if (symbols.Count > 0)
      {
        IPsiSymbol symbol = symbols.ToArray()[0];
        var element =
          symbol.SourceFile.GetPsiFile<PsiLanguage>().FindNodeAt(new TreeTextRange(new TreeOffset(symbol.Offset), 1));
        while (element != null)
        {
          if (element is IDeclaredElement)
          {
            {
              return (IDeclaredElement) element;
            }
          }
          element = element.Parent;
        }
        {
          return declaredElement;
        }
      }
      return declaredElement;
    }

    private IDeclaredElement GetPrimaryDeclaredElementForMethod(IDeclaredElement declaredElement)
    {
      var method = declaredElement as IMethod;
      string methodName = method.ShortName;
      if ("parse".Equals(methodName.Substring(0, "parse".Length)))
      {
        string ruleName = methodName.Substring("parse".Length, methodName.Length - "parse".Length);
        var cache = declaredElement.GetPsiServices().Solution.GetComponent<PsiCache>();
        var symbols = cache.GetSymbols(PsiRenamesFactory.NameFromCamelCase(ruleName)).ToList();
        if (symbols.Count > 0)
        {
          IPsiSymbol symbol = symbols.ToArray()[0];
          var element = symbol.SourceFile.GetPsiFile<PsiLanguage>().FindNodeAt(new TreeTextRange(new TreeOffset(symbol.Offset), 1));
          var parserPackageName = cache.GetOptionSymbols("parserPackage").ToList();
          var parserClassName = cache.GetOptionSymbols("parserClassName").ToList();
          IList<IDeclaredElement> classes = new List<IDeclaredElement>();
          foreach (var packageName in parserPackageName)
          {
            foreach (var className in parserClassName)
            {
              if (packageName.SourceFile == className.SourceFile)
              {
                var sourceFile = packageName.SourceFile;
                classes.AddRange(
                  sourceFile.PsiModule.GetPsiServices().CacheManager.GetDeclarationsCache(sourceFile.PsiModule, false, true).
                    GetTypeElementsByCLRName(packageName.Value + "." + className.Value));
              }
            }
          }

          var parentClass = method.GetContainingType() as IClass;
          if (parentClass != null)
          {
            if (classes.Contains(parentClass))
            {
              while (element != null)
              {
                if (element is IDeclaredElement)
                {
                  {
                    return (IDeclaredElement) element;
                  }
                }
                element = element.Parent;
              }
            }
          }
          {
            return declaredElement;
          }
        }
      }
      else
      {
        return PrimaryDeclaredElementForVisitorMethod(declaredElement, method);
      }
      return declaredElement;
    }

    private IDeclaredElement PrimaryDeclaredElementForVisitorMethod(IDeclaredElement declaredElement, IMethod method)
    {
      var cache = declaredElement.GetPsiServices().Solution.GetComponent<PsiCache>();
      var classesWithInfo = GetVisitorClasses(cache);
      if (classesWithInfo.Count > 0)
      {
        var parentClass = method.GetContainingType() as IClass;
        if (parentClass != null)
        {
          if (classesWithInfo.ContainsKey(parentClass))
          {
            var list = classesWithInfo[parentClass];
            var visitorName = list[0];
            var methodSuffix = list[1];
            var methodPrefix = list[2];
            var sourceFile = visitorName.SourceFile;
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
              var elements = cache.GetSymbols(NameFromCamelCase(name));
              foreach (var psiSymbol in elements)
              {
                if (psiSymbol.SourceFile == sourceFile)
                {
                  var psiFile = sourceFile.GetPsiFile<PsiLanguage>() as PsiFile;
                  if (psiFile != null)
                  {
                    IList<ISymbolInfo> infos = psiFile.FileRuleSymbolTable.GetSymbolInfos(name);
                    foreach (ISymbolInfo info in infos)
                    {
                      var element = info.GetDeclaredElement();
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
                return declaredElement;
              }
            }
          }
        }
      }
      {
        return declaredElement;
      }
    }

    private static Dictionary<ITypeElement, IList<PsiOptionSymbol>> GetVisitorClasses(PsiCache cache)
    {
      var visitorClassName = cache.GetOptionSymbols("visitorClassName").ToList();
      var visitorMetodSuffix = cache.GetOptionSymbols("visitorMethodSuffix").ToList();
      var visitorMetodPrefix = cache.GetOptionSymbols("\"visitMethodPrefix\"").ToList();
      var interfacesPackageName = cache.GetOptionSymbols("psiInterfacePackageName").ToList();
      var classes = new Dictionary<ITypeElement, IList<PsiOptionSymbol>>();
      foreach (var visitorName in visitorClassName)
      {
        foreach (var methodSuffix in visitorMetodSuffix)
        {
          foreach (var methodPrefix in visitorMetodPrefix)
          {
            foreach (var packageName in interfacesPackageName)
            {
              if ((visitorName.SourceFile == methodSuffix.SourceFile) && (methodPrefix.SourceFile == packageName.SourceFile) && (packageName.SourceFile == methodSuffix.SourceFile))
              {
                var sourceFile = visitorName.SourceFile;
                ICollection<ITypeElement> visitorClasses = new List<ITypeElement>();
                var visitorNotGenericClasses = sourceFile.GetPsiServices().CacheManager.GetDeclarationsCache(sourceFile.PsiModule, false, true).GetTypeElementsByCLRName(packageName.Value + "." + visitorName.Value);
                foreach (var typeElement in visitorNotGenericClasses)
                {
                  visitorClasses.Add(typeElement);
                }
                var visitorGenericClasses =
                  sourceFile.GetPsiServices().CacheManager.GetDeclarationsCache(sourceFile.PsiModule, false, true).GetTypeElementsByCLRName(
                    packageName.Value + "." + visitorName.Value + "`1");
                foreach (var visitorGenericClass in visitorGenericClasses)
                {
                  visitorClasses.Add(visitorGenericClass);
                }
                visitorGenericClasses =
                  sourceFile.GetPsiServices().CacheManager.GetDeclarationsCache(sourceFile.PsiModule, false, true).GetTypeElementsByCLRName(
                    packageName.Value + "." + visitorName.Value + "`2");
                foreach (var visitorGenericClass in visitorGenericClasses)
                {
                  visitorClasses.Add(visitorGenericClass);
                }
                foreach (var typeElement in visitorClasses)
                {
                  classes.Add(typeElement, new List<PsiOptionSymbol> {visitorName, methodSuffix, methodPrefix, packageName});
                }
              }
            }
          }
        }
      }
      return classes;
    }


    private string NameFromCamelCase(string s)
    {
      string firstLetter = s.Substring(0, 1);
      firstLetter = firstLetter.ToLower();
      s = firstLetter + s.Substring(1, s.Length - 1);
      return s;
    }
  }
}