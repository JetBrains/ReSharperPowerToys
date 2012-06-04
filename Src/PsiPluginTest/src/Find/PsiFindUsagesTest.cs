using System;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;
using FindUsagesTestBase = JetBrains.ReSharper.Psi.Find.Test.FindUsagesTestBase;

namespace PsiPluginTest.Find
{
  [TestFixture]
  [Category("Find")]
  [TestNetFramework4]
  class PsiFindUsagesTest : FindUsagesTestBase
  {
    protected override String RelativeTestDataPath
    {
      get { return @"find"; }
    }

    protected override SearchPattern SearchPattern
    {
      get { return SearchPattern.FIND_USAGES | SearchPattern.FIND_IMPLEMENTORS_USAGES | SearchPattern.FIND_RELATED_ELEMENTS; }
    }

    [Test] public void test001() {DoTestOneFile("test001.psi");}
  }
}