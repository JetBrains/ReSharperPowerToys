using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Feature.Services.Search;
using JetBrains.ReSharper.FeaturesTests.Finding.GotoFileMember;
using NUnit.Framework;

namespace PsiPluginTest.GotoMember
{
  [TestFixture]
  [Category("Psi")]
  //[Ignore("(H) test data outdated")]
  internal class PsiGotoFileMemberTest : GotoFileMemberTestBase
  {
    protected override string SpecificFolder
    {
      get { return "Psi"; }
    }

    protected override string RelativeTestDataPath
    {
      get { return Path.Combine(@"gotoFileMember\", SpecificFolder); }
    }

    [TestCase("test001", LibrariesFlag.SolutionOnly, "", "test001.psi")]
    public void Test(string resultFileName, LibrariesFlag advancedSearch, string filter, string fileToTest)
    {
      DoTest(resultFileName, advancedSearch, filter, fileToTest);
    }
  }
}