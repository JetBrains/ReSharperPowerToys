using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using JetBrains.dotTrace.Integration.Utils;
using JetBrains.Util;
using Microsoft.Win32;

namespace JetBrains.ReSharper.PowerToys.dotTrace31
{
  public sealed class InstalledProfiler
  {
    private const string EXECUTABLE = "JetBrains.dotTrace.exe";
    private const string INTEGRATION = "JetBrains.dotTrace.exe.integration";

    private readonly string Location;

    private readonly string myStartCPU_ImmediatelyShow;
    private readonly string myStartCPU_ImmediatelyHide;
    private readonly string myStartCPU_ProfileSolution;
	
    private readonly string myStartCPU_ManualShow;
    private readonly string myStartCPU_ManualHide;

    private readonly string myOpen_Permanent;
    private readonly string myOpen_Temporary;

    private InstalledProfiler(string location)
    {
      var doc = new XmlDocument();
      doc.Load(Path.Combine(location, INTEGRATION));
      XmlNode root = doc.SelectSingleNode("Integration");
      switch (root.Attributes["Version"].InnerText)
      {
        case "1.0":
          {
            XmlAttributeCollection startCPUAttributes = root.SelectSingleNode("Start").SelectSingleNode("CPU").Attributes;
            myStartCPU_ImmediatelyShow = startCPUAttributes["ImmediatelyShow"].InnerText;

            if (startCPUAttributes["ProfileSolution"] != null)
              myStartCPU_ProfileSolution = startCPUAttributes["ProfileSolution"].InnerText;
            else
              myStartCPU_ProfileSolution = null; // dotTrace 1.1 didn't have this xml node

            myStartCPU_ImmediatelyHide = startCPUAttributes["ImmediatelyHide"].InnerText;
            myStartCPU_ManualShow = startCPUAttributes["ManualShow"].InnerText;
            myStartCPU_ManualHide = startCPUAttributes["ManualHide"].InnerText;

            XmlAttributeCollection openAttributes = root.SelectSingleNode("Open").Attributes;
            myOpen_Permanent = openAttributes["Permanent"].InnerText;
            myOpen_Temporary = openAttributes["Temporary"].InnerText;
            break;
          }

        default:
          throw new NotSupportedException("Invalid integration file version");
      }

      Location = location;
    }

    private Process myProcess;

    public void StartCPU(string application, string[] arguments, string directory, string snapshot, bool hideConsole,
                         bool profileeImmediately)
    {
      StartCPU(
        CommandLine.QuoteIfNeed(application) + (arguments.Length > 0 ? " " + CommandLine.ToString(arguments) : string.Empty),
        directory, snapshot, hideConsole, profileeImmediately, false);
    }

    private void StartCPU(string commandline, string directory, string snapshot, bool hideConsole, 
                          bool profileeImmediately, bool profSolMode)
    {
      if (myProcess != null)
        throw new ApplicationException("Already started");

      if (directory[directory.Length - 1] == '\\')
        directory = directory.Substring(0, directory.Length - 1);

      try
      {
        myProcess = new Process {EnableRaisingEvents = true};
        myProcess.Exited += myProcess_Exited;
        myProcess.StartInfo = new ProcessStartInfo
                                {
                                  CreateNoWindow = true, 
                                  UseShellExecute = false, 
                                  FileName = Path.Combine(Location, EXECUTABLE)
                                };
        if (profSolMode)
        {
          myProcess.StartInfo.Arguments = string.Format(
            myStartCPU_ProfileSolution, 
            (!string.IsNullOrEmpty(directory) ? CommandLine.QuoteIfNeed(directory) : "."),
            commandline);

        }
        else
        {
          myProcess.StartInfo.Arguments = string.Format(
            (profileeImmediately
               ? (hideConsole ? myStartCPU_ImmediatelyHide : myStartCPU_ImmediatelyShow)
               : (hideConsole ? myStartCPU_ManualHide : myStartCPU_ManualShow)),
            CommandLine.QuoteIfNeed(snapshot),
            (!string.IsNullOrEmpty(directory) ? CommandLine.QuoteIfNeed(directory) : "."),
            commandline);
        }
        myProcess.StartInfo.WorkingDirectory = directory;
        
        if (!myProcess.Start())
          throw new ApplicationException("Cannot start process");
      }
      catch
      {
        myProcess = null;
        throw;
      }
    }

    public void WaitForExit()
    {
      if (myProcess != null)
      {
        myProcess.WaitForExit();
        if (myProcess != null)
          myProcess = null;
      }
    }


    public void Kill()
    {
      if (myProcess != null)
      {
        try
        {
          ProcessKiller.KillProcessTree(myProcess.Id);
        }
        catch (Exception e)
        {
          Logger.LogExceptionSilently(e);
        }
        myProcess = null;
      }
    }

    private void myProcess_Exited(object sender, EventArgs e)
    {
      if (myProcess != null)
        myProcess = null;
    }

    public void Browse(string snapshot, bool temporary)
    {
      var process = new Process
                      {
                        StartInfo = new ProcessStartInfo
                                      {
                                        UseShellExecute = false, 
                                        FileName = Path.Combine(Location, EXECUTABLE), 
                                        Arguments = string.Format((temporary ? myOpen_Temporary : myOpen_Permanent), 
                                                                  CommandLine.QuoteIfNeed(snapshot)), 
                                        WorkingDirectory = Location
                                      }
                      };
      if (!process.Start())
        throw new ApplicationException("Cannot start process");
    }

    public static InstalledProfiler[] Gather()
    {
      var list = new List<InstalledProfiler>();
      using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"))
        Gather(key, list);

      // dotTrace 3.1 x64 installs into 64-bit registry hive which is only accessible from 32-bit applications by using special keys
      using (RegistryKey key = Registry64.OpenSubKey(Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"))
        Gather(key, list);

      return list.ToArray();
    }

    private static void Gather(RegistryKey key, ICollection<InstalledProfiler> list)
    {
      foreach (string productKeyName in key.GetSubKeyNames())
        using (RegistryKey productKey = key.OpenSubKey(productKeyName))
          try
          {
            if (productKey != null)
            {
              var displayName = (string) productKey.GetValue("DisplayName");
              if (displayName != null && displayName.IndexOf("JetBrains dotTrace") == 0)
                list.Add(new InstalledProfiler((string)productKey.GetValue("InstallLocation")));
            }
          }
          catch (Exception e)
          {
            Logger.LogExceptionSilently(e);
          }
    }
  }
}