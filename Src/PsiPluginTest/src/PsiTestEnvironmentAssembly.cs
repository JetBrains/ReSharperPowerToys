#pragma warning disable CheckNamespace

using System.Collections.Generic;
using System.Reflection;
using JetBrains.Application;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing;
using NUnit.Framework;
using JetBrains.Threading;

[SetUpFixture]
public class PsiTestEnvironmentAssembly : ReSharperTestEnvironmentAssembly
{
  /// <summary>
  /// Gets the assemblies to load into test environment.
  /// Should include all assemblies which contain components.
  /// </summary>
  private static IEnumerable<Assembly> GetAssembliesToLoad()
  {
    // Plugin code
    yield return typeof(PsiLexer).Assembly;

    // Test assembly
    yield return Assembly.GetExecutingAssembly();
  }

  public override void SetUp()
  {
    base.SetUp();
    ReentrancyGuard.Current.Execute(
      "LoadAssemblies",
      () => Shell.Instance.GetComponent<AssemblyManager>().LoadAssemblies(
        GetType().Name, GetAssembliesToLoad()));
  }

  public override void TearDown()
  {
    ReentrancyGuard.Current.Execute(
      "UnloadAssemblies",
      () => Shell.Instance.GetComponent<AssemblyManager>().UnloadAssemblies(
        GetType().Name, GetAssembliesToLoad()));
    base.TearDown();
  }
}