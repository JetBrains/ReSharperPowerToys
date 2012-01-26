using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Feature.Services.Tests.CSharp.FeatureServices.CodeCompletion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace PsiPluginTest.Completion
{
  [TestFixture]
  [TestReferences("System.Core.dll")]
  [Category("Code Completion")]
  //[Category("PSI")]
  //[TestFileExtension(JavaScriptProjectFileType.JS_EXTENSION)]
  public class PsiCompletionTest : CodeCompletionTestBase
  {
    protected override String RelativeTestDataPath
    {
      get { return @"completion"; }
    }

    /*protected override IEnumerable<CodeCompletionType> GetCodeCompletionTypes()
    {
      return new[] { CodeCompletionType.BasicCompletion};
    }*/

    [Test]
    public void test001() { DoTestFiles("test001.psi"); }

    [Test]
    public void test002() { DoTestFiles("test002.psi"); }

    [Test]
    public void test003() { DoTestFiles("test003.psi"); }

    protected override bool ExecuteAction
    {
      get { return true; }
    }
  }
}
