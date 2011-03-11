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