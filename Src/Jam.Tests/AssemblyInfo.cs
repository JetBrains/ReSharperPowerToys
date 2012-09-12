using JetBrains.Application;
using JetBrains.Threading;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;

namespace JetBrains.ReSharper.Psi.Jam.Tests
{
  /// <summary>
  /// Test environment. Must be in the global namespace.
  /// </summary>
  [SetUpFixture]
  public class TestEnvironmentAssembly : ReSharperTestEnvironmentAssembly
  {
    /// <summary>
    /// Gets the assemblies to load into test environment.
    /// Should include all assemblies which contain components.
    /// </summary>
    private static IEnumerable<Assembly> GetAssembliesToLoad()
    {
      // Test assembly
      yield return Assembly.GetExecutingAssembly();
      yield return typeof(JamLanguage).Assembly;
    }

    public override void SetUp()
    {
      base.SetUp();
      ReentrancyGuard.Current.Execute(
        "LoadAssemblies",
        () => Application.Shell.Instance.GetComponent<AssemblyManager>().LoadAssemblies(
          GetType().Name, GetAssembliesToLoad()));
    }

    public override void TearDown()
    {
      ReentrancyGuard.Current.Execute(
        "UnloadAssemblies",
        () => Application.Shell.Instance.GetComponent<AssemblyManager>().UnloadAssemblies(
          GetType().Name, GetAssembliesToLoad()));
      base.TearDown();
    }
  }
}
