using System;
using System.Xml;
using JetBrains.Application;
using JetBrains.ComponentModel;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.OptionsPage
{
  [ShellComponentInterface(ProgramConfigurations.VS_ADDIN)]
  [ShellComponentImplementation]
  public class SampleSettings : IXmlExternalizableShellComponent
  {
    private bool mySampleOption;

    public static SampleSettings Instance
    {
      get { return Shell.Instance.GetComponent<SampleSettings>(); }
    }

    [XmlExternalizable(true)]
    public bool SampleOption
    {
      get { return mySampleOption; }
      set { mySampleOption = value; }
    }

    #region IShellComponent implementation

    void IComponent.Init()
    {
    }

    void IDisposable.Dispose()
    {
    }

    #endregion

    #region IXmlExternalizableShellComponent implementation

    string IXmlExternalizableComponent.TagName
    {
      get
      {
        // tag name, should not conflict with any other plugins and internal ReSharper components
        return "PluginSamples.AddOptionsPage.SampleSettings";
      }
    }

    XmlExternalizationScope IXmlExternalizableComponent.Scope
    {
      get
      {
        return XmlExternalizationScope.UserSettings;
      }
    }

    void IXmlExternalizable.ReadFromXml(XmlElement element)
    {
      // read values via reflection
      XmlExternalizationUtil.ReadFromXml(element, this);
    }

    void IXmlExternalizable.WriteToXml(XmlElement element)
    {
      // write values via reflection
      XmlExternalizationUtil.WriteToXml(element, this);
    }

    #endregion
  }
}