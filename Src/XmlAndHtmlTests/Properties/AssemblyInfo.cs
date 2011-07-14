using System.Collections.Generic;
using System.Reflection;
using JetBrains.Application;
using JetBrains.Threading;
using NUnit.Framework;
using XmlAndHtml;

/// <summary>
/// Must be in the global namespace.
/// </summary>
[SetUpFixture]
public class XmlAndHtmlTestEnvironmentAssembly : ReSharperTestEnvironmentAssembly
{
  /// <summary>
  /// Gets the assemblies to load into test environment.
  /// Should include all assemblies which contain components.
  /// </summary>
  private static IEnumerable<Assembly> GetAssembliesToLoad()
  {
    // Test assembly
    yield return Assembly.GetExecutingAssembly();

    // Plugin code
    yield return typeof(SpecifyIdHtmlContextAction).Assembly;
  }
  
  [SetUp]
  public void SetUp()
  {
    base.SetUp();
    ReentrancyGuard.Current.Execute(
      "LoadAssemblies",
      () => Shell.Instance.GetComponent<AssemblyManager>().LoadAssemblies(
        GetType().Name, GetAssembliesToLoad()));
  }

  [TearDown]
  public void TearDown()
  {
    ReentrancyGuard.Current.Execute(
      "UnloadAssemblies",
      () => Shell.Instance.GetComponent<AssemblyManager>().LoadAssemblies(
        GetType().Name, GetAssembliesToLoad()));
    base.TearDown();
  }
}