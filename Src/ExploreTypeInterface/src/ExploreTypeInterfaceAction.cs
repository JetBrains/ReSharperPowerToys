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
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Feature.Services.Util;
using DataConstants = JetBrains.ReSharper.Psi.Services.DataConstants;
using MessageBox = JetBrains.Util.MessageBox;
using System.Linq;

namespace JetBrains.ReSharper.PowerToys.ExploreTypeInterface
{
  /// <summary>
  /// Handles ExploreTypeInterface action, see Actions.xml
  /// </summary>
  [ActionHandler("PowerToys.ExploreTypeInterface")]
  public class ExploreTypeInterfaceAction : IActionHandler
  {
    #region IActionHandler Members

    public void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      // Get solution from context in which action is executed
      ISolution solution = context.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      if (solution == null)
        return;

      // Get PsiManager for solution, from which we will obtain code elements
      var services = solution.GetPsiServices();

      if (!services.PsiManager.AllDocumentsAreCommited)
        return;

      using (CommitCookie caches = CommitCookie.Commit(solution).WaitForCaches(this))
      {
        if (caches.Cancelled)
          return;

        bool instanceOnly;
        ITypeElement typeElement = TypeInterfaceUtil.GetTypeElement(context, out instanceOnly);
        if (typeElement == null)
        {
          MessageBox.ShowExclamation("Cannot get type from current location", "Explore Type Interface");
          return;
        }

        // Create descriptor and ask TreeModelBrowser to show it in the HierarchyResults view. 
        // Same view, where type hierarchy is shown
        var descriptor = new TypeInterfaceDescriptor(typeElement, instanceOnly);
        var windowRegistrar = solution.GetComponent<TypeInterfaceToolWindowRegistrar>();
        windowRegistrar.Show(descriptor);
      }
    }

    public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      var declaredElements = context.GetData(DataConstants.DECLARED_ELEMENTS);
      return declaredElements != null && declaredElements.Any();
    }

    #endregion
  }
}