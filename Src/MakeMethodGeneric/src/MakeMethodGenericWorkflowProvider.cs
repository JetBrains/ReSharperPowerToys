/*
 * Copyright 2007-2011 JetBrains
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

using System;
using System.Collections.Generic;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Refactorings.Workflow;
using DataConstants = JetBrains.ProjectModel.DataContext.DataConstants;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric
{
  [RefactoringWorkflowProviderAttribute]
  public class MakeMethodGenericWorkflowProvider : IRefactoringWorkflowProvider
  {
    public IEnumerable<IRefactoringWorkflow> CreateWorkflow(IDataContext dataContext)
    {
      var solution = dataContext.GetData(DataConstants.SOLUTION);
      if (solution == null)
        yield break;

      yield return new MakeMethodGenericWorkflow(solution, "MakeMethodGeneric");
    }
  }
}