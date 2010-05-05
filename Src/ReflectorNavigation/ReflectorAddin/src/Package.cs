using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Reflector;
using Reflector.CodeModel;

namespace JetBrains.ReSharper.PowerToys.ReflectorNavigation.ReflectorAddin
{
  public class Package : IPackage
  {
    private ServiceProviderWrapper myServices;

    #region IPackage Members

    public void Load(IServiceProvider serviceProvider)
    {
      myServices = new ServiceProviderWrapper(serviceProvider);

      new Server(DecompileInRightThread);
    }

    public void Unload()
    {
    }

    #endregion

    private string DecompileInRightThread(IDictionary<string, string> arguments)
    {
      var windowManager = myServices.GetService<IWindowManager>();
      Control control = windowManager.Content;

      if (control != null)
      {
        DecompileDelegate dd = Decompile;
        return (string) control.Invoke(dd, arguments) ?? "";
      }

      return "";
    }

    private static IEnumerable<string> GetReferencesPaths(IDictionary<string, string> arguments)
    {
      foreach (var argument in arguments)
        if (argument.Key.StartsWith("ref."))
          yield return argument.Value;
    }

    private void ExtractArguments(IDictionary<string, string> arguments, out string ext, out bool showXmlDoc)
    {
      if (!arguments.TryGetValue("Language", out ext))
        ext = "cs";

      string xmldoc;
      if (!arguments.TryGetValue("ShowXmlDoc", out xmldoc))
        xmldoc = "true";

      if (!bool.TryParse(xmldoc, out showXmlDoc))
        showXmlDoc = true;
    }

    private string Decompile(IDictionary<string, string> arguments)
    {
      // TODO more error handling

      var assemblyManager = myServices.GetService<IAssemblyManager>();

      foreach (string location in GetReferencesPaths(arguments))
        assemblyManager.LoadFile(location);

      IAssembly assembly = assemblyManager.LoadFile(arguments["AssemblyPath"]);
      ITypeDeclaration type = LocateType(assembly, arguments["TypeName"]);
      if (type == null)
        return "";

      string ext;
      bool xmlDoc;
      ExtractArguments(arguments, out ext, out xmlDoc);

      var cg = new CodeGenerator(myServices.GetService<ILanguageManager>(),
                                 myServices.GetService<ITranslatorManager>());
      return cg.Decompile(type, ext, xmlDoc);
    }

    private static ITypeDeclaration LocateType(IAssembly assembly, string fullName)
    {
      foreach (IModule module in assembly.Modules)
        foreach (ITypeDeclaration type in module.Types)
        {
          string name = type.Name;
          if (!string.IsNullOrEmpty(type.Namespace))
            name = type.Namespace + "." + name;

          if (type.GenericArguments.Count > 0)
            name += "`" + type.GenericArguments.Count;

          if (name == fullName)
            return type;
        }

      return null;
    }

    #region Nested type: DecompileDelegate

    private delegate string DecompileDelegate(IDictionary<string, string> arguments);

    #endregion
  }
}