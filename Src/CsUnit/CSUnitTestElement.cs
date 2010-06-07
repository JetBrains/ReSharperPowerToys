using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.Text;

namespace JetBrains.ReSharper.PowerToys.CsUnit
{
  public class CSUnitTestElement : CSUnitElementBase
  {
    private readonly CSUnitTestFixtureElement myFixture;
    private readonly string myMethodName;
    private readonly int myOrder;

    public CSUnitTestElement(IUnitTestProvider provider, CSUnitTestFixtureElement fixture, IProject project, string declaringTypeName, string methodName, int order)
      : base(provider, fixture, project, declaringTypeName)
    {
      myFixture = fixture;
      myOrder = order;
      myMethodName = methodName;
    }

    public override string GetTitle()
    {
      return string.Format("{0}.{1}", myFixture.GetTitle(), myMethodName);
    }

    public override string ShortName
    {
      get { return myMethodName; }
    }

    public override bool Matches(string filter, IdentifierMatcher matcher)
    {
      if (myFixture.Matches(filter, matcher))
        return true;
      return matcher.Matches((myMethodName));
    }

    public CSUnitTestFixtureElement Fixture
    {
      get { return myFixture; }
    }

    public string MethodName
    {
      get { return myMethodName; }
    }

    public int Order
    {
      get { return myOrder; }
    }

    public override IDeclaredElement GetDeclaredElement()
    {
      ITypeElement declaredType = GetDeclaredType();
      if (declaredType == null)
        return null;
      foreach (ITypeMember member in declaredType.EnumerateMembers(myMethodName, false))
      {
        var method = member as IMethod;
        if (method == null)
          continue;
        if (method.IsAbstract)
          continue;
        if (method.TypeParameters.Length > 0)
          continue;
        if (method.AccessibilityDomain.DomainType != AccessibilityDomain.AccessibilityDomainType.PUBLIC)
          continue;
        return member;
      }
      return null;
    }

    public override string GetKind()
    {
      return "csUnit Test";
    }

    public override bool Equals(object obj)
    {
      if (!base.Equals(obj))
        return false;
      var element = (CSUnitTestElement) obj;
      return Equals(myFixture, element.myFixture) && myMethodName == element.myMethodName;
    }

    public override int GetHashCode()
    {
      int result = base.GetHashCode();
      result = 29*result + myFixture.GetHashCode();
      result = 29*result + myMethodName.GetHashCode();
      return result;
    }
  }
}