using System;
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
using JetBrains.ReSharper.Psi.Web.Resolve;
using JetBrains.ReSharper.Psi.Web.Util;
using JetBrains.ReSharper.PsiPlugin.Cache;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  public static class PsiPathReferenceUtil
  {

    public static FileSystemPath GetRootPath([NotNull] IPathReference pathReference)
    {
      var project = pathReference.GetTreeNode().GetPsiModule().ContainingProjectModule as IProject;
      if (project != null)
        return WebFilesUtil.GetProjectPath(project);

      return FileSystemPath.Empty;
    }


    private static FileSystemPath GetBasePathBeforeMapping(IQualifiableReference pathReference)
    {
      var qualifier = pathReference.GetQualifier();
      if (qualifier == null)
      {
        var file = pathReference.GetTreeNode().GetSourceFile().ToProjectFile();
        Assertion.AssertNotNull(file, "file == null");
        return file.Location.Directory;
      }

      var reference = qualifier as IReference;
      if (reference != null)
      {
        var resolveResultWithInfo = (reference).Resolve();
        var pathDeclaredElement = resolveResultWithInfo.DeclaredElement as IPathDeclaredElement;
        if (pathDeclaredElement == null)
          return FileSystemPath.Empty;

        return pathDeclaredElement.Path;
      }

      var pathQualifier = qualifier as IPathQualifier;
      if (pathQualifier != null)
        return pathQualifier.Path;

      return FileSystemPath.Empty;
    }

    public static FileSystemPath GetBasePath(IQualifiableReference pathReference)
    {
      var basePathBeforeMapping = GetBasePathBeforeMapping(pathReference);
      var qualifierReference = pathReference.GetQualifier() as IReference;
      if (qualifierReference != null)
      {
        var resolveResultWithInfo = qualifierReference.Resolve();
        if (resolveResultWithInfo.ResolveErrorType == WebResolveErrorType.PATH_MAPPED)
        {
          var mapping = WebPathMappingManager.GetPathMapping((IPathReference)pathReference);
          var mappedPath = mapping.GetRealPaths(basePathBeforeMapping).FirstOrDefault();
          if (mappedPath != null)
            return mappedPath;
        }
      }

      return basePathBeforeMapping;
    }

    public static ISymbolTable GetReferenceSymbolTable(IPathReference pathReference, bool useReferenceName, bool includeHttpHandlers = true)
    {
      MSBuildPropertiesCache propertiesSearcher =
        pathReference.GetTreeNode().GetSolution().GetComponent<MSBuildPropertiesCache>();

      string productHomeDir = propertiesSearcher.GetProjectPropertyByName(pathReference.GetTreeNode().GetProject(),
                                                                    "ProductHomeDir");
      FileSystemPath basePath = new FileSystemPath(productHomeDir);
      //FileSystemPath basePath = GetBasePath(pathReference);
      if (basePath.IsEmpty)
      {
        return EmptySymbolTable.INSTANCE;
      }

      FolderQualifierInfo folderQualifierInfo = null;
      var psiServices = pathReference.GetTreeNode().GetPsiServices();
      var baseProjectFolder = psiServices.Solution.FindProjectItemsByLocation(basePath).FirstOrDefault() as IProjectFolder;
      if (baseProjectFolder != null)
        folderQualifierInfo = new FolderQualifierInfo(baseProjectFolder);

      var websiteRoot = GetRootPath(pathReference);
      var qualifier = pathReference.GetQualifier();
      if (useReferenceName)
      {
        PathDeclaredElement target = null;
        var name = pathReference.GetName();
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
              goto default;
            target = new PathDeclaredElement(PathDeclaredElement.ROOT_NAME, psiServices, websiteRoot);
            break;
          default:
            try
            {
              string parserGenOutputBase =
               propertiesSearcher.GetProjectPropertyByName(pathReference.GetTreeNode().GetProject(), "ParserGenOutputBase");
              var path = basePath.Combine(parserGenOutputBase + "\\" + name);
              target = new PathDeclaredElement(name,psiServices, path);
            }
            catch (InvalidPathException) { }
            catch (ArgumentException) { }
            break;
        }
        var table = new SymbolTable(psiServices, folderQualifierInfo != null ? new SymbolTableDependencySet(folderQualifierInfo) : null);
        if (target != null)
          table.AddSymbol(target, EmptySubstitution.INSTANCE, 1);
        return table;
      }

      var rootPath = (qualifier == null) ? websiteRoot : FileSystemPath.Empty;
      var symbolTableByPath = PathReferenceUtil.GetSymbolTableByPath(basePath, psiServices, basePath.Directory, rootPath, true);
      var basePathBeforeMapping = GetBasePathBeforeMapping(pathReference);
      if (!basePathBeforeMapping.IsNullOrEmpty())
      {
        var pathMapping = WebPathMappingManager.GetPathMapping(pathReference);
        var mappedPaths = pathMapping.GetAllPathPartsIn(basePathBeforeMapping).ToList();
        if (mappedPaths.Any())
        {
          var mappedPathsTable = new SymbolTable(psiServices, folderQualifierInfo != null ? new SymbolTableDependencySet(folderQualifierInfo) : null);
          foreach (var mappedPath in mappedPaths)
          {
            var declaredElement = new PathDeclaredElement(psiServices, mappedPath);
            mappedPathsTable.AddSymbol(declaredElement, EmptySubstitution.INSTANCE, 1);
          }
          symbolTableByPath = symbolTableByPath.Merge(mappedPathsTable);
        }
      }

      if (!includeHttpHandlers)
        return symbolTableByPath;

      var httpHandlersTable = new SymbolTable(psiServices);
      var projectFile = pathReference.GetTreeNode().GetSourceFile().ToProjectFile();

      return httpHandlersTable.Merge(symbolTableByPath);
    }

    private class SymbolTableDependencySet : ISymbolTableDependencySet
    {
      private readonly FolderQualifierInfo myQualifierInfo;

      public SymbolTableDependencySet(FolderQualifierInfo qualifierInfo)
      {
        myQualifierInfo = qualifierInfo;
      }

      public void AddDependenciesTo(IDependencyStore store, string accessName)
      {
        myQualifierInfo.AddDependencies(store, accessName);
      }

      public void AddUsingDependenciesTo(IDependencyStore store, string accessName)
      {
      }
    }

    public static ISymbolFilter[] GetSmartSymbolFilters(IPathReference pathReference)
    {
      return new ISymbolFilter[]
      {
        //new PathInWebsiteFilter(GetRootPath(pathReference))
      };
    }

    public static ISymbolFilter[] GetSmartSymbolFilters(IFileReference fileReference)
    {
      var filters = new LocalList<ISymbolFilter>(GetSmartSymbolFilters((IPathReference)fileReference));
      filters.Add(new FileFilters.IsProjectFileFilter(fileReference.ExpectedFileType));

      var extensions = fileReference.ExpectedExtensions;
      if (extensions.Count > 0)
        filters.Add(new FileFilters.ExtensionFilter(extensions));

      return filters.ToArray();
    }

    public static ISymbolFilter[] GetFolderSmartSymbolFilters(IPathReference folderReference)
    {
      return ArrayUtil.Add(GetSmartSymbolFilters(folderReference), FileFilters.IsProjectFolder);
    }

    public static ISymbolFilter[] GetCompletionFilters(IPathReference pathReference)
    {
      return new ISymbolFilter[]
      {
        //new PathInWebsiteFilter(GetRootPath(pathReference))
      };
    }

    public static ISymbolTable GetQualifierSymbolTable(IPathReference pathReference)
    {
      var declaredElement = pathReference.Resolve().DeclaredElement as IPathDeclaredElement;
      if (declaredElement != null)
      {
        var psiServices = pathReference.GetTreeNode().GetPsiServices();
        return PathReferenceUtil.GetSymbolTableByPath(declaredElement.Path, psiServices, declaredElement.Path.Directory, GetRootPath(pathReference), true);
      }

      return EmptySymbolTable.INSTANCE;
    }

    [NotNull]
    public static ISymbolFilter[] AddNoCircularPathReferenceFilter([NotNull] ISymbolFilter[] filters, [NotNull] IPathReference pathReference)
    {
      var projectFile = pathReference.GetTreeNode().GetSourceFile().ToProjectFile();
      if (projectFile == null)
        return filters;
      var self = projectFile.Location;
      return ArrayUtil.Add(filters, new FileFilters.PredicateFilter(path => path != self));
    }

    public static ResolveResultWithInfo CheckResolveResut(IIgnorablePathReference pathReference, ResolveResultWithInfo resolveResult)
    {
      var name = pathReference.GetName();
      var resolveResultWithInfo = FileResolveUtil.CheckResolveResut(resolveResult, name);
      if (pathReference.CanBeMappedOrIgnored)
      {
        var pathDeclaredElement = resolveResultWithInfo.DeclaredElement as IPathDeclaredElement;
        if (resolveResultWithInfo.ResolveErrorType != ResolveErrorType.OK && pathDeclaredElement != null && !pathDeclaredElement.Path.IsNullOrEmpty())
        {
          var mapping = WebPathMappingManager.GetPathMapping(pathReference);

          var pathState = mapping.GetPathState(pathDeclaredElement.Path);
          // mapped path
          if ((pathState & PathState.MAPPED) != 0)
          {
            var mappedPaths = mapping.GetRealPaths(pathDeclaredElement.Path);
            if (mappedPaths.Any())
              return new ResolveResultWithInfo(resolveResultWithInfo.Result, WebResolveErrorType.PATH_MAPPED);
          }

          // ignored path
          if ((pathState & PathState.IGNORED_OR_PART_OF) != 0)
            return new ResolveResultWithInfo(resolveResultWithInfo.Result, WebResolveErrorType.PATH_IGNORED);
        }
      }
      return resolveResultWithInfo;
    }

    public static FileSystemPath GetPathUnmapped(IIgnorablePathReference pathReference, IPathDeclaredElement pathDeclaredElement)
    {
      if (!pathReference.CanBeMappedOrIgnored)
        return pathDeclaredElement.Path;

      var pathMapping = WebPathMappingManager.GetPathMapping(pathReference).GetRealToWebPathMapping();

      for (var path = pathDeclaredElement.Path; !path.IsEmpty; path = path.Directory)
      {
        FileSystemPath webPath;
        if (pathMapping.TryGetValue(path, out webPath))
        {
          var tail = pathDeclaredElement.Path.ConvertToRelativePath(path);
          return webPath.Combine(tail);
        }
      }

      return pathDeclaredElement.Path;
    }
  }
}
