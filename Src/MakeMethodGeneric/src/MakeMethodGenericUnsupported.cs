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

using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Refactorings;
using JetBrains.ReSharper.PowerToys.MakeMethodGeneric.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Refactorings.Conflicts;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric
{
  /// <summary>
  /// This is default implementation. Most likely we can do nothing when language is not supported/
  /// </summary>
  public class MakeMethodGenericUnsupported : MakeMethodGenericBase
  {
    public MakeMethodGenericUnsupported(MakeMethodGenericWorkflow workflow, ISolution solution,
                                        IRefactoringDriver driver) : base(workflow, solution, driver)
    {
    }

    public override MethodInvocation ProcessUsage(IReference reference)
    {
      // when something goes wrong just add conflict
      Driver.AddConflict(new UnsupportedLanguageConflict(reference.GetTreeNode(), "usage", ConflictSeverity.Error));
      return null;
    }

    public override void RemoveParameter(IDeclaration declaration, int index)
    {
      Driver.AddConflict(new UnsupportedLanguageConflict(declaration, "method declaration", ConflictSeverity.Error));
    }

    public override ITypeParameter AddTypeParameter(IDeclaration declaration)
    {
      Driver.AddConflict(new UnsupportedLanguageConflict(declaration, "method declaration", ConflictSeverity.Error));
      return null;
    }

    public override void ProcessParameterReference(IReference reference)
    {
      Driver.AddConflict(new UnsupportedLanguageConflict(reference.GetTreeNode(), "parameter usage",
                                                         ConflictSeverity.Error));
    }
  }
}