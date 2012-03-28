using System;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;
using PlatformID = JetBrains.ProjectModel.PlatformID;
using JetBrains.ReSharper.Psi.Search;

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
