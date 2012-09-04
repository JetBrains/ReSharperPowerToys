using System.IO;
using System.Reflection;
using JetBrains.Application;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model
{
  [ShellComponent]
  class PredefinedZenCodingSettings : IHaveDefaultSettingsStream
  {
    public Stream GetDefaultSettingsStream(Lifetime lifetime)
    {
      var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("JetBrains.ReSharper.PowerToys.ZenCoding.resources.PredefinedZenCodingSettings.xml");
      Assertion.AssertNotNull(stream, "stream == null");
      lifetime.AddDispose(stream);
      return stream;
    }

    public string Name
    {
      get { return "Default ZenCoding Settings"; }
    }
  }
}
