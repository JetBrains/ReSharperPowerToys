using JetBrains.ReSharper.PsiTests.Lexing;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace JetBrains.ReSharper.Psi.Jam.Tests
{
  [TestFixture]
  [Category("Lexer")]
  [TestFileExtension(".jam")]
  public class JamLexerTest : LexerTestBase
  {
    protected override string RelativeTestDataPath
    {
      get { return @"psi\lexer"; }
    }

    [Test] public void Test01() { DoNamedTest(); }
    [Test] public void Test02() { DoNamedTest(); }
    [Test] public void Test03() { DoNamedTest(); }
    [Test] public void Test04() { DoNamedTest(); }
    [Test] public void Test05() { DoNamedTest(); }
    [Test] public void Test06() { DoNamedTest(); }
    [Test] public void Test07() { DoNamedTest(); }
  }
}