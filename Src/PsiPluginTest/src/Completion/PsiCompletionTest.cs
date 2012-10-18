using System;
using JetBrains.ReSharper.Feature.Services.Tests.CSharp.FeatureServices.CodeCompletion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace PsiPluginTest.Completion
{
  [TestFixture]
  [TestReferences("System.Core.dll")]
  [Category("Code Completion")]
  public class PsiCompletionTest : CodeCompletionTestBase
  {
    protected override String RelativeTestDataPath
    {
      get { return @"completion"; }
    }

    [Test]
    public void test001()
    {
      DoTestFiles("test001.psi");
    }

    [Test]
    public void test002()
    {
      DoTestFiles("test002.psi");
    }

    [Test]
    public void test003()
    {
      DoTestFiles("test003.psi");
    }

    [Test]
    public void test004()
    {
      DoTestFiles("test004.psi");
    }

    [Test]
    public void test005()
    {
      DoTestFiles("test005.psi");
    }

    protected override bool ExecuteAction
    {
      get { return true; }
    }
  }
}