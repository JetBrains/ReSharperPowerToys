using JetBrains.ReSharper.Intentions.Html.Tests.ContextActions;
using NUnit.Framework;
using XmlAndHtml;

namespace XmlAndHtmlTests
{
  [TestFixture]
  class SpecifyIdHtmlContextActionExecuteTests : HtmlContextActionExecuteTestBase<SpecifyIdHtmlContextAction>
  {
    protected override string ExtraPath
    {
      get { return "SpecifyIdHtmlExecute"; }
    }

    [Test]
    [TestCase("execute01")]
    public void TestExecution(string src)
    {
      DoOneTest(src);
    }
  }
}