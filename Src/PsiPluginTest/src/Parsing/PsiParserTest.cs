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