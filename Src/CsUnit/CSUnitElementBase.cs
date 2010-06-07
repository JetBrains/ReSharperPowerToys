using System;
using System.Collections.Generic;
using JetBrains.Application;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.CsUnit
{
  public abstract class CSUnitElementBase : UnitTestElement
  {
    private readonly ProjectModelElementEnvoy myProject;
    private readonly string myTypeName;

    protected CSUnitElementBase(IUnitTestProvider provider, CSUnitElementBase parent, IProject project, string typeName)
      : base(provider, parent)
    {
      if (project == null && !Shell.Instance.IsTestShell)
        throw new ArgumentNullException("project");
      if (typeName == null)
        throw new ArgumentNullException("typeName");

      if (project != null) 
        myProject = new ProjectModelElementEnvoy(project);
      myTypeName = typeName;
    }

    public override IProject GetProject()
    {
      return myProject.GetValidProjectElement() as IProject;
    }

    protected ITypeElement GetDeclaredType()
    {
      IProject project = GetProject();
      if (project == null)
        return null;
      PsiManager manager = PsiManager.GetInstance(project.GetSolution());
      using (ReadLockCookie.Create())
      {
        IDeclarationsCache declarationsCache = manager.GetDeclarationsCache(DeclarationsScopeFactory.ModuleScope(PsiModuleManager.GetInstance(project.GetSolution()).GetPrimaryPsiModule(project), true), true);
        return declarationsCache.GetTypeElementByCLRName(myTypeName);
      }
    }

    public override string GetTypeClrName()
    {
      return myTypeName;
    }

    public override UnitTestNamespace GetNamespace()
    {
      return new UnitTestNamespace(new CLRTypeName(myTypeName).NamespaceName);
    }

    public override IList<IProjectFile> GetProjectFiles()
    {
      ITypeElement declaredType = GetDeclaredType();
      if (declaredType == null)
        return EmptyArray<IProjectFile>.Instance;
      return declaredType.GetProjectFiles();
    }

    public override UnitTestElementDisposition GetDisposition()
    {
      IDeclaredElement element = GetDeclaredElement();
      if (element != null && element.IsValid())
      {
        var locations = new List<UnitTestElementLocation>();
        foreach (IDeclaration declaration in element.GetDeclarations())
        {
          IFile file = declaration.GetContainingFile();
          if (file != null)
            locations.Add(new UnitTestElementLocation(file.ProjectFile, declaration.GetNameDocumentRange().TextRange, declaration.GetDocumentRange().TextRange));
        }
        return new UnitTestElementDisposition(locations, this);
      }
      return UnitTestElementDisposition.InvalidDisposition;
    }

    public override bool Equals(object obj)
    {
      if (!base.Equals(obj))
        return false;
      var elementBase = (CSUnitElementBase) obj;
      return Equals(elementBase.myProject, myProject) && elementBase.myTypeName == myTypeName;
    }

    public override int GetHashCode()
    {
      int result = base.GetHashCode();
      result = 29*result + myProject.GetHashCode();
      result = 29*result + myTypeName.GetHashCode();
      return result;
    }
  }
}