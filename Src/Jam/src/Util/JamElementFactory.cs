using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.Util
{
  public abstract class JamElementFactory
  {
    protected ISolution Solution { get; set; }

    public static JamElementFactory GetInstance([NotNull] IPsiModule module, bool applyCodeFormatter = false)
    {
      return new JamElementFactoryImpl(module, applyCodeFormatter);
    }

    public static JamElementFactory GetInstance([NotNull] ITreeNode context, bool applyCodeFormatter = false)
    {
      return new JamElementFactoryImpl(context.GetPsiModule(), applyCodeFormatter);
    }

    public abstract IJamFile CreateJamFile(string format, params object[] args);
    public abstract T CreateDeclaration<T>(string format, params object[] args) where T : IJamDeclaration;
    public abstract T CreateStatement<T>(string format, params object[] args) where T : IJamStatement;
    public abstract T CreateExpression<T>(string format, params object[] args) where T : IJamExpression;
  }
}