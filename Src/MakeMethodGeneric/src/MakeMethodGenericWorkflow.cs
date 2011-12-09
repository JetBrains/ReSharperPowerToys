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

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Application.DataContext;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Naming;
using JetBrains.ReSharper.Psi.Naming.Extentions;
using JetBrains.ReSharper.Psi.Naming.Impl;
using JetBrains.ReSharper.Psi.Naming.Settings;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Refactorings.Workflow;
using System.Linq;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric
{
  /// <summary>
  /// </summary>
  public class MakeMethodGenericWorkflow : DrivenRefactoringWorkflow
  {
    private readonly SearchDomainFactory mySearchDomainFactory;

    public MakeMethodGenericWorkflow(ISolution solution, string actionId, SearchDomainFactory searchDomainFactory)
      : base(solution, actionId)
    {
      mySearchDomainFactory = searchDomainFactory;
    }

    [NotNull]
    public string TypeParameterName { get; set; }

    [NotNull]
    public IDeclaredElementPointer<IMethod> MethodPointer { get; private set; }

    [NotNull]
    public IDeclaredElementPointer<IParameter> ParameterPointer { get; private set; }

    public override RefactoringActionGroup ActionGroup
    {
      get { throw new NotImplementedException(); }
    }

    public override string HelpKeyword
    {
      get
      {
        //
        return null;
      }
    }

    /// <summary>
    /// UI page to be shown first. return 'null' to start refactoring immediately. 
    /// </summary>
    public override IRefactoringPage FirstPendingRefactoringPage
    {
      get { return new MakeMethodGenericPage(this); }
    }

    /// <summary>
    /// This property determines whether to show 'enable undo...' checkbox in wizard form.
    /// </summary>
    public override bool MightModifyManyDocuments
    {
      get { return true; }
    }

    public override string Title
    {
      get { return "Make Method Generic"; }
    }

    /// <summary>
    /// Execution of refatoring starts here. Data from context is initialized. 
    /// </summary>
    public override bool Initialize(IDataContext context)
    {
      IParameter parameter;
      IMethod method;
      if (!IsAvailableInternal(context, out parameter, out method))
        return false;

      // Never reference DeclaredElements, References, Types, etc. from workflow. Use pointers!
      // Code can be edited during execution end elements (types, references) can be invalidated. 
      MethodPointer = method.CreateElementPointer();
      ParameterPointer = parameter.CreateElementPointer();

      // use also:
      // PsiManager.GetInstance(Solution).CreateReferencePointer(...);
      // PsiManager.GetInstance(Solution).CreateTreeElementPointer(...);

      // following code produces name for type parameter using parameter name...
      NamingManager namingManager = method.GetPsiServices().Naming;
      var suggestionOptions = new SuggestionOptions {DefaultName = "T"};
      
      TypeParameterName = namingManager.Suggestion.GetDerivedName(parameter, NamedElementKinds.TypeParameters,
                                                                  ScopeKind.Common, parameter.PresentationLanguage, suggestionOptions, null);
      return true;
    }

    /// <summary>
    /// This method is used by refactoring action handler. Should be as quick as possible. 
    /// </summary>
    public override bool IsAvailable(IDataContext context)
    {
      IParameter parameter;
      IMethod method;
      if (!IsAvailableInternal(context, out parameter, out method))
        return false;

      return true;
    }

    public override void SuccessfulFinish(IProgressIndicator pi)
    {
      throw new NotImplementedException();
    }

    private bool IsAvailableInternal(IDataContext context, out IParameter systemTypeParameter, out IMethod method)
    {
      systemTypeParameter = null;
      method = null;

      var declaredElements = context.GetData(Psi.Services.DataConstants.DECLARED_ELEMENTS);
      if (declaredElements == null)
        return false;

      method = declaredElements.OfType<IMethod>().FirstOrDefault();
      if (method == null)
        return false;

      if (method is ICompiledElement)
        return false;

      IList<IDeclaration> declarations = method.GetDeclarations();
      if (declarations.Count == 0)
        return false;

      IList<IParameter> parameters = method.Parameters;
      if (parameters.Count == 0)
        return false;

      IPsiModule module = method.Module;

      IDeclaredType systemType = TypeFactory.CreateTypeByCLRName("System.Type", module);

      foreach (IParameter parameter in parameters)
        if (parameter.Type.Equals(systemType))
          systemTypeParameter = parameter;

      if (systemTypeParameter == null)
        return false;

      return true;
    }

    /// <summary>
    /// Last step of refactoring. This code is executed when all changes are made and PSI transaction is committed.
    /// Usal actions here are: project model changes (e.g. file rename), textual changes in documents. 
    /// </summary>
    public override bool PostExecute(IProgressIndicator progressIndicator)
    {
      return false;
    }

    /// <summary>
    /// Returns refactoring class that executed PSI transaction.
    /// </summary>
    public override IRefactoringExecuter CreateRefactoring(IRefactoringDriver driver)
    {
      return new MakeMethodGenericRefactoring(this, Solution, driver, mySearchDomainFactory);
    }

    public bool IsValid()
    {
      return MethodPointer.FindDeclaredElement() != null &&
             ParameterPointer.FindDeclaredElement() != null;
    }
  }
}