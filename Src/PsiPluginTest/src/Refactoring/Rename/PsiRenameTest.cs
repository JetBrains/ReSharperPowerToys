using System;
using JetBrains.ReSharper.Refactorings;
using NUnit.Framework;

namespace PsiPluginTest.Parsing.Rename
{
  [Category("PSI")]
  class PsiRenameTest : RenameTestBase
  {
    protected override String RelativeTestDataPath
    {
      get { return @"refactoring\rename"; }
    }

    [Test]
    public void test001()
    {
      DoTestFiles(new[] { "test001.psi" });
    }

    [Test]
    public void test002()
    {
      DoTestFiles(new[] { "test002.psi" });
    }
    [Test]
    public void test003()
    {
      DoTestFiles(new[] { "test003.psi" });
    }

    [Test]
    public void test004()
    {
      DoTestFiles(new[] { "test004.psi" });
    }
  }
}
