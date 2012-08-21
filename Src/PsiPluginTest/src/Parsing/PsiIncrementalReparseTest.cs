using System;
using JetBrains.ReSharper.PsiPlugin.PsiGrammar;
using JetBrains.ReSharper.PsiTests.parsing;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace PsiPluginTest.Parsing
{
  [Category("PSI")]
  [TestFileExtension(PsiProjectFileType.PsiExtension)]
  class PsiIncrementalReparseTest : IncrementalReparseTestBase
  {
    protected override String RelativeTestDataPath
    {
      get { return @"parsing\reparse"; }
    }

    [Test]
    public void test001()
    {
      DoTestFiles("test001.psi");
    }
  }
}