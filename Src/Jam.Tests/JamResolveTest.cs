using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace JetBrains.ReSharper.Psi.Jam.Tests
{
  [TestFixture]
  [Category("Resolve")]
  [TestFileExtension(".jam")]
  public class JamResolveTest : ReferenceTestBase
  {
    protected override string RelativeTestDataPath
    {
      get { return @"resolve"; }
    }

    protected override bool AcceptReference(IReference reference)
    {
      return true;
    }

    protected override string Format(IDeclaredElement declaredElement, ISubstitution substitution, PsiLanguageType languageType, DeclaredElementPresenterStyle presenter, IReference reference)
    {
      return base.Format(declaredElement, substitution, languageType, DeclaredElementPresenter.KIND_NAME_PRESENTER, reference);
    }

    [Test] public void Test001() {DoNamedTest();}
    [Test] public void Test002() {DoNamedTest();}
    [Test] public void Test003() {DoNamedTest();}
  }
}