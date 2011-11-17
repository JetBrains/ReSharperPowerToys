/*
 * Copyright 2007-2011 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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