using JetBrains.ReSharper.PsiTests.parsing;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace JetBrains.ReSharper.Psi.Jam.Tests
{
  [TestFixture]
  [Category("Parser")]
  [TestFileExtension(".jam")]
  public class JamParserTest : ParserTestBase
  {
    protected override string RelativeTestDataPath
    {
      get { return @"psi\parser"; }
    }

    [Test] public void Test01() { DoNamedTest(); }
    [Test] public void Test02() { DoNamedTest(); }
    [Test] public void Test03() { DoNamedTest(); }
  }
}