using JetBrains.ProjectModel;
using JetBrains.ReSharper.PowerToys.MakeMethodGeneric.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Refactorings.Conflicts;
using JetBrains.ReSharper.Refactorings.Workflow;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric
{
  /// <summary>
  /// This is default implementation. Most likely we can do nothing when language is not supported/
  /// </summary>
  public class MakeMethodGenericUnsupported : MakeMethodGenericBase
  {
    public MakeMethodGenericUnsupported(MakeMethodGenericWorkflow workflow, ISolution solution, IRefactoringDriver driver) : base(workflow, solution, driver)
    {
    }

    public override MethodInvocation ProcessUsage(IReference reference)
    {
      // when something goes wrong just add conflict
      Driver.AddConflict(new UnsupportedLanguageConflict(reference.GetElement(), "usage", ConflictSeverity.Error));
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
      Driver.AddConflict(new UnsupportedLanguageConflict(reference.GetElement(), "parameter usage", ConflictSeverity.Error));
    }
  }
}