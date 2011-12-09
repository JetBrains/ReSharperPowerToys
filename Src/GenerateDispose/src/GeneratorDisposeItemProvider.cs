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

using System.Collections.Generic;
using System.Drawing;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Generate;
using JetBrains.ReSharper.Feature.Services.Generate.Actions;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.DeclaredElements;

namespace JetBrains.ReSharper.PowerToys.GenerateDispose
{
  [ActionHandler("Generate.Dispose")]
  public class GenerateDisposeAction : GenerateActionBase<GenerateDisposeItemProvider>
  {
    protected override bool ShowMenuWithOneItem
    {
      get { return true; }
    }

    protected override UI.RichText.RichText Caption
    {
      get { return "Generate Dispose"; }
    }
  } 

  [GenerateProvider]
  public class GenerateDisposeItemProvider : IGenerateActionProvider
  {
    public IEnumerable<IGenerateActionWorkflow> CreateWorkflow(IDataContext dataContext)
    {
      var solution = dataContext.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      var iconManager = solution.GetComponent<PsiIconManager>();
      var icon = iconManager.GetImage(CLRDeclaredElementType.METHOD);
      yield return new GenerateDisposeActionWorkflow(icon);
    }
  }

  public class GenerateDisposeActionWorkflow : StandardGenerateActionWorkflow
  {
    public GenerateDisposeActionWorkflow(Image icon)
      : base("Dispose", icon, "Dispose", GenerateActionGroup.CLR_LANGUAGE, "Generate dispose", 
        "Generate a Dispose() implementation which disposes selected fields.", "Generate.Dispose")
    {
    }

    public override double Order
    {
      get { return 100; }
    }

    /// <summary>
    /// This method is redefined in order to get rid of the IsKindAllowed() check at the end.
    /// </summary>
    public override bool IsAvailable(IDataContext dataContext)
    {
      var solution = dataContext.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      if (solution == null)
        return false;

      var generatorManager = GeneratorManager.GetInstance(solution);
      if (generatorManager == null)
        return false;

      var languageType = generatorManager.GetPsiLanguageFromContext(dataContext);
      if (languageType == null)
        return false;

      var generatorContextFactory = LanguageManager.Instance.TryGetService<IGeneratorContextFactory>(languageType);
      return generatorContextFactory != null;
    }
  }
}