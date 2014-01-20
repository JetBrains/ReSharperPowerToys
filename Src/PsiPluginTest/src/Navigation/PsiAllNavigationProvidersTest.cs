using JetBrains.ReSharper.IntentionsTests.Navigation;
using NUnit.Framework;

namespace PsiPluginTest.Navigation
{
  internal class PsiAllNavigationProvidersTest : AllNavigationProvidersTestBase
  {
    protected override string ExtraPath
    {
      get { return "..\\..\\navigation"; }
    }

    protected override WhatToDump DumpOptions { get { return WhatToDump.All; } }

    [Test]
    public void test001()
    {
      DoTestFiles("test001.psi");
    }

    [Test]
    public void test002()
    {
      DoTestFiles("test002.psi");
    }

    [Test]
    public void test003()
    {
      DoTestFiles("test003.psi");
    }

    [Test]
    public void test004()
    {
      DoTestFiles("test004.psi");
    }
  }
}