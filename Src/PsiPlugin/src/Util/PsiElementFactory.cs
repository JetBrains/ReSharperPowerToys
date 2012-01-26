using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Util
{
  public abstract class PsiElementFactory
  {
    public static PsiElementFactory GetInstance([NotNull] IPsiModule module)
    {
      return new PsiElementFactoryImpl(module, true);
    }

    public abstract IRuleName CreateIdentifierExpression(string name);

    public ISolution Solution { get; protected set; }
  }
}
