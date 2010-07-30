using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace JetBrains.ReSharper.PowerToys.CsUnit
{
  internal class CSUnitTaskRunner : RemoteTaskRunner
  {
    private static MethodInfo myFixtureSetUp;
    private static MethodInfo myFixtureTearDown;
    private static MethodInfo myTestSetUp;
    private static MethodInfo myTestTearDown;

    public CSUnitTaskRunner(IRemoteTaskServer server)
      : base(server)
    {
    }

    public override TaskResult Start(TaskExecutionNode node)
    {
      RemoteTask task = node.RemoteTask;
      if (task is CSUnitTestFixtureTask)
        return Start(Server, node, (CSUnitTestFixtureTask) task);
      if (task is CSUnitTestTask)
        return Start(Server, node, (CSUnitTestTask) task);
      return TaskResult.Error;
    }

    public override TaskResult Execute(TaskExecutionNode node)
    {
      RemoteTask task = node.RemoteTask;
      if (task is CSUnitTestFixtureTask)
        return TaskResult.Success;
      if (task is CSUnitTestTask)
        return Execute(Server, node, (CSUnitTestTask) task);
      return TaskResult.Error;
    }

    public override TaskResult Finish(TaskExecutionNode node)
    {
      RemoteTask task = node.RemoteTask;
      if (task is CSUnitTestFixtureTask)
      {
        return Finish(Server, (CSUnitTestFixtureTask)task);
      }
      if (task is CSUnitTestTask)
      {
        return Finish(Server, node, (CSUnitTestTask)task);
      }
      return TaskResult.Error;
    }

    public override void ConfigureAppDomain(TaskAppDomainConfiguration configuration)
    {
      var settings = ConfigurationManager.GetSection("csUnit/TestRunner") as NameValueCollection;
      if (settings != null)
      {
        string apartment = settings["ApartmentState"];
        if (apartment != null)
          try
          {
            configuration.ApartmentState = (ApartmentState)Enum.Parse(typeof(ApartmentState), apartment, true);
          }
          catch (ArgumentException ex)
          {
            string msg = string.Format("Invalid ApartmentState setting '{1}' in configuration file '{0}'",
                                       AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, apartment);
            throw new ArgumentException(msg, ex);
          }

        string threadPriority = settings["ThreadPriority"];
        if (threadPriority != null)
          try
          {
            configuration.Priority = (ThreadPriority)Enum.Parse(typeof(ThreadPriority), threadPriority, true);
          }
          catch (ArgumentException ex)
          {
            string msg = string.Format("Invalid ThreadPriority setting '{1}' in '{0}'",
                                       AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, threadPriority);
            throw new ArgumentException(msg, ex);
          }
      }
    }

    #region Test

    private TaskResult Start(IRemoteTaskServer server, TaskExecutionNode node, CSUnitTestTask test)
    {
      var fixture = (CSUnitTestFixtureTask) node.Parent.RemoteTask;

      object instance = fixture.Instance;
      Type type = instance.GetType();

      MethodInfo testMI = type.GetMethod(test.TestMethod, new Type[0]);
      if (testMI == null)
      {
        server.TaskError(test, string.Format("Cannot find test  method '{0}'", test.TestMethod));
        return TaskResult.Error;
      }

      string ignoreReason;
      if (IsIgnored(testMI, out ignoreReason) && !test.Explicitly)
      {
        server.TaskFinished(test, ignoreReason, TaskResult.Skipped);
        return TaskResult.Skipped;
      }

      if (myTestSetUp != null)
      {
        server.TaskProgress(test, "Setting up...");
        try
        {
          TaskExecutor.Invoke(fixture.Instance, myTestSetUp);
        }
        catch (TargetInvocationException e)
        {
          Exception exception = e.InnerException ?? e;

          string message;
          Server.TaskException(test, TaskExecutor.ConvertExceptions(exception, out message));
          Server.TaskFinished(test, message, TaskResult.Exception);
          return TaskResult.Exception;
        }
      }

      return TaskResult.Success;
    }


    private static TaskResult Execute(IRemoteTaskServer server, TaskExecutionNode node, CSUnitTestTask test)
    {
      var fixture = (CSUnitTestFixtureTask) node.Parent.RemoteTask;
      object instance = fixture.Instance;
      Type type = instance.GetType();

      MethodInfo testMI = type.GetMethod(test.TestMethod, new Type[0]);
      if (testMI == null)
      {
        server.TaskError(test, string.Format("Cannot find test  method '{0}'", test.TestMethod));
        return TaskResult.Error;
      }
      server.TaskProgress(test, "");

      string expectedExceptionType;
      GetExpectedException(testMI, out expectedExceptionType);

      Exception exception = null;
      try
      {
        TaskExecutor.Invoke(instance, testMI);
      }
      catch (TargetInvocationException e)
      {
        exception = e.InnerException ?? e;
      }
      if (exception != null && exception.GetType().FullName == "csUnit.IgnoreException")
      {
        server.TaskFinished(test, exception.Message, TaskResult.Skipped);
        return TaskResult.Skipped;
      }
      if (expectedExceptionType != null && exception == null)
      {
        // failed, exception expected but not thrown
        server.TaskError(test, string.Format("Expected exception '{0}' was not thrown", expectedExceptionType));
        return TaskResult.Error;
      }
      if (expectedExceptionType != null && expectedExceptionType == exception.GetType().FullName)
      {
        return TaskResult.Success;
      }
      if (exception != null)
        throw new TargetInvocationException(exception);

      return TaskResult.Success;
    }

    private TaskResult Finish(IRemoteTaskServer server, TaskExecutionNode node, CSUnitTestTask test)
    {
      var fixture = (CSUnitTestFixtureTask) node.Parent.RemoteTask;

      if (myTestTearDown != null)
      {
        server.TaskProgress(test, "Tearing down...");
        try
        {
          TaskExecutor.Invoke(fixture.Instance, myTestTearDown);
        }
        catch (TargetInvocationException e)
        {
          Exception exception = e.InnerException ?? e;

          string message;
          Server.TaskException(test, TaskExecutor.ConvertExceptions(exception, out message));
          Server.TaskFinished(test, message, TaskResult.Exception);
          return TaskResult.Exception;
        }
      }
      server.TaskProgress(test, "");
      return TaskResult.Success;
    }

    #endregion

    #region Fixture

    private TaskResult Start(IRemoteTaskServer server, TaskExecutionNode node, CSUnitTestFixtureTask fixture)
    {
      server.TaskProgress(fixture, "Instantiating...");
      Type type = GetFixtureType(fixture, server);
      if (type == null)
        return TaskResult.Error;

      string ignoreReason;
      if (IsIgnored(type, out ignoreReason))
      {
        // fixture is ignored
        if (!fixture.Explicitly)
        {
          // check that we don't have any explicitly run test
          bool hasExplicitTest = false;
          foreach (TaskExecutionNode testNode in node.Children)
          {
            if (((CSUnitTestTask) testNode.RemoteTask).Explicitly)
            {
              hasExplicitTest = true;
              break;
            }
          }
          if (!hasExplicitTest)
          {
            server.TaskProgress(fixture, ""); 
            server.TaskFinished(fixture, ignoreReason, TaskResult.Skipped);
            return TaskResult.Skipped;
          }
        }
      }

      ConstructorInfo ci = type.GetConstructor(Type.EmptyTypes);
      if (ci == null)
      {
        server.TaskError(fixture, string.Format("Cannot find parameterless constructor on type '{0}'", type));
        return TaskResult.Error;
      }

      if (!BuildTypeInfo(server, fixture, type))
        return TaskResult.Error;

      object instance = ci.Invoke(new object[0]);
      if (instance == null)
      {
        server.TaskError(fixture, string.Format("Cannot create instance of type '{0}'", type));
        return TaskResult.Error;
      }

      fixture.Instance = instance;
      if (myFixtureSetUp != null)
      {
        server.TaskProgress(fixture, "Setting up...");
        try
        {
          TaskExecutor.Invoke(fixture.Instance, myFixtureSetUp);
        }
        catch (TargetInvocationException e)
        {
          Exception exception = e.InnerException ?? e;

          string message;
          Server.TaskException(fixture, TaskExecutor.ConvertExceptions(exception, out message));
          Server.TaskFinished(fixture, message, TaskResult.Exception);
          return TaskResult.Exception;
        }
      }

      server.TaskProgress(fixture, "");
      return TaskResult.Success;
    }

    private TaskResult Finish(IRemoteTaskServer server, CSUnitTestFixtureTask fixture)
    {
      if (myFixtureTearDown != null)
      {
        server.TaskProgress(fixture, "Tearing down...");
        try
        {
          TaskExecutor.Invoke(fixture.Instance, myFixtureTearDown);
        }
        catch (TargetInvocationException e)
        {
          Exception exception = e.InnerException ?? e;

          string message;
          Server.TaskException(fixture, TaskExecutor.ConvertExceptions(exception, out message));
          Server.TaskFinished(fixture, message, TaskResult.Exception);
          return TaskResult.Exception;
        }
      }

      server.TaskProgress(fixture, "");
      myTestTearDown = null;
      myTestSetUp = null;
      myFixtureSetUp = null;
      myFixtureTearDown = null;
      fixture.Instance = null;
      return TaskResult.Success;
    }

    #endregion

    private static Type GetFixtureType(CSUnitTestFixtureTask fixture, IRemoteTaskServer server)
    {
      string assemblyLocation = fixture.AssemblyLocation;
      if (!File.Exists(assemblyLocation))
      {
        server.TaskError(fixture, string.Format("Cannot load assembly from {0}: file not exists", assemblyLocation));
        return null;
      }
      AssemblyName assemblyName = AssemblyName.GetAssemblyName(assemblyLocation);
      if (assemblyName == null)
      {
        server.TaskError(fixture, string.Format("Cannot load assembly from {0}: not an assembly", assemblyLocation));
        return null;
      }
      Assembly assembly = Assembly.Load(assemblyName);
      if (assembly == null)
      {
        server.TaskError(fixture, string.Format("Cannot load assembly from {0}", assemblyLocation));
        return null;
      }
      return assembly.GetType(fixture.TypeName);
    }

    private static bool BuildTypeInfo(IRemoteTaskServer server, RemoteTask fixture, Type type)
    {
      myTestTearDown = null;
      myTestSetUp = null;
      myFixtureSetUp = null;
      myFixtureTearDown = null;

      IList<MethodInfo> fixtureSetUp = new List<MethodInfo>();
      IList<MethodInfo> fixtureTearDown = new List<MethodInfo>();
      IList<MethodInfo> testSetUp = new List<MethodInfo>();
      IList<MethodInfo> testTearDown = new List<MethodInfo>();

      MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
      foreach (MethodInfo method in methods)
      {
        if (IsFixtureSetup(method)) fixtureSetUp.Add(method);
        else if (IsFixtureTeardown(method)) fixtureTearDown.Add(method);
        else if (IsTestSetup(method)) testSetUp.Add(method);
        else if (IsTestTeardown(method)) testTearDown.Add(method);
      }

      fixtureSetUp = FilterHiddenMethods(fixtureSetUp);
      if (fixtureSetUp.Count == 1)
        myFixtureSetUp = fixtureSetUp[0];
      else if (fixtureSetUp.Count > 1)
      {
        server.TaskError(fixture, type.Name + " has multiple fixture setup methods");
        return false;
      }

      fixtureTearDown = FilterHiddenMethods(fixtureTearDown);
      if (fixtureTearDown.Count == 1)
        myFixtureTearDown = fixtureTearDown[0];
      else if (fixtureTearDown.Count > 1)
      {
        server.TaskError(fixture, type.Name + " has multiple fixture teardown methods");
        return false;
      }

      testSetUp = FilterHiddenMethods(testSetUp);
      if (testSetUp.Count == 1)
        myTestSetUp = testSetUp[0];
      else if (testSetUp.Count > 1)
      {
        server.TaskError(fixture, type.Name + " has multiple test setup methods");
        return false;
      }

      testTearDown = FilterHiddenMethods(testTearDown);
      if (testTearDown.Count == 1)
        myTestTearDown = testTearDown[0];
      else if (testTearDown.Count > 1)
      {
        server.TaskError(fixture, type.Name + " has multiple test teardown methods");
        return false;
      }
      return true;
    }

    private static IList<MethodInfo> FilterHiddenMethods(IList<MethodInfo> methods)
    {
      if (methods.Count <= 1)
        return methods;

      var newMethods = new List<MethodInfo>();

      foreach (MethodInfo info in methods)
      {
        Type declaringType = info.DeclaringType;

        bool isvisible = true;
        foreach (MethodInfo info2 in methods)
        {
          Type declaringType2 = info2.DeclaringType;
          if (declaringType != declaringType2 && !declaringType.IsSubclassOf(declaringType2))
          {
            isvisible = false;
            break;
          }
        }

        if (isvisible)
          newMethods.Add(info);
      }

      return newMethods;
    }

    private static bool IsFixtureSetup(MethodInfo info)
    {
      return CheckSetUpTearDownSignature(info, "csUnit.TestFixtureSetUpAttribute");
    }

    private static bool IsFixtureTeardown(MethodInfo info)
    {
      return CheckSetUpTearDownSignature(info, "csUnit.TestFixtureTearDownAttribute");
    }

    private static bool IsTestSetup(MethodInfo info)
    {
      return CheckSetUpTearDownSignature(info, "csUnit.SetUpAttribute");
    }

    private static bool IsTestTeardown(MethodInfo info)
    {
      return CheckSetUpTearDownSignature(info, "csUnit.TearDownAttribute");
    }

    private static bool IsSubtypeOf(Type type, string name)
    {
      while (type != null)
      {
        if (type.FullName == name)
          return true;
        type = type.BaseType;
      }
      return false;
    }

    private static bool IsSubtypeOf(object obj, string name)
    {
      return IsSubtypeOf(obj.GetType(), name);
    }

    private static object[] GetCustomAttributesOf(ICustomAttributeProvider mi, bool inherit, string typeName)
    {
      return SelectAttributes(mi.GetCustomAttributes(inherit), typeName);
    }

    private static object[] SelectAttributes(object[] attrs, string typeName)
    {
      var found = new List<object>();
      foreach (object attr in attrs)
      {
        if (IsSubtypeOf(attr, typeName))
          found.Add(attr);
      }

      return found.ToArray();
    }

    private static bool CheckSetUpTearDownSignature(MethodInfo method, string attrName)
    {
      object[] attributes = GetCustomAttributesOf(method, true, attrName);
      if (attributes.Length == 0)
        return false;

      if (!method.IsPublic && !method.IsFamily || method.ReturnType != typeof (void) || method.GetParameters().Length > 0)
        return false; //throw new Exception("Invalid SetUp or TearDown method signature");

      return true;
    }

    private static object GetPropertyValue(object obj, string propName)
    {
      MemberInfo[] members = obj.GetType().GetMember(propName);
      if (members.Length != 1)
        return null;

      var propertyInfo = members[0] as PropertyInfo;
      if (propertyInfo == null)
        return null;

      return propertyInfo.GetValue(obj, null);
    }

    private static void GetExpectedException(ICustomAttributeProvider info, out string expectedExceptionType)
    {
      expectedExceptionType = null;

      object[] attributes = GetCustomAttributesOf(info, false, "csUnit.ExpectedExceptionAttribute");
      if (attributes.Length == 1)
      {
        object exceptionType = GetPropertyValue(attributes[0], "ExceptionType");
        if (exceptionType != null)
          expectedExceptionType = ((Type) exceptionType).FullName;
      }
    }

    private static bool IsIgnored(ICustomAttributeProvider info, out string reason)
    {
      object[] attributes = GetCustomAttributesOf(info, false, "csUnit.IgnoreAttribute");
      if (attributes.Length > 0)
      {
        reason = (string) GetPropertyValue(attributes[0], "Reason") ?? "";
        return true;
      }

      reason = null;
      return false;
    }
  }
}