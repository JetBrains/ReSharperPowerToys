using JetBrains.ReSharper.IntentionsTests.Navigation;
using NUnit.Framework;

namespace LexPluginTest.Navigation
{
  internal class LexAllNavigationProvidersTest : AllNavigationProvidersTestBase
  {
    protected override string ExtraPath
    {
      get { return "..\\..\\navigation"; }
    }

    protected override WhatToDump DumpOptions { get { return WhatToDump.All; } }

    [Test]
    public void test001()
    {
      DoTestFiles("test001.lex");
    }

    [Test]
    public void test002()
    {
      DoTestFiles("test002.lex");
    }
  }
}