using JetBrains.ReSharper.Feature.Services.Tests.FeatureServices.RearrangeCode;
using NUnit.Framework;

namespace PsiPluginTest.Feature.Services.RearrangeCode
{
  [Category("PSI")]
  public class PsiRearrangeCodeTest : RearrangeTestBase
  {
    protected override string RelativeTestDataPath
    {
      get { return @"feautere\services\rearrangeCode"; }
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
  }
}