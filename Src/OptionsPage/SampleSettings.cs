using System;
using System.Xml;
using JetBrains.Application;
using JetBrains.Application.Components;
using JetBrains.Application.Configuration;

namespace JetBrains.ReSharper.PowerToys.OptionsPage
{
  [ShellComponent(ProgramConfigurations.VS_ADDIN)]
  public class SampleSettings : IXmlExternalizable
  {
    public static SampleSettings Instance
    {
      get { return Shell.Instance.GetComponent<SampleSettings>(); }
    }

    [XmlExternalizable(true)]
    public bool SampleOption { get; set; }

    #region IXmlExternalizable implementation

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