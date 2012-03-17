using System;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.TestFramework;
using JetBrains.ReSharper.Psi.Find.Test;

using NUnit.Framework;

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

    [Test] public void test1() {DoTestOneFile("test1.psi");}
  }
}
