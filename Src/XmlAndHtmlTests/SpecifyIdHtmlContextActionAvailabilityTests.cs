using JetBrains.ReSharper.Intentions.Html.Tests.ContextActions;
using NUnit.Framework;
using XmlAndHtml;

namespace XmlAndHtmlTests
{
  [TestFixture]
  public class SpecifyIdHtmlContextActionAvailabilityTests : HtmlContextActionAvailabilityTestBase<SpecifyIdHtmlContextAction>
  {
    protected override string ExtraPath
    {
      get { return "SpecifyIdHtmlAvailability"; }
    }

    [Test]
    [TestCase("availability01")]
    public void TestAvailability(string src)
    {
      DoOneTest(src);
    }
  }
}
