using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Cache;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.ReSharper.Refactorings.Conflicts;
using JetBrains.ReSharper.Refactorings.Rename;
using JetBrains.ReSharper.Refactorings.RenameModel;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Refactoring.Rename
{
  [FeaturePart]
  public class PsiRenamesFactory : AtomicRenamesFactory
  {
    public override bool IsApplicable(IDeclaredElement declaredElement)
    {
      if(declaredElement.PresentationLanguage.Is<PsiLanguage>())
      {
        return true;
      }
      return false;
    }

    /*internal bool isCSharpPsiMember(IDeclaredElement declaredElement)
    {
      if(declaredElement.PresentationLanguage.Is<PsiLanguage>()){
        if(declaredElement is CSharpMethod)
        {
          var method = declaredElement as CSharpMethod;
          string name = method.ShortName;
          return (name.Length > "parse".Length) && ("parse".Equals(name.Substring(0, "parse".Length)));
        }

        return ((declaredElement is IClass) || (declaredElement is IInterface));
     }
     return false;
    }*/

    public override IEnumerable<AtomicRenameBase> CreateAtomicRenames(IDeclaredElement declaredElement, string newName, bool doNotAddBindingConflicts)
    {
      yield return new PsiAtomicRename(declaredElement, newName, doNotAddBindingConflicts);
      if (declaredElement is RuleDeclaration)
      {
        var ruleDeclaration = declaredElement as RuleDeclaration;
        if (ruleDeclaration != null)
        {
          ruleDeclaration.UpdateDerivedDeclaredElements();
          foreach (IDeclaredElement element in ruleDeclaration.DerivedDeclaredElements)
          {
            yield return new PsiDerivedElementRename(element, "parse" + NameToCamelCase(newName),
                                                                         doNotAddBindingConflicts);
          }
          foreach (IDeclaredElement element in ruleDeclaration.DerivedClasses)
          {
            yield return new PsiDerivedElementRename(element, NameToCamelCase(newName),
                                                                         doNotAddBindingConflicts);
          }
          foreach (IDeclaredElement element in ruleDeclaration.DerivedInterfaces)
          {
            yield return new PsiDerivedElementRename(element, "I" + NameToCamelCase(newName),
                                                                         doNotAddBindingConflicts);
          }
          foreach(IDeclaredElement element in ruleDeclaration.DerivedVisitorMethods)
          {
            yield return new PsiDerivedElementRename(element, ruleDeclaration.VisitorMethodPrefix + NameToCamelCase(newName) + ruleDeclaration.VisitorMethodSuffix,
                                                                        doNotAddBindingConflicts);           
          }
        }
      }
    }

    public override RenameAvailabilityCheckResult CheckRenameAvailability(IDeclaredElement element)
    {
      return RenameAvailabilityCheckResult.CanBeRenamed;
    }

    public static string NameToCamelCase(string s)
    {
      string firstLetter = s.Substring(0, 1);
      firstLetter = firstLetter.ToUpper();
      s = firstLetter + s.Substring(1, s.Length - 1);
      return s;
    }

    public static string NameFromCamelCase(string s)
    {
      string firstLetter = s.Substring(0, 1);
      firstLetter = firstLetter.ToLower();
      s = firstLetter + s.Substring(1, s.Length - 1);
      return s;
    }

  }

  [RenamePart]
  internal class PsiPrimaryDeclaredElementForRenameProvider : IPrimaryDeclaredElementForRenameProvider
  {
    public IDeclaredElement GetPrimaryDeclaredElement(IDeclaredElement declaredElement, IReference reference)
    {
      if (declaredElement is IMethod)
      {
        IMethod method = declaredElement as IMethod;
        string methodName = method.ShortName;
        if ("parse".Equals(methodName.Substring(0, "parse".Length)))
        {
          string ruleName = methodName.Substring("parse".Length, methodName.Length - "parse".Length);
          var cache = declaredElement.GetPsiServices().Solution.GetComponent<PsiCache>();
          ICollection<IPsiSymbol> symbols = cache.GetSymbol(PsiRenamesFactory.NameFromCamelCase(ruleName));
          if (symbols.Count > 0)
          {
            IPsiSymbol symbol = symbols.ToArray()[0];
            var element =
              symbol.SourceFile.GetPsiFile<PsiLanguage>().FindNodeAt(new TreeTextRange(new TreeOffset(symbol.Offset), 1));
            ICollection<IPsiSymbol> parserPackageName = cache.GetSymbol("parserPackage");
            ICollection<IPsiSymbol> parserClassName = cache.GetSymbol("parserClassName");
            IList<IDeclaredElement> classes = new List<IDeclaredElement>();
            foreach (var packageName in parserPackageName)
            {
              foreach (var className in parserClassName)
              {
                if (packageName.SourceFile == className.SourceFile)
                {
                  var sourceFile = packageName.SourceFile;
                  classes.AddRange(
                    sourceFile.PsiModule.GetPsiServices().CacheManager.GetDeclarationsCache(sourceFile.PsiModule, false,true).
                      GetTypeElementsByCLRName(packageName.Value + "." + className.Value));
                }
              }
            }

            IClass parentClass = method.GetContainingType() as IClass;
            if (parentClass != null)
            {
              if (classes.Contains(parentClass))
              {

                while (element != null)
                {
                  if (element is IDeclaredElement)
                  {
                    return (IDeclaredElement) element;
                  }
                  element = element.Parent;
                }
              }
            }
            return declaredElement;
          }
        }
        else
        {
          var cache = declaredElement.GetPsiServices().Solution.GetComponent<PsiCache>();
          ICollection<IPsiSymbol> visitorClassName = cache.GetSymbol("visitorClassName");
          ICollection<IPsiSymbol> visitorMetodSuffix = cache.GetSymbol("visitorMethodSuffix");
          ICollection<IPsiSymbol> visitorMetodPrefix = cache.GetSymbol("\"visitMethodPrefix\"");
          ICollection<IPsiSymbol> interfacesPackageName = cache.GetSymbol("psiInterfacePackageName");
          Dictionary<ITypeElement, IList<IPsiSymbol>> classes = new Dictionary<ITypeElement, IList<IPsiSymbol>>();
          foreach (var visitorName in visitorClassName)
          {
            foreach (var methodSuffix in visitorMetodSuffix)
            {
              foreach (var methodPrefix in visitorMetodPrefix)
              {
                foreach (var packageName in interfacesPackageName)
                {
                  if((visitorName.SourceFile == methodSuffix.SourceFile) && (methodPrefix.SourceFile == packageName.SourceFile) && (packageName.SourceFile == methodSuffix.SourceFile))
                  {
                    var sourceFile = visitorName.SourceFile;
                    ICollection<ITypeElement> visitorClasses = new List<ITypeElement>();
                    foreach (var typeElement in sourceFile.GetPsiServices().CacheManager.GetDeclarationsCache(sourceFile.PsiModule, false, true).GetTypeElementsByCLRName(
                        packageName.Value + "." + visitorName.Value))
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
                      classes.Add(typeElement, new List<IPsiSymbol>(){visitorName, methodSuffix, methodPrefix, packageName});                     
                    }
                  }
                }
              }
            }
          }
          if(classes.Count > 0)
          {
            IClass parentClass = method.GetContainingType() as IClass;
            if(parentClass != null)
            {
              if(classes.ContainsKey(parentClass))
              {
                var list = classes[parentClass];
                var visitorName = list[0];
                var methodSuffix = list[1];
                var methodPrefix = list[2];
                var interfacesPackage = list[3];
                var sourceFile = visitorName.SourceFile;
                string name = method.ShortName;
                if(name.Length > methodPrefix.Value.Length + methodSuffix.Value.Length)
                {
                  if(methodPrefix.Value.Length > 0)
                  {
                    name = name.Substring(methodPrefix.Value.Length, name.Length - methodPrefix.Value.Length);
                  }
                  if(methodSuffix.Value.Length > 0)
                  {
                    name = name.Substring(0, name.Length - methodSuffix.Value.Length);
                  }
                  var elements = cache.GetSymbol(NameFromCamelCase(name));
                  foreach (var psiSymbol in elements)
                  {
                    if(psiSymbol.SourceFile == sourceFile)
                    {
                      PsiFile psiFile = sourceFile.GetPsiFile<PsiLanguage>() as PsiFile;
                      IList<ISymbolInfo> infos = psiFile.FileRuleSymbolTable.GetSymbolInfos(name);
                      foreach (ISymbolInfo info in infos)
                      {
                        var element = info.GetDeclaredElement();
                        if(element is RuleDeclaration)
                        {
                          RuleDeclaration ruleDeclaration= element as RuleDeclaration;
                          //if(ruleDeclaration.DerivedVisitorMethods.Contains(method))
                          //{
                            return ruleDeclaration;
                          //}
                        }
                      }
                    }
                  }
                } else
                {
                  return declaredElement;
                }
              }
            }
          } 
          return declaredElement;
        }
      }

      if(declaredElement is IClass)
      {
        IClass classElement = declaredElement as IClass;
        string className = classElement.ShortName;
        var cache = declaredElement.GetPsiServices().Solution.GetComponent<PsiCache>();
        ICollection<IPsiSymbol> symbols = cache.GetSymbol(PsiRenamesFactory.NameFromCamelCase(className));
        if (symbols.Count > 0)
        {
          IPsiSymbol symbol = symbols.ToArray()[0];
          var element =
            symbol.SourceFile.GetPsiFile<PsiLanguage>().FindNodeAt(new TreeTextRange(new TreeOffset(symbol.Offset), 1));
          while (element != null)
          {
            if (element is IDeclaredElement)
            {
              return (IDeclaredElement)element;
            }
            element = element.Parent;
          }
          return declaredElement;
        }
      }

      if (declaredElement is IInterface)
      {
        IInterface interfaceElement = declaredElement as IInterface;
        string interfaceName = interfaceElement.ShortName;
        interfaceName = interfaceName.Substring(1, interfaceName.Length - 1);
        var cache = declaredElement.GetPsiServices().Solution.GetComponent<PsiCache>();
        ICollection<IPsiSymbol> symbols = cache.GetSymbol(PsiRenamesFactory.NameFromCamelCase(interfaceName));
        if (symbols.Count > 0)
        {
          IPsiSymbol symbol = symbols.ToArray()[0];
          var element =
            symbol.SourceFile.GetPsiFile<PsiLanguage>().FindNodeAt(new TreeTextRange(new TreeOffset(symbol.Offset), 1));
          while (element != null)
          {
            if (element is IDeclaredElement)
            {
              return (IDeclaredElement)element;
            }
            element = element.Parent;
          }
          return declaredElement;
        }
      }
      return declaredElement;
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