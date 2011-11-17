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

using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.MenuItem
{
  [ActionHandler("AddMenuItem.ShowCurrentSolution")]
  internal class ShowCurrentSolutionAction : IActionHandler
  {
    #region IActionHandler Members

    public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      // fetch active solution from context
      ISolution solution = context.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);

      // enable this action if there is an active solution, disable otherwise
      return solution != null;
    }

    public void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      // Fetch active solution from context.

      // It should be not null because it is checked in "Update". 
      // "Execute" is guaranteed to not be invoked if "Update" returns false.
      ISolution solution = context.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      if (solution == null)
        return;

      string message = string.Format("Currently active solution is {0}", solution.Name);
      MessageBox.ShowInfo(message, "AddMenuItem Sample Plugin");
    }

    #endregion
  }
}