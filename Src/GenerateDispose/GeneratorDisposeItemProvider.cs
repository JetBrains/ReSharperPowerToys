using System.Collections.Generic;
using System.Drawing;
using JetBrains.ActionManagement;
using JetBrains.ActivityTracking;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Generate;
using JetBrains.ReSharper.Feature.Services.Generate.Actions;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.DeclaredElements;

[assembly: RegisterEvent("Generate.Dispose", EventType.WITH_START_FINISH)]

namespace JetBrains.ReSharper.PowerToys.GenerateDispose
{
  [ActionHandler("Generate.Dispose")]
  public class GenerateDisposeAction : GenerateActionBase<GenerateDisposeItemProvider>
  {
    protected override bool ShowMenuWithOneItem
    {
      get { return true; }
    }

    protected override UI.RichText.RichText Caption
    {
      get { return "Generate Dispose"; }
    }
  } 

  [GenerateProvider]
  public class GenerateDisposeItemProvider : IGenerateActionProvider
  {
    public IEnumerable<IGenerateActionWorkflow> CreateWorkflow(IDataContext dataContext)
    {
      var solution = dataContext.GetData(IDE.DataConstants.SOLUTION);
      var iconManager = solution.GetComponent<PsiIconManager>();
      var icon = iconManager.GetImage(CLRDeclaredElementType.METHOD);
      yield return new GenerateDisposeActionWorkflow(icon);
    }
  }

  public class GenerateDisposeActionWorkflow : StandardGenerateActionWorkflow
  {
    public GenerateDisposeActionWorkflow(Image icon)
      : base("Dispose", icon, "&Dispose", GenerateActionGroup.CLR_LANGUAGE, "Generate dispose", 
        "Generate a Dispose() implementation which disposes selected fields.", "Generate.Dispose")
    {
    }

    public override double Order
    {
      get { return 100; }
    }

    /// <summary>
    /// This method is redefined in order to get rid of the IsKindAllowed() check at the end.
    /// </summary>
    public override bool IsAvailable(IDataContext dataContext)
    {
      var solution = dataContext.GetData(IDE.DataConstants.SOLUTION);
      if (solution == null)
        return false;

      var generatorManager = GeneratorManager.GetInstance(solution);
      if (generatorManager == null)
        return false;

      var languageType = generatorManager.GetPsiLanguageFromContext(dataContext);
      if (languageType == null)
        return false;

      var generatorContextFactory = LanguageManager.Instance.TryGetService<IGeneratorContextFactory>(languageType);
      return generatorContextFactory != null;
    }
  }
}