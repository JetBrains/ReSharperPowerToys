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
// ReSharper disable CheckNamespace
public class XmlAndHtmlTestEnvironmentAssembly : ReSharperTestEnvironmentAssembly
// ReSharper restore CheckNamespace
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
      () => Shell.Instance.GetComponent<AssemblyManager>().LoadAssemblies(
        GetType().Name, GetAssembliesToLoad()));
    base.TearDown();
  }
}