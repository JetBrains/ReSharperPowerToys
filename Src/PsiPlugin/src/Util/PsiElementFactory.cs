using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Util
{
  public abstract class PsiElementFactory
  {
    protected ISolution Solution { get; set; }

    public static PsiElementFactory GetInstance([NotNull] IPsiModule module)
    {
      return new PsiElementFactoryImpl(module);
    }

    public static PsiElementFactory GetInstance([NotNull] ITreeNode context)
    {
      return new PsiElementFactoryImpl(context.GetPsiModule());
    }

    public abstract IRuleName CreateIdentifierExpression(string name);

    public abstract IRuleDeclaration CreateRuleDeclaration(string name, bool hasBraceParameters = false);

    public abstract IRuleDeclaration CreateRuleDeclaration(string name, bool hasBraceParameters, IList<Pair<string, string>> variableParameters);
  }
}
