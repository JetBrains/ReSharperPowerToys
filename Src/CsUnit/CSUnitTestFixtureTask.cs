using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace JetBrains.ReSharper.PowerToys.CsUnit
{
  [Serializable]
  public class CSUnitTestFixtureTask : RemoteTask, IEquatable<CSUnitTestFixtureTask>
  {
    private readonly string myAssemblyLocation;

    private readonly string myTypeName;
    private readonly bool myExplicitly;

    [NonSerialized]
    private object myInstance;  
    
    public CSUnitTestFixtureTask(string assemblyLocation, string typeName, bool explicitly)
      : base(CSUnitTestProvider.CSUnit_ID)
    {
      if (typeName == null)
        throw new ArgumentNullException("typeName");

      myAssemblyLocation = assemblyLocation;
      myTypeName = typeName;
      myExplicitly = explicitly;
    }

    [SuppressMessage("","")]
    public CSUnitTestFixtureTask(XmlElement element)
      : base(element)
    {
      myTypeName = GetXmlAttribute(element, "TypeName");
      myAssemblyLocation = GetXmlAttribute(element, "AssemblyLocation");
      myExplicitly = GetXmlAttribute(element, "Explicitly") == "true";
    }

    public override void SaveXml(XmlElement element)
    {
      base.SaveXml(element);
      SetXmlAttribute(element, "TypeName", TypeName);
      SetXmlAttribute(element, "AssemblyLocation", AssemblyLocation);
      SetXmlAttribute(element, "Explicitly", Explicitly ? "true" : "false");
    }

    public string AssemblyLocation
    {
      get { return myAssemblyLocation; }
    }

    public string TypeName
    {
      get { return myTypeName; }
    }

    public bool Explicitly
    {
      get { return myExplicitly; }
    }

    public object Instance
    {
      get { return myInstance; }
      set { myInstance = value; }
    }

    public bool Equals(CSUnitTestFixtureTask CSUnitTestFixtureTask)
    {
      if (CSUnitTestFixtureTask == null) return false;
      return Equals(myAssemblyLocation, CSUnitTestFixtureTask.myAssemblyLocation) 
             && Equals(myTypeName, CSUnitTestFixtureTask.myTypeName)
             && myExplicitly == CSUnitTestFixtureTask.myExplicitly;
    }

    public override bool Equals(RemoteTask other)
    {
      if (this == other) return true;
      return Equals(other as CSUnitTestFixtureTask);
    }

    public override bool Equals(object obj)
    {
      if (this == obj) return true;
      return Equals(obj as CSUnitTestFixtureTask);
    }

    public override int GetHashCode()
    {
      int result = base.GetHashCode();
      result = 29*result + myAssemblyLocation.GetHashCode();
      result = 29*result + myTypeName.GetHashCode();
      result = 29*result + myExplicitly.GetHashCode();
      return result;
    }
  }
}