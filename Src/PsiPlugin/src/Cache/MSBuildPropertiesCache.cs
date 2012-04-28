using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.impl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Cache
{
  [SolutionComponent]
  internal class MSBuildPropertiesCache
  {
    private readonly ViewableProjectsCollection myViewableProjectsCollection;

    private readonly Dictionary<IProject, Dictionary<string, string>> myData;

    public MSBuildPropertiesCache(Lifetime lifetime, ViewableProjectsCollection viewableProjectsCollection)
    {
      myViewableProjectsCollection = viewableProjectsCollection;
      myData = new Dictionary<IProject, Dictionary<string, string>>();
      myViewableProjectsCollection.Projects.View(lifetime, project =>
                                                           {
                                                           }, project =>
                                                                {
                                                                  if (myData.ContainsKey(project))
                                                                    myData.Remove(project);
                                                                });
    }

    public string GetProjectPropertyByName(IProject project, string name)
    {
      Dictionary<string, string> cachedProperties;
      if (myData.TryGetValue(project, out cachedProperties))
      {
        string value;
        if(cachedProperties.TryGetValue(name, out value))
        {
          return value;
        }
      } else
      {
        cachedProperties = new Dictionary<string, string>();
        myData.Add(project, cachedProperties);
      }
      try
      {
        const string resolveassemblyreference = "ResolveAssemblyReferences";
        var projectFile = project.ProjectFile;
        if (projectFile == null)
          return null;
        
        var loadedProjects =
          Microsoft.Build.Evaluation.ProjectCollection.GlobalProjectCollection.GetLoadedProjects(
            projectFile.Location.FullPath).ToList();
        if (loadedProjects.Count != 1)
          return null;

        var loadedProject = loadedProjects[0];
        var projectInstance =
          Microsoft.Build.Execution.BuildManager.DefaultBuildManager.GetProjectInstanceForBuild(loadedProject);
        if (projectInstance.Build(resolveassemblyreference, EmptyList<Microsoft.Build.Framework.ILogger>.InstanceList))
        {
          var allProperties = projectInstance.Properties;
          foreach (var property in allProperties)
          {
            cachedProperties.Add(property.Name, property.EvaluatedValue);
          }
          var projectPropertyInstance = projectInstance.GetProperty(name);
          if (projectPropertyInstance != null) return projectPropertyInstance.EvaluatedValue;
        }

      }
      catch (Exception e)
      {
        Logger.LogExceptionSilently(e);
      }
      return "";
    }
  }
}
