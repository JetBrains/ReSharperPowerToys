using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.Cach
{
  [SolutionComponent]
  internal class PsiStandartCacheProvider : IPsiCacheProvider
  {
    public int Version
    {
      get
      {
        return 8;
      }
    }

    public IPsiCustomCacheBuilder CreateCustomBuilder(IPsiSourceFile sourceFile, bool isFrameworkFile)
    {
      return new PsiStandardCacheBuilder(sourceFile);
    }

    public Func<IPsiSourceFile, IPersistentCacheItem> CreateItemConstructor(string guid)
    {
      /*if (guid == PsiStandartSymbol.Guid)
        return sourceFile => new PsiStandartSymbol(sourceFile);
      if (guid == StandartBindingProperty.Guid)
        return sourceFile => new StandartBindingProperty(sourceFile);
      if (guid == StandartBindingInstanceProperty.Guid)
        return sourceFile => new StandartBindingInstanceProperty(sourceFile);
      if (guid == NameToExpressionBinding.Guid)
        return sourceFile => new NameToExpressionBinding(sourceFile);
      if (guid == NameToPrototypeExpressionBinding.Guid)
        return sourceFile => new NameToPrototypeExpressionBinding(sourceFile);
      if (guid == NameToVariableBinding.Guid)
        return sourceFile => new NameToVariableBinding(sourceFile);*/
      return null;
    }
  }
}
