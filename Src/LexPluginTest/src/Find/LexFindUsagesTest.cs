using System;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.PsiTests.find;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace LexPluginTest.Find
{
  [TestFixture]
  [Category("Find")]
  [TestNetFramework4]
  class LexFindUsagesTest : FindUsagesTestBase
  {
    protected override String RelativeTestDataPath
    {
      get { return @"find"; }
    }

    protected override SearchPattern SearchPattern
    {
      get { return SearchPattern.FIND_USAGES | SearchPattern.FIND_IMPLEMENTORS_USAGES | SearchPattern.FIND_RELATED_ELEMENTS; }
    }

    [Test]
    public void test001() { DoTestOneFile("test001.lex"); }
  }
}