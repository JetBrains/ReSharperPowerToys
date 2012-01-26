using System;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Impl.Shared;
using JetBrains.ReSharper.PsiTests;
using JetBrains.ReSharper.PsiTests.parsing;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace PsiPluginTest.Parsing
{
  //[SetUpFixture]
  [TestFixture, Category("PSI")]
  public class PsiParserTest : ParserTestBase
  {
    protected override string RelativeTestDataPath
    {
      get { return @"parsing"; }
    }

    [Test]
    public void test001()
    {
      DoTestFiles("test001.psi");
    }
  }
}
