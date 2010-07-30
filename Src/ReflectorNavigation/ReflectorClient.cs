using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Metadata.Utils;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ExternalSources.Utils;
using JetBrains.ReSharper.PowerToys.ReflectorNavigation.ReflectorAddin;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ReflectorNavigation
{
  public static class ReflectorClient
  {
    private const string ADDIN_DLL = "ReflectorNavigation.ReflectorAddin.dll";
    private const string REFLECTOR_CFG = "Reflector.cfg";
    private const string RESOURCE_ADDIN_DLL = "JetBrains.ReSharper.PowerToys.ReflectorNavigation.ReflectorAddin.bin." + ADDIN_DLL;

    [NotNull]
    private static Stream GetAddinStream()
    {
      var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(RESOURCE_ADDIN_DLL);
      if (stream == null)
        throw new ApplicationException("Can't get addin from resources: " + RESOURCE_ADDIN_DLL);

      return stream;
    }

    private static string BufToHex(byte[] buf)
    {
      var s = new StringBuilder();
      foreach (byte b in buf)
        s.Append(b.ToString("x2").ToLower());
      return s.ToString();
    }


    private static string GetAddinChecksum()
    {
      using (var stream = GetAddinStream())
        return BufToHex(MD5.Create().ComputeHash(stream));
    }

    public static void CopyStream(Stream input, Stream output)
    {
      int read;
      var buffer = new byte[2048];
      do
      {
        read = input.Read(buffer, 0, buffer.Length);
        output.Write(buffer, 0, read);
      } while (read > 0);
    }

    public static void ExtractAddinAndPrepareReflectorCfg(out FileSystemPath reflectorCfg)
    {
      var checksum = GetAddinChecksum();
      var addinDir = new FileSystemPath(Path.Combine(
                                          Path.GetTempPath(), "ReSharper-ReflectorAddin-" + checksum));

      if (!addinDir.ExistsDirectory)
        Directory.CreateDirectory(addinDir.FullPath);

      var dll = addinDir.Combine(ADDIN_DLL);
      if (!dll.ExistsFile)
      {
        using (var output = new FileStream(dll.FullPath, FileMode.CreateNew))
        using (Stream input = GetAddinStream())
          CopyStream(input, output);
      }

      reflectorCfg = addinDir.Combine(REFLECTOR_CFG);
      if (!reflectorCfg.ExistsFile)
        WriteReflectorCfg(reflectorCfg, dll);
    }

    private static void WriteReflectorCfg(FileSystemPath cfg, FileSystemPath addinDll)
    {
      cfg.WriteAllText(
        "\r\n[AddInManager]\r\n\"" + addinDll.FullPath + "\"\r\n" +
        "\r\n[AssemblyManager]\r\n\"" + typeof(int).Assembly.Location + "\"\r\n\r\n");      
    }

    public static bool IsAvailable()
    {
      using (var client = new NamedPipeClient(ReflectorConstants.LOCAL_PIPE))
        return client.Connected;
    }

    public static bool LaunchReflector(ISolution solution)
    {
      var logger = ReflectorSpecificLogger.GetInstance(solution);

      var reflectorExe = ReflectorOptions.Instance.ReflectorExe.Value;
      if (string.IsNullOrEmpty(reflectorExe))
      {
        logger.LogFailure("Reflector binary path wasn't specified in options");
        return false;
      }

      if (!File.Exists(reflectorExe))
      {
        logger.LogFailure("Reflector binary " + reflectorExe + " wasn't found");
        return false;
      }

      FileSystemPath reflectorCfg;
      ExtractAddinAndPrepareReflectorCfg(out reflectorCfg);

      var startInfo = new ProcessStartInfo
                        {
                          Arguments = "\"/configuration:" + reflectorCfg.FullPath + "\"",
                          ErrorDialog = true,
                          FileName = reflectorExe,
                          WindowStyle = ProcessWindowStyle.Minimized,
                        };
      
      var process = Process.Start(startInfo);
      if (process == null)
        return false; // already created?

      return true;
    }

    [CanBeNull]
    public static string Decompile(IAssembly assembly, string typeName, bool showXmlDoc, string language)
    {
      Shell.Instance.PrimaryDispatcher.AssertAccess();

      ReflectorSpecificLogger logger = ReflectorSpecificLogger.GetInstance(assembly.GetSolution());

      IAssemblyFile assemblyFile = GetAssemblyFileByAssembly(assembly);
      if (assemblyFile == null)
      {
        logger.LogFailure("Assembly file is missing for assembly " + assembly.FullAssemblyName);
        return null;
      }

      using (var client = new NamedPipeClient(ReflectorConstants.LOCAL_PIPE))
      {
        if (!client.Connected)
        {
          logger.LogFailure("Can't communicate with Reflector through pipe {0}", client.PipeName);
          return null;
        }

        byte[] request = MakeRequest(assemblyFile, typeName, showXmlDoc, language);

        byte[] response = client.WriteMessageReadResponse(request);
        if (response == null)
        {
          logger.LogFailure("Error writing to the pipe {0}", client.PipeName);
          return null;
        }

        return Encoding.UTF8.GetString(response);
      }
    }

    [CanBeNull]
    private static IAssemblyFile GetAssemblyFileByAssembly(IAssembly assembly)
    {
      if (assembly.IsMissing || assembly.GetFiles().Length == 0)
        return null;

      return assembly.GetFiles()[0];
    }

    private static byte[] MakeRequest(IAssemblyFile assemblyFile, string typeName, bool showXmlDoc, string language)
    {
      IEnumerable<KeyValuePair<AssemblyNameInfo, FileSystemPath>> references = GetAssemblyReferences(assemblyFile.Assembly);

      IDictionary<string, string> request = new Dictionary<string, string>();
      request["AssemblyPath"] = assemblyFile.Location.FullPath;
      request["TypeName"] = typeName;
      request["Language"] = language;
      request["ShowXmlDoc"] = showXmlDoc.ToString();
      foreach (var reference in references)
        request["ref." + reference.Key.FullName] = reference.Value.FullPath;


      var serializer = new XmlSerializer(typeof (SerializableDictionary<string, string>));
      var serializableDictionary = new SerializableDictionary<string, string>(request);

      var ms = new MemoryStream();
      using (var sw = new StreamWriter(ms))
      {
        serializer.Serialize(sw, serializableDictionary);
        return ms.ToArray();
      }
    }

    private static IEnumerable<KeyValuePair<AssemblyNameInfo, FileSystemPath>> GetAssemblyReferences(IAssembly assembly)
    {
      ReflectorSpecificLogger logger = ReflectorSpecificLogger.GetInstance(assembly.GetSolution());

      using (WriteLockCookie.Create())
      {
        var references = new Dictionary<AssemblyNameInfo, FileSystemPath>();
        foreach (IAssemblyToAssemblyReference reference in assembly.GetReferences())
        {
          IAssembly referencedAssembly = reference.ResolveReferencedAssemblyAndAddToSolutionIfNecessary
            (false);
          if (referencedAssembly == null || referencedAssembly.IsMissing)
          {
            logger.LogInformation("Can't load referenced assembly {0} from {1}",
                                  reference.AssemblyName, assembly.FullAssemblyName);
            continue;
          }

          references[reference.AssemblyName] = referencedAssembly.GetFiles()[0].Location;
        }

        return references;
      }
    }
  }
}