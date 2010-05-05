using JetBrains.ProjectModel;
using JetBrains.ReSharper.Refactorings;
using JetBrains.ReSharper.Refactorings.Workflow;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric
{
  /// <summary>
  /// Base class of language specific parts instantiator. 
  /// </summary>
  public abstract class PowerToyRefactoringsLanguageService : IRefactoringLanguageService
  {
    public virtual MakeMethodGenericBase CreateMakeMethodGeneric(MakeMethodGenericWorkflow workflow, ISolution solution, IRefactoringDriver driver)
    {
      return null;
    }
  }
}