using System.Xml;
using JetBrains.Application;
using JetBrains.ComponentModel;
using JetBrains.DataFlow;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ReflectorNavigation
{
  [ShellComponentImplementation]
  [ShellComponentInterface]
  public class ReflectorOptions : IXmlExternalizableShellComponent
  {
    private readonly IProperty<string> myLanguageExtension =
      new Property<string>("LanguageExtension", "cs", false);

    private readonly IProperty<bool> myPostReformat = new Property<bool>("PostReformat", false);
    private readonly IProperty<string> myReflectorExe = new Property<string>("ReflectorExe", "", false);
    private readonly IProperty<bool> myShowXmlDoc = new Property<bool>("ShowXmlDoc", true);

    public IProperty<string> LanguageExtension
    {
      get { return myLanguageExtension; }
    }

    public IProperty<bool> PostReformat
    {
      get { return myPostReformat; }
    }

    public IProperty<string> ReflectorExe
    {
      get { return myReflectorExe; }
    }

    public IProperty<bool> ShowXmlDoc
    {
      get { return myShowXmlDoc; }
    }

    public static ReflectorOptions Instance
    {
      get { return Shell.Instance.GetComponent<ReflectorOptions>(); }
    }

    #region IXmlExternalizableShellComponent Members

    public void Dispose()
    {
    }

    public void Init()
    {
    }

    public void ReadFromXml(XmlElement element)
    {
      if (element == null)
        return;

      myPostReformat.Value = element.GetAttribute("PostReformat", false);
      myReflectorExe.Value = element.GetAttribute("ReflectorExe", "");
      myShowXmlDoc.Value = element.GetAttribute("ShowXmlDoc", true);
      myLanguageExtension.Value = XmlUtil.GetAttribute(element, "LanguageExtension", "cs");
    }

    public void WriteToXml(XmlElement element)
    {
      element.SetAttribute("PostReformat", myPostReformat.Value.ToString());
      element.SetAttribute("ShowXmlDoc", myShowXmlDoc.Value.ToString());
      element.SetAttribute("ReflectorExe", myReflectorExe.Value);
      element.SetAttribute("LanguageExtension", myLanguageExtension.Value);
    }

    public XmlExternalizationScope Scope
    {
      get { return XmlExternalizationScope.UserSettings; }
    }

    public string TagName
    {
      get { return "ReflectorOptions"; }
    }

    #endregion
  }
}