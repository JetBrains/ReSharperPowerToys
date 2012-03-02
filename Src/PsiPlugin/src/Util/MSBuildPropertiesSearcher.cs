using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ProjectModel;
using JetBrains.Util;
using Microsoft.Build.Evaluation;

namespace JetBrains.ReSharper.PsiPlugin.Util
{
  [SolutionComponent]
  public class MSBuildPropertiesSearcher
  {
    private IDictionary<IProject, IDictionary<string, string>> cache = new Dictionary<IProject, IDictionary<string, string>>();

    public string GetProperty(IProject project, string name)
    {
      IDictionary<string, string> properties;
      if (cache.ContainsKey(project))
      {
        properties = cache.GetValue(project);
        if (properties.ContainsKey(name))
        {
          return properties.GetValue(name);
        } else
        {
          return "";
        }
      }
      else
      {
        properties = new Dictionary<string, string>();
        cache.Add(project, properties);
        const string resolveassemblyreference = "ResolveAssemblyReferences";
        /*
              if (!((VsSolutionManager10)SolutionManager).VsAslSupported)
                return null;
        */

        try
        {
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
              properties.Add(property.Name, property.EvaluatedValue);
            }
            return projectInstance.GetProperty(name).EvaluatedValue;
          }

        }
        catch (Exception e)
        {
          Logger.LogExceptionSilently(e);
        }
        return null;
      }
    }
  }
}
