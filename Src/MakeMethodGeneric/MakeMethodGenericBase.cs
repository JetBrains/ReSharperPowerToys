using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.PowerToys.MakeMethodGeneric.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Refactorings.Common;
using JetBrains.ReSharper.Refactorings.Workflow;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric
{
  /// <summary>
  /// Base class of language specific implementation. 
  /// </summary>
  public abstract class MakeMethodGenericBase : RefactoringExecBase<MakeMethodGenericWorkflow, MakeMethodGenericRefactoring>
  {
    protected MakeMethodGenericBase(MakeMethodGenericWorkflow workflow, ISolution solution, IRefactoringDriver driver) : base(workflow, solution, driver)
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