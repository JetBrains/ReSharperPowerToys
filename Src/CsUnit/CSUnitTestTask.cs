/*
 * Copyright 2007-2011 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Xml;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace JetBrains.ReSharper.PowerToys.CsUnit
{
  [Serializable]
  public class CSUnitTestTask : RemoteTask, IEquatable<CSUnitTestTask>
  {
    private readonly string myTestType;
    private readonly string myTestMethod;
    private readonly bool myExplicitly;

    public CSUnitTestTask(string testType, string testMethod, bool explicitly)
      : base(CSUnitTestProvider.CSUnit_ID)
    {
      if (testMethod == null)
        throw new ArgumentNullException("testMethod");
      if (testType == null)
        throw new ArgumentNullException("testType");

      myTestType = testType;
      myTestMethod = testMethod;
      myExplicitly = explicitly;
    }

    public CSUnitTestTask(XmlElement element)
      : base(element)
    {
      myTestMethod = GetXmlAttribute(element, "TestMethod");
      myTestType = GetXmlAttribute(element, "TestType");
      myExplicitly = GetXmlAttribute(element, "Explicitly") == "true";
    }

    public override void SaveXml(XmlElement element)
    {
      base.SaveXml(element);
      SetXmlAttribute(element, "TestMethod", TestMethod);
      SetXmlAttribute(element, "TestType", TestType);
      SetXmlAttribute(element, "Explicitly", Explicitly ? "true" : "false");
    }

    public bool Explicitly
    {
      get { return myExplicitly; }
    }

    public string TestMethod
    {
      get { return myTestMethod; }
    }

    private string TestType
    {
      get { return myTestType; }
    }


    public bool Equals(CSUnitTestTask CSUnitTestTask)
    {
      if (CSUnitTestTask == null) return false;
      return Equals(myTestType, CSUnitTestTask.myTestType) && Equals(myTestMethod, CSUnitTestTask.myTestMethod)
             && myExplicitly == CSUnitTestTask.myExplicitly;
    }

    public override bool Equals(RemoteTask other)
    {
      if (this == other) return true;
      return Equals(other as CSUnitTestTask);
    }

    public override bool Equals(object obj)
    {
      if (this == obj) return true;
      return Equals(obj as CSUnitTestTask);
    }

    public override int GetHashCode()
    {
      int result = base.GetHashCode();
      result = 29*result + myTestType.GetHashCode();
      result = 29*result + myTestMethod.GetHashCode();
      result = 29*result + myExplicitly.GetHashCode();
      return result;
    }
  }
}