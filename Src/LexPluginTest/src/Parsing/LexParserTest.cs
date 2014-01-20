using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace LexPluginTest.Parsing
{
  [TestFixture, Category("Lex")]
  public class LexParserTest : ParserTestBase
  {
    protected override string RelativeTestDataPath
    {
      get { return @"parsing"; }
    }

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

    [Test]
    public void test003()
    {
      DoTestFiles("test003.lex");
    }

    [Test]
    public void test004()
    {
      DoTestFiles("test004.lex");
    }

    [Test]
    public void test005()
    {
      DoTestFiles("test005.lex");
    }
  }
}
