using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Dependencies;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.Psi.Web.PathMapping;
using JetBrains.ReSharper.Psi.Web.References;
using JetBrains.ReSharper.Psi.Web.Util;
using JetBrains.ReSharper.PsiPlugin.Cache;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  public static class PsiPathReferenceUtil
  {
    private static FileSystemPath GetRootPath([NotNull] IPathReference pathReference)
    {
      var project = pathReference.GetTreeNode().GetPsiModule().ContainingProjectModule as IProject;
      if (project != null)
      {
        return WebFilesUtil.GetProjectPath(project);
      }

      return FileSystemPath.Empty;
    }


    private static FileSystemPath GetBasePathBeforeMapping(IQualifiableReference pathReference)
    {
      IQualifier qualifier = pathReference.GetQualifier();
      if (qualifier == null)
      {
        IProjectFile file = pathReference.GetTreeNode().GetSourceFile().ToProjectFile();
        Assertion.AssertNotNull(file, "file == null");
        return file.Location.Directory;
      }

      var reference = qualifier as IReference;
      if (reference != null)
      {
        ResolveResultWithInfo resolveResultWithInfo = (reference).Resolve();
        var pathDeclaredElement = resolveResultWithInfo.DeclaredElement as IPathDeclaredElement;
        if (pathDeclaredElement == null)
        {
          return FileSystemPath.Empty;
        }

        return pathDeclaredElement.Path;
      }

      var pathQualifier = qualifier as IPathQualifier;
      if (pathQualifier != null)
      {
        return pathQualifier.Path;
      }

      return FileSystemPath.Empty;
    }

    public static ISymbolTable GetReferenceSymbolTable(IPathReference pathReference, bool useReferenceName, bool includeHttpHandlers = true)
    {
      var propertiesSearcher =
        pathReference.GetTreeNode().GetSolution().GetComponent<MSBuildPropertiesCache>();

      string productHomeDir = propertiesSearcher.GetProjectPropertyByName(pathReference.GetTreeNode().GetProject(),
        "ProductHomeDir");
      var basePath = new FileSystemPath(productHomeDir);
      if (basePath.IsEmpty)
      {
        return EmptySymbolTable.INSTANCE;
      }

      FolderQualifierInfo folderQualifierInfo = null;
      IPsiServices psiServices = pathReference.GetTreeNode().GetPsiServices();
      var baseProjectFolder = psiServices.Solution.FindProjectItemsByLocation(basePath).FirstOrDefault() as IProjectFolder;
      if (baseProjectFolder != null)
      {
        folderQualifierInfo = new FolderQualifierInfo(baseProjectFolder);
      }

      FileSystemPath websiteRoot = GetRootPath(pathReference);
      IQualifier qualifier = pathReference.GetQualifier();
      if (useReferenceName)
      {
        PathDeclaredElement target = null;
        string name = pathReference.GetName();
        switch (name)
        {
          case PathDeclaredElement.CURRENT_DIR_NAME:
            target = new PathDeclaredElement(PathDeclaredElement.CURRENT_DIR_NAME, psiServices, basePath);
            break;
          case PathDeclaredElement.LEVEL_UP_NAME:
            target = new PathDeclaredElement(PathDeclaredElement.LEVEL_UP_NAME, psiServices, basePath.Directory);
            break;
          case PathDeclaredElement.ROOT_NAME:
            if (qualifier != null)
            {
              goto default;
            }
            target = new PathDeclaredElement(PathDeclaredElement.ROOT_NAME, psiServices, websiteRoot);
            break;
          default:
            try
            {
              string parserGenOutputBase =
                propertiesSearcher.GetProjectPropertyByName(pathReference.GetTreeNode().GetProject(), "ParserGenOutputBase");
              FileSystemPath path = basePath.Combine(parserGenOutputBase + "\\" + name);
              target = new PathDeclaredElement(name, psiServices, path);
            }
            catch (InvalidPathException)
            {
            }
            catch (ArgumentException)
            {
            }
            break;
        }
        var table = new SymbolTable(psiServices, folderQualifierInfo != null ? new SymbolTableDependencySet(folderQualifierInfo) : null);
        if (target != null)
        {
          table.AddSymbol(target, EmptySubstitution.INSTANCE, 1);
        }
        return table;
      }

      FileSystemPath rootPath = (qualifier == null) ? websiteRoot : FileSystemPath.Empty;
      ISymbolTable symbolTableByPath = PathReferenceUtil.GetSymbolTableByPath(basePath, psiServices, basePath.Directory, rootPath, true);
      FileSystemPath basePathBeforeMapping = GetBasePathBeforeMapping(pathReference);
      if (!basePathBeforeMapping.IsNullOrEmpty())
      {
        IWebProjectPathMapping pathMapping = WebPathMappingManager.GetPathMapping(pathReference);
        List<FileSystemPath> mappedPaths = pathMapping.GetAllPathPartsIn(basePathBeforeMapping).ToList();
        if (mappedPaths.Any())
        {
          var mappedPathsTable = new SymbolTable(psiServices, folderQualifierInfo != null ? new SymbolTableDependencySet(folderQualifierInfo) : null);
          foreach (FileSystemPath mappedPath in mappedPaths)
          {
            var declaredElement = new PathDeclaredElement(psiServices, mappedPath);
            mappedPathsTable.AddSymbol(declaredElement, EmptySubstitution.INSTANCE, 1);
          }
          symbolTableByPath = symbolTableByPath.Merge(mappedPathsTable);
        }
      }

      if (!includeHttpHandlers)
      {
        return symbolTableByPath;
      }

      var httpHandlersTable = new SymbolTable(psiServices);

      return httpHandlersTable.Merge(symbolTableByPath);
    }

    #region Nested type: SymbolTableDependencySet

    private class SymbolTableDependencySet : ISymbolTableDependencySet
    {
      private readonly FolderQualifierInfo myQualifierInfo;

      public SymbolTableDependencySet(FolderQualifierInfo qualifierInfo)
      {
        myQualifierInfo = qualifierInfo;
      }

      #region ISymbolTableDependencySet Members

      public void AddDependenciesTo(IDependencyStore store, string accessName)
      {
        myQualifierInfo.AddDependencies(store, accessName);
      }

      #endregion
    }

    #endregion
  }
}
