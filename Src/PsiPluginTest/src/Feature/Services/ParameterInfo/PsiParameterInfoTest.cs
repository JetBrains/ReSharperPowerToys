using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Feature.Services.ParameterInfo.Tests;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace PsiPluginTest.Feature.Services.ParameterInfo
{
  [Category("PSI")]
  [TestFileExtension(PsiProjectFileType.PsiExtension)]
  public class PsiParameterInfoTest : ParameterInfoTestBase
  {
    protected override string RelativeTestDataPath
    {
      get { return @"feautere\services\parameterInfo"; }
    }

    [Test]
    public void test001()
    {
      DoNamedTest();
    }
  }
}