using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using JetBrains.dotTrace.Api;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.Threading;
using JetBrains.UI;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.dotTrace31
{
  /// <summary>
  /// Host provider represents information about available host
  /// </summary>
  [UnitTestHostProvider]
  public class ProfilerHostProvider : IHostProvider
  {
    private const string ProfileProviderID = "Profile31";

    public string ID { get { return ProfileProviderID; } }

    public int Priority
    {
      get { return -1; }
    }

    public Image Icon
    {
      get { return ImageLoader.GetImage("profile"); }
    }

    public string Format
    {
      get { return "Profile {0} with dotTrace 3.1"; }
    }

    public bool Available(UnitTestElement element)
    {
      return ProfilerTaskRunnerHostController.IsAvailable;
    }
    
    public ITaskRunnerHostController CreateHostController(UnitTestManager manager)
    {
      return new ProfilerTaskRunnerHostController(manager);
    }
  }

  public class ProfilerTaskRunnerHostController : TaskRunnerHostControllerBase
  {
    private string myRunId;
    private InstalledProfiler myProfiler;
    private string myRemotingAddress;
    private static readonly bool ourHasProfiler;

    static ProfilerTaskRunnerHostController()
    {
      try
      {
        // Use dotTrace 3.x integration API to discover installed profilers
        ourHasProfiler = InstalledProfiler.Gather().Length > 0;
      }
      catch (Exception e)
      {
        // This posts exception to log file without showing it to the user
        Logger.LogException(e);
        ourHasProfiler = false;
      }
    }

    public ProfilerTaskRunnerHostController(UnitTestManager manager)
      : base(manager)
    {
    }

    public static bool IsAvailable
    {
      get { return ourHasProfiler; }
    }

    public override void Run(string remotingAddress, string runId)
    {
      myRemotingAddress = remotingAddress;
      myRunId = runId;

      // We need to re-check available profilers as the list might have changed since ReSharper start-up
      InstalledProfiler[] profilers = InstalledProfiler.Gather();
      if (profilers.Length > 0)
      {
        myProfiler = profilers[0];

        // Thread manager pools threads similarly to ThreadPool, but if all pooled threads are busy it creates a new thread right away
        ThreadManager.Instance.ExecuteTask(ThreadProc);
      }
      else
      {
        Manager.GetSessionByRun(myRunId).TaskFinished(null, "Profiler not found", TaskResult.Error);
        Manager.GetRun(myRunId).Stopped();
        MessageBox.ShowError("Profiler not found.", "Unit Testing");
      }
    }

    public override void Abort()
    {
      Debug.Assert(myProfiler != null);
      try
      {
        myProfiler.Kill();
      }
      catch (Exception e)
      {
        // This forwards exception to ReSharper exception handling subsystem
        Logger.LogException(e);
      }
    }

    /// <summary>
    /// Provides information about client controller object which will be loaded into task runner process
    /// </summary>
    public override TaskRunnerClientControllerInfo GetClientControllerInfo()
    {
      return new TaskRunnerClientControllerInfo(typeof(ClientController));
    }

    private void ThreadProc()
    {
      bool browseStarted = false;
      string tempFilePath = Path.GetTempFileName() + ".dtc";

      try
      {
        FileSystemPath path = TaskRunnerPath;
        // The following call is asynchronous
        myProfiler.StartCPU(path.FullPath, new [] { myRemotingAddress, myRunId }, path.Directory.FullPath, tempFilePath, true, false);
        myProfiler.WaitForExit();

        // Check if profiler actually produced the snapshot
        if (File.Exists(tempFilePath))
        {
          // Open snapshot in dotTrace          
          myProfiler.Browse(tempFilePath, true);
          browseStarted = true;
        }
        else
          MessageBox.ShowError("Profiler didn't produce snapshot", "dotTrace 3");
      }
      catch (Exception e)
      {
        Logger.LogException(e);
        try
        {
          myProfiler.Kill();
        }
        catch
        {
        }
      }

      // Cleanup
      try
      {
        Manager.GetRun(myRunId).Stopped();
        if (!browseStarted && File.Exists(tempFilePath))
          File.Delete(tempFilePath);
        myProfiler = null;
      }
      catch (Exception e)
      {
        Logger.LogException(e);
      }
    }

    /// <summary>
    /// Client controller is run in the task runner process and operates dotTrace to only include call stacks which are part of test execution
    /// </summary>
    [Serializable]
    private class ClientController : ITaskRunnerClientController
    {
      public string BeforeRunStarted()
      {
        return null;
      }

      public string AfterRunFinished()
      {
        CPUProfiler.StopAndSaveSnapShot();
        return null;
      }

      public string BeforeTaskStarted(RemoteTask task)
      {
        CPUProfiler.Start();
        return null;
      }

      public string AfterTaskFinished(RemoteTask task)
      {
        CPUProfiler.Stop();
        return null;
      }
    }
  }
}