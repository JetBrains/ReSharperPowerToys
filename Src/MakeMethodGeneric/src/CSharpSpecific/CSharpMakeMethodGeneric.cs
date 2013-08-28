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

using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.PowerToys.MakeMethodGeneric.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CSharp.Util;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Refactorings.Conflicts;
using JetBrains.ReSharper.Refactorings.Workflow;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric.CSharpSpecific
{
  public class CSharpMakeMethodGeneric : MakeMethodGenericBase
  {
    public CSharpMakeMethodGeneric(MakeMethodGenericWorkflow workflow, ISolution solution, IRefactoringDriver driver)
      : base(workflow, solution, driver)
    {
    }

    public override MethodInvocation ProcessUsage(IReference reference)
    {
      var referenceExpression = reference.GetTreeNode() as IReferenceExpression;
      if (referenceExpression == null)
      {
        Driver.AddConflict(ReferenceConflict.CreateError(reference, "{0} can not be updated correctly.", "Usage"));
        return null;
      }

      bool isExtensionMethod = referenceExpression.IsExtensionMethod();
      IInvocationExpression invocation = InvocationExpressionNavigator.GetByInvokedExpression(referenceExpression);
      if (invocation == null)
      {
        Driver.AddConflict(ReferenceConflict.CreateError(reference, "{0} can not be updated correctly.", "Usage"));
        return null;
      }

      ITreeNode element = GetArgument(invocation, isExtensionMethod);

      var argument = element as ICSharpArgument;
      IType type = argument != null ? GetTypeOfValue(argument.Value) : GetTypeOfValue(element);
      if (type == null || !type.CanUseExplicitly(invocation))
      {
        Driver.AddConflict(ReferenceConflict.CreateError(
          reference, "Arguemnt of {0} is not valid 'typeof' expression.", "usage"));
        return null;
      }

      // we can rely on resolve result since method declaration is not yet changed. 
      ResolveResultWithInfo resolveResult = reference.Resolve();
      ISubstitution substitution = resolveResult.Result.Substitution;
      var method = resolveResult.DeclaredElement as IMethod;
      if (method == null)
        return null;

      if (argument != null)
      {
        invocation.RemoveArgument(argument);
        return new MethodInvocation(reference, type, method, substitution);
      }

      CSharpElementFactory factory = CSharpElementFactory.GetInstance(invocation.GetPsiModule());
      IReferenceExpression newInvokedExpression =
        invocation.InvokedExpression.ReplaceBy(factory.CreateReferenceExpression("$0", Executer.Method));
      return new MethodInvocation(newInvokedExpression.Reference, type, method, substitution);
    }

    public override void RemoveParameter(IDeclaration declaration, int index)
    {
      var methodDeclaration = declaration as IMethodDeclaration;
      if (methodDeclaration != null)
      {
        var parameterDeclarations = methodDeclaration.ParameterDeclarations;
        if (index < parameterDeclarations.Count)
          methodDeclaration.RemoveParameterDeclaration((IRegularParameterDeclaration)parameterDeclarations[index]);
      }
    }

    public override void BindUsage(MethodInvocation usage, ITypeParameter typeParameter)
    {
      ISubstitution substitution = usage.Substitution;
      // extend old substitution with new pair...
      if (typeParameter != null)
        substitution = usage.Substitution.Extend(new[] {typeParameter}, new[] {usage.Type});
      // run bind...
      usage.Reference.BindTo(usage.Method, substitution);
      // type argument is now inserted. 
    }

    public override ITypeParameter AddTypeParameter(IDeclaration declaration)
    {
      var methodDeclaration = declaration as IMethodDeclaration;
      if (methodDeclaration != null)
      {
        CSharpElementFactory factory = CSharpElementFactory.GetInstance(declaration.GetPsiModule());
        ITypeParameterOfMethodDeclaration parameter =
          methodDeclaration.AddTypeParameterBefore(
            factory.CreateTypeParameterOfMethodDeclaration(Workflow.TypeParameterName), null);
        return parameter.DeclaredElement;
      }
      return null;
    }

    public override void ProcessParameterReference(IReference reference)
    {
      var referenceExpression = reference as IReferenceExpression;
      if (referenceExpression != null)
      {
        CSharpElementFactory factory = CSharpElementFactory.GetInstance(referenceExpression.GetPsiModule());
        referenceExpression.ReplaceBy(factory.CreateExpression("typeof($0)", Workflow.TypeParameterName));
      }
    }

    [CanBeNull]
    private static IType GetTypeOfValue(ITreeNode value)
    {
      var typeofExpression = value as ITypeofExpression;
      if (typeofExpression != null)
      {
        var isOpenType = typeofExpression.IsOpenType();
        if (isOpenType == null || isOpenType == true)
          return null;
        return typeofExpression.ArgumentType;
      }
      return null;
    }

    [CanBeNull]
    private ITreeNode GetArgument(IInvocationExpression invocation, bool isExtensionMethod)
    {
      var containingParametersOwner = Executer.Parameter.ContainingParametersOwner;
      if (containingParametersOwner == null)
        return null;

      int parameterIndex = containingParametersOwner.Parameters.IndexOf(Executer.Parameter);
      IList<ICSharpArgument> arguments = invocation.Arguments;
      if (isExtensionMethod)
      {
        // special treatment of extention methods.
        if (parameterIndex == 0)
        {
          return ((IReferenceExpression) invocation.InvokedExpression).QualifierExpression;
        }
        if (parameterIndex - 1 < arguments.Count)
        {
          return arguments[parameterIndex - 1];
        }
      }
      else
      {
        if (parameterIndex < arguments.Count)
        {
          return arguments[parameterIndex];
        }
      }

      return null;
    }
  }
}