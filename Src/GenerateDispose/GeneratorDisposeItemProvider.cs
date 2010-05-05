using System.Collections.Generic;
using JetBrains.ActionManagement;
using JetBrains.ActivityTracking;
using JetBrains.IDE;
using JetBrains.ReSharper.Feature.Services.Generate;
using JetBrains.ReSharper.Features.Altering.Generate;
using JetBrains.ReSharper.Psi;
using JetBrains.Util;

// Register event for ActivityTracking, required as we use StandardGeneratorItem
[assembly: RegisterEvent("Generate.Dispose", EventType.WITH_START_FINISH)]

namespace JetBrains.ReSharper.PowerToys.GenerateDispose
{
  /// <summary>
  /// Describes item in generate menu, we reuse StandardGeneratorItem
  /// </summary>
  [GeneratorItemProvider]
  public class GeneratorDisposeItemProvider : IGeneratorItemProvider
  {
    public IEnumerable<IGeneratorItem> GetItems(IDataContext context)
    {
      var textControl = context.GetData(DataConstants.TEXT_CONTROL);
      var solution = context.GetData(DataConstants.SOLUTION);
      if (textControl == null || solution == null)
        return EmptyArray<IGeneratorItem>.Instance;

      return new[]
               {
                 new StandardGeneratorItem("Dispose",
                                           PsiIconManager.Instance.GetImage(CLRDeclaredElementType.METHOD,
                                                                            PsiIconExtension.None),
                                           "Dispose",
                                           0,
                                           "Generate Dispose",
                                           "Select Disposable fields which should be disposed in generated method")
                 ,
               };
    }
  }
}