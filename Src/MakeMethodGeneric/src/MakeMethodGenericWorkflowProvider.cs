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
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Refactorings.Workflow;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric
{
  [RefactoringWorkflowProviderAttribute]
  public class MakeMethodGenericWorkflowProvider : IRefactoringWorkflowProvider
  {
    public RefactoringActionGroup ActionGroup
    {
      get { return RefactoringActionGroup.Unsorded; }
    }

    public IEnumerable<IRefactoringWorkflow> CreateWorkflow(IDataContext dataContext)
    {
      return null;
    }

    public IRefactoringWorkflow CreateWorkflow(ISolution solution, string actionId, SearchDomainFactory factory)
    {
      return new MakeMethodGenericWorkflow(solution, actionId, factory);
    }
  }
}