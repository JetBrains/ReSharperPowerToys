/*
 * Copyright 2007-2014 JetBrains
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
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Refactorings.Workflow;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric.CSharpSpecific
{
  /// <summary>
  /// C# specific implementation. 
  /// </summary>
  [Language(typeof (CSharpLanguage))]
  public class CSharpPowerToyRefactoringsLanguageService : PowerToyRefactoringsLanguageService
  {
    public override MakeMethodGenericBase CreateMakeMethodGeneric(MakeMethodGenericWorkflow workflow, ISolution solution,
                                                                  IRefactoringDriver driver)
    {
      return new CSharpMakeMethodGeneric(workflow, solution, driver);
    }
  }
}