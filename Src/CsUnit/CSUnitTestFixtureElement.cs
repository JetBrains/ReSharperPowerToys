using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.Text;

namespace JetBrains.ReSharper.PowerToys.CsUnit
{
  public class CSUnitTestFixtureElement : CSUnitElementBase
  {
    private readonly string myAssemblyLocation;

    public CSUnitTestFixtureElement(IUnitTestProvider provider, IProject project, string typeName, string assemblyLocation)
      : base(provider, null, project, typeName)
    {
      myAssemblyLocation = assemblyLocation;
    }

    public string AssemblyLocation
    {
      get { return myAssemblyLocation; }
    }

    public override bool Matches(string filter, IdentifierMatcher matcher)
    {
      return matcher.Matches(GetTypeClrName());
    }

    public override string GetTitle()
    {
      return new CLRTypeName(GetTypeClrName()).ShortName;
    }

    public override string ShortName
    {
      get { return GetTitle(); }
    }

    public override IDeclaredElement GetDeclaredElement()
    {
      PsiManager manager = PsiManager.GetInstance(GetSolution());
      IDeclarationsCache declarationsCache = manager.GetDeclarationsCache(DeclarationsScopeFactory.ModuleScope(PsiModuleManager.GetInstance(GetSolution()).GetPrimaryPsiModule(GetProject()), false), true);
      return declarationsCache.GetTypeElementByCLRName(GetTypeClrName());
    }

    public override string GetKind()
    {
      return "csUnit Fixture";
    }
  }
}