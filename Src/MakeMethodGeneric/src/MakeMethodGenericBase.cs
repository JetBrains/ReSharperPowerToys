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

using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Refactorings;
using JetBrains.ReSharper.PowerToys.MakeMethodGeneric.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric
{
  /// <summary>
  /// Base class of language specific implementation. 
  /// </summary>
  public abstract class MakeMethodGenericBase :
    RefactoringExecBase<MakeMethodGenericWorkflow, MakeMethodGenericRefactoring>
  {
    protected MakeMethodGenericBase(MakeMethodGenericWorkflow workflow, ISolution solution, IRefactoringDriver driver)
      : base(workflow, solution, driver)
    {
    }

    public abstract MethodInvocation ProcessUsage(IReference reference);

    public abstract void RemoveParameter(IDeclaration declaration, int index);

    public virtual void BindUsage(MethodInvocation usage, [CanBeNull] ITypeParameter parameter)
    {
    }

    public abstract ITypeParameter AddTypeParameter(IDeclaration declaration);

    public abstract void ProcessParameterReference(IReference reference);
  }
}