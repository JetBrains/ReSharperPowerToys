using System.Collections.Generic;
using System.IO;
using System.Threading;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.dataStructures;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ExternalSources.Core;
using JetBrains.ReSharper.Feature.Services.ExternalSources.Utils;
using JetBrains.ReSharper.PowerToys.ReflectorNavigation.ReflectorAddin;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.UI.Application.Progress;
using JetBrains.UI.Options;
using JetBrains.UI.TaskDialog;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ReflectorNavigation
{
  [ExternalSourcesProvider]
  public class ReflectorExternalSourcesProvider : IExternalSourcesProvider
  {
    #region IExternalSourcesProvider Members

    public string PresentableShortName
    {
      get { return ReflectorConstants.PRESENTABLE_SHORT_NAME; }
    }

    public string Id
    {
      get { return ReflectorConstants.ID; }
    }

    public int DefaultPriority
    {
      get { return ReflectorConstants.DEFAULT_PRIORITY; }
    }

    public IOptionsPage CreateOptionsPage(IOptionsDialog optionsDialog)
    {
      return new ReflectorOptionsPage();
    }

    public IAssembly MapProjectFileToAssembly(IProjectFile projectFile)
    {
      DecompilationCache cache = DecompilationCache.GetInstance(projectFile.GetSolution());

      DecompilationCacheItem item = cache.GetCacheItem(projectFile.Location);
      if (item == null)
        return null;

      if (item.DecompilerId != ReflectorConstants.ID)
        return null;

      return item.Assembly;
    }

    public IEnumerable<FileSystemPath> NavigateToSources(ICompiledElement compiledElement)
    {
      var topLevelTypeElement = DeclaredElementUtil.GetTopLevelTypeElement(compiledElement);
      if (topLevelTypeElement == null)
        return EmptyArray<FileSystemPath>.Instance;

      var assemblyPsiModule = topLevelTypeElement.Module as IAssemblyPsiModule;
      if (assemblyPsiModule == null)
        return EmptyArray<FileSystemPath>.Instance;

      var assembly = assemblyPsiModule.Assembly;

      ReflectorOptions options = ReflectorOptions.Instance;

      ISolution solution = assembly.GetSolution();

      var reflectorExe = options.ReflectorExe.Value;
      if (string.IsNullOrEmpty(reflectorExe) || !File.Exists(reflectorExe))
      {
        const int browseForReflectorId = 100;
        const int continueWithoutReflectorId = 101;
        const int disablePluginId = 102;

        var dialogParams = new TaskDialogParams
        {
          AllowDialogCancellation = true,
          UseCommandLinks = true,
          Content = "Path to RedGate Reflector executable is not set. Please, select action to continue:",
          Buttons = new[]
                      {
                        new TaskDialogCustomButton(browseForReflectorId, "&Browse for Reflector.exe"), 
                        new TaskDialogCustomButton(continueWithoutReflectorId, "&Continue navigation without Reflector plugin"), 
                        new TaskDialogCustomButton(disablePluginId, "&Disable Reflector plugin"), 
                      },
          MainIcon = TaskDialogIcon.Question,
          WindowTitle = Shell.Instance.Descriptor.ProductFullName + ": Reflector Navigation",
        };

        var dialogResult = TaskDialog.Default.Show(dialogParams);
        switch (dialogResult)
        {
          case browseForReflectorId:
            string path = ReflectorOptionsPage.AskReflectorExePath(null);
            if (string.IsNullOrEmpty(path))
              return EmptyArray<FileSystemPath>.Instance;
            options.ReflectorExe.Value = path;
            break;

          case continueWithoutReflectorId:
            return EmptyArray<FileSystemPath>.Instance;

          case disablePluginId:
            ExternalSourcesOptions.Instance.DisableProvider(ReflectorConstants.ID);
            return EmptyArray<FileSystemPath>.Instance;

          default:
            throw new ProcessCancelledException();
        }
      }

      IPsiModule psiModule = PsiModuleManager.GetInstance(solution).GetPrimaryPsiModule(assembly);
      if (psiModule == null)
        return EmptyArray<FileSystemPath>.Instance;

      string ext = options.LanguageExtension.Value;

      DecompilationCache cache = DecompilationCache.GetInstance(solution);
      DecompilationCacheItem cacheItem = cache.GetCacheItem(ReflectorConstants.ID, assembly,
                                                            topLevelTypeElement.CLRName, ext);

      if (cacheItem != null && !cacheItem.Expired &&
          cacheItem.Flags.DumpToString() == GetFlags(options).DumpToString())
        return ImmutableArray.FromArguments(cacheItem.Location);

      IEnumerable<FileSystemPath> result = EmptyArray<FileSystemPath>.Instance;
      if (!UITaskExecutor.SingleThreaded.ExecuteTask(
        "Reflector", TaskCancelable.Yes,
        indicator =>
          {
            bool reflectorStarted = ReflectorClient.IsAvailable();

            int items = 1;
            if (!reflectorStarted) items++;
            if (options.PostReformat.Value) items++;

            indicator.Start(items, "Decompiling with Reflector");
            indicator.CheckForInterrupt();

            if (!reflectorStarted)
            {
              indicator.CurrentItemText = "Launching Reflector";

              if (!ReflectorClient.LaunchReflector(solution))
                return;

              indicator.CheckForInterrupt();
              Thread.Sleep(ReflectorConstants.WAIT_AFTER_LAUNCH);

              indicator.Advance(1);
            }

            indicator.CheckForInterrupt();
            indicator.CurrentItemText = topLevelTypeElement.CLRName + " : decompiling";

            string content = ReflectorClient.Decompile(assembly, topLevelTypeElement.CLRName, options.ShowXmlDoc.Value, ext);
            indicator.Advance(1);
            if (content == null)
              return;

            DecompilationCacheItem item = cache.PutCacheItem(
              ReflectorConstants.ID, assembly, topLevelTypeElement.CLRName,
              ext, GetFlags(options), content);
            if (item == null)
              return;

            result = ImmutableArray.FromArguments(item.Location);

            if (options.PostReformat.Value)
            {
              indicator.CheckForInterrupt();
              indicator.CurrentItemText = topLevelTypeElement.CLRName + " : reformatting";

              FileReformatter.ReformatFile(psiModule, item.Location, true, CSharpLanguageService.CSHARP); // TODO
              indicator.Advance(1);
            }
          }))
        throw new ProcessCancelledException();

      return result;
    }

    #endregion

    private static IDictionary<string, string> GetFlags(ReflectorOptions options)
    {
      return new Dictionary<string, string>
               {
                 {"ShowXmlDoc", options.ShowXmlDoc.Value.ToString()},
                 {"PostReformat", options.PostReformat.Value.ToString()},
               };
    }
  }
}